using System;
using System.Collections.Concurrent;
using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace _App.Runtime.Web
{
    public class RequestQueueManager : IDisposable
    {
        private readonly ConcurrentQueue<Func<UniTask>> _requestQueue = new ConcurrentQueue<Func<UniTask>>();
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private bool _isProcessing;

        private static bool _simulateDelayInEditor;
        private static float _requestDelay = 0.1f;


        public async UniTask<T> AddRequest<T>(
            string url,
            Action<T> onResult = null,
            CancellationToken cancellationToken = default) where T : class
        {
            var taskCompletionSource = new UniTaskCompletionSource<T>();
            _requestQueue.Enqueue(async () =>
            {
                try
                {
                    var result = await RunRequest<T>(url, cancellationToken).SuppressCancellationThrow();
                    if (result.IsCanceled)
                    {
                        taskCompletionSource.TrySetCanceled();
                        return;
                    }

                    taskCompletionSource.TrySetResult(result.Result);
                    onResult?.Invoke(result.Result);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Request failed: {ex.Message}");
                    taskCompletionSource.TrySetException(ex);
                }
            });

            if (!_isProcessing)
            {
                ProcessQueue().Forget();
            }

            return await taskCompletionSource.Task;
        }
        public void Dispose()
        {
            CancelAllRequests();
        }

        private void CancelAllRequests()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _isProcessing = false;
        }

        private async UniTaskVoid ProcessQueue()
        {
            _isProcessing = true;
            while (_requestQueue.TryDequeue(out var request))
            {
                try
                {
                    await request();
                }
                catch (OperationCanceledException)
                {
                    Debug.Log("Request cancelled.");
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Request failed: {ex.Message}");
                }
            }
            _isProcessing = false;
        }

        private static async UniTask<T> RunRequest<T>(string url, CancellationToken cancellationToken = default) where T : class
        {
            if (cancellationToken == default)
                cancellationToken = Application.exitCancellationToken;
            
#if UNITY_EDITOR
            if (_simulateDelayInEditor)
            {
                Debug.Log($"Simulating request delay in editor. URL: {url}");
                await UniTask.Delay(TimeSpan.FromSeconds(_requestDelay), ignoreTimeScale: true, cancellationToken: cancellationToken);
            }
#endif
            try
            {
                using var request = UnityWebRequest.Get(url);

                var operation = await request.SendWebRequest().WithCancellation(cancellationToken);
                if (operation.result == UnityWebRequest.Result.Success)
                {
                    return JsonConvert.DeserializeObject<T>(request.downloadHandler.text);
                }

                Debug.LogError($"Request to {url} failed with result: {operation.result}");
            }
            catch (OperationCanceledException)
            {
                Debug.Log("Request cancelled.");
                return default;
            }
            catch (JsonSerializationException e)
            {
                Debug.LogError($"Deserialization error: {e.Message}");
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            return null;
        }
    }
}
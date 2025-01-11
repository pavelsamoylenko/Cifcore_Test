using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _App.Runtime.Web
{
    public class RequestQueueManager : IDisposable
    {
        private readonly ConcurrentQueue<RequestWrapper> _requestQueue = new ConcurrentQueue<RequestWrapper>();
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private bool _isProcessing;

        public void AddRequest(Func<UniTask> request, string requestId = null)
        {
            _requestQueue.Enqueue(new RequestWrapper(request, requestId));
            if (!_isProcessing)
            {
                ProcessQueue().Forget();
            }
        }

        public void CancelRequest(string requestId)
        {
            if (requestId == null) return;

            // Создаем временную очередь, исключая элемент с данным requestId
            var newQueue = new ConcurrentQueue<RequestWrapper>(_requestQueue.Where(wrapper => wrapper.Id != requestId));

            // Перезаписываем очередь
            while (_requestQueue.TryDequeue(out _)) { } // Очистка старой очереди
            foreach (var wrapper in newQueue)
            {
                _requestQueue.Enqueue(wrapper); // Заполняем заново
            }
        }

        public void CancelAllRequests()
        {
            _cts.Cancel();
            _isProcessing = false;
            _cts.Dispose();
        }

        public void Dispose()
        {
            CancelAllRequests();
        }

        private async UniTaskVoid ProcessQueue()
        {
            _isProcessing = true;
            while (_requestQueue.TryDequeue(out var requestWrapper))
            {
                try
                {
                    await requestWrapper.Request();
                }
                catch (OperationCanceledException)
                {
                    Debug.Log($"Request {requestWrapper.Id} cancelled.");
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Request {requestWrapper.Id} failed: {ex.Message}");
                }
            }
            _isProcessing = false;
        }

        private class RequestWrapper
        {
            public Func<UniTask> Request { get; }
            public string Id { get; }

            public RequestWrapper(Func<UniTask> request, string id)
            {
                Request = request;
                Id = id;
            }
        }    }
}
using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace _App.Runtime.Web
{
    public class FactService
    {
        private const string FactsApiUrl = "https://dogapi.dog/api/v2/breeds";

        public async UniTask<string[]> GetFactsAsync(CancellationToken cancellationToken)
        {
            try
            {
                using var request = UnityWebRequest.Get(FactsApiUrl);
                var operation = await request.SendWebRequest().WithCancellation(cancellationToken);

                if (operation.result == UnityWebRequest.Result.Success)
                {
                    var json = JObject.Parse(request.downloadHandler.text);
                    var breeds = json["data"];
                    string[] facts = breeds?.Select(b => b["attributes"]["name"]?.ToString()).Take(10).ToArray();
                    return facts;
                }

                Debug.LogError($"Facts API error: {request.error}");
                return Array.Empty<string>();
            }
            catch (OperationCanceledException)
            {
                Debug.Log("Facts request cancelled.");
                return Array.Empty<string>();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Unexpected error: {ex.Message}");
                return Array.Empty<string>();
            }
        }
    }
}
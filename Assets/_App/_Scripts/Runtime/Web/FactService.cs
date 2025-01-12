using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using _App.Runtime.UI.Facts;
using _App.Runtime.Web.DTO;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace _App.Runtime.Web
{
    [UsedImplicitly]
    public class FactService : IFactService
    {
        private const string BaseApiUrl = "https://dogapi.dog/api/v2/";

        private const string BreedsEndpoint = "breeds";


        public async UniTask<List<BreedModel>> GetBreedsListAsync(CancellationToken cancellationToken)
        {
            try
            {
                var url = $"{BaseApiUrl}{BreedsEndpoint}";
                using var request = UnityWebRequest.Get(url);
                var operation = await request.SendWebRequest().WithCancellation(cancellationToken);

                if (operation.result == UnityWebRequest.Result.Success)
                {
                    var response = JsonConvert.DeserializeObject<DTOs.BreedsListResponse>(request.downloadHandler.text);

                    var facts = response.Data.Select(breed => new BreedModel
                        {
                            Id = breed.Id,
                            Name = breed.Attributes.Name,
                            Description = breed.Attributes.Description,
                            Hypoallergenic = breed.Attributes.Hypoallergenic
                        })
                        .ToList();

                    return facts;
                }

                Debug.LogError($"Facts API error: {request.error}");
                return new List<BreedModel>();
            }
            catch (OperationCanceledException)
            {
                Debug.Log("Facts request cancelled.");
                return new List<BreedModel>();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Unexpected error: {ex.Message}");
                return new List<BreedModel>();
            }
        }

        public async UniTask<DTOs.Breed> GetBreedByIdAsync(string id, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Breed ID cannot be null or empty", nameof(id));
            }

            string url = $"{BaseApiUrl}{BreedsEndpoint}/{id}";

            try
            {
                using var request = UnityWebRequest.Get(url);
                var operation = await request.SendWebRequest().WithCancellation(cancellationToken);

                if (operation.result == UnityWebRequest.Result.Success)
                {
                    var breed = JsonConvert.DeserializeObject<DTOs.BreedResponse>(request.downloadHandler.text);
                    return breed.Data;
                }

                Debug.LogError($"Failed to fetch breed with ID {id}: {request.error}");
                return null;
            }
            catch (OperationCanceledException)
            {
                Debug.Log($"Request for breed with ID {id} was cancelled.");
                return null;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Unexpected error while fetching breed with ID {id}: {ex.Message}");
                return null;
            }
        }
    }
}
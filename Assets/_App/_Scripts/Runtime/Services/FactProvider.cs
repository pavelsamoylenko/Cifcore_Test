using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using _App.Runtime.UI.Facts;
using _App.Runtime.Web;
using _App.Runtime.Web.DTO;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

namespace _App.Runtime.Services
{
    [UsedImplicitly]
    public class FactProvider : IFactProvider
    {
        private const string BaseApiUrl = "https://dogapi.dog/api/v2/";
        private const string BreedsEndpoint = "breeds";

        private readonly RequestQueueManager _requestQueueManager;

        public FactProvider(RequestQueueManager requestQueueManager)
        {
            _requestQueueManager = requestQueueManager;
        }
        
        public async UniTask<List<BreedModel>> GetBreedsListAsync(CancellationToken cancellationToken)
        {
            var url = $"{BaseApiUrl}{BreedsEndpoint}";

            var response = await _requestQueueManager.AddRequest<DTOs.BreedsListResponse>(url, cancellationToken: cancellationToken).SuppressCancellationThrow();
            
            if (cancellationToken.IsCancellationRequested)
            {
                throw new OperationCanceledException();
            }

            var breedsListResponse = response.Result;
            
            if (breedsListResponse == null || breedsListResponse.Data == null)
            {
                Debug.LogError("Failed to fetch breeds or received an empty response.");
                return new List<BreedModel>();
            }

            return breedsListResponse.Data.Select(breed => new BreedModel
            {
                Id = breed.Id,
                Name = breed.Attributes.Name,
                Description = breed.Attributes.Description,
                Hypoallergenic = breed.Attributes.Hypoallergenic
            }).ToList();


        }

        
        public async UniTask<DTOs.Breed> GetBreedByIdAsync(string id, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Breed ID cannot be null or empty", nameof(id));
            }

            var url = $"{BaseApiUrl}{BreedsEndpoint}/{id}";

            var response = await _requestQueueManager.AddRequest<DTOs.BreedResponse>(
                url, cancellationToken: cancellationToken).SuppressCancellationThrow();
            
            if (cancellationToken.IsCancellationRequested)
            {
                throw new OperationCanceledException();
            }

            return response.Result?.Data;

        }
    }
}
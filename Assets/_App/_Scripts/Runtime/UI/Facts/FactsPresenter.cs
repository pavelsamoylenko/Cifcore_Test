using System;
using System.Threading;
using _App.Runtime.UI.Popup;
using _App.Runtime.Web;
using _App.Runtime.Web.DTO;
using Cysharp.Threading.Tasks;
using Sirenix.Utilities;
using UniRx;
using UnityEngine;
using Zenject;

namespace _App.Runtime.UI.Facts
{
    public class FactsPresenter : IFactsPresenter
    {
        private readonly IFactService _factsService;
        private readonly RequestQueueManager _queueManager;
        private readonly PopupService _popupService;
        
        private readonly ReactiveCollection<BreedModel> _facts = new();
        private readonly ReactiveProperty<bool> _isLoading = new(false);
        private readonly ReactiveProperty<string> _errorMessage = new(string.Empty);

        public IReadOnlyReactiveCollection<BreedModel> Facts => _facts;
        public IReadOnlyReactiveProperty<bool> IsLoading => _isLoading;
        public IReadOnlyReactiveProperty<string> ErrorMessage => _errorMessage;
        

        private readonly CompositeDisposable _disposable = new();
        private CancellationTokenSource _cancellationTokenSource = new();
        
        private CancellationTokenSource _elementCancellationToken = new();

        [Inject]
        public FactsPresenter(IFactService factsService, RequestQueueManager queueManager, PopupService popupService)
        {
            _factsService = factsService;
            _queueManager = queueManager;
            _popupService = popupService;
        }
        
        
        public void Initialize(IFactsView view)
        {
            Dispose();
            _cancellationTokenSource = new CancellationTokenSource();
            
            view.Clear();
            
            _facts
                .ObserveAdd()
                .Subscribe(change => view.Add(change.Value))
                .AddTo(_disposable);
            
            _facts
                .ObserveRemove()
                .Subscribe(change => view.Remove(change.Value))
                .AddTo(_disposable);
            
            _facts
                .ObserveReset()
                .Subscribe(_ => view.Clear())
                .AddTo(_disposable);
            
            view
                .OnElementClicked
                .Subscribe(ElementClickedHandler)
                .AddTo(_disposable);
            
            _isLoading
                .Subscribe(view.SetLoading)
                .AddTo(_disposable);
            
            _errorMessage
                .Where(x => !string.IsNullOrEmpty(x))
                .Subscribe(view.SetError)
                .AddTo(_disposable);
            
            LoadBreedsList();
        }

        private void LoadBreedsList()
        {
            _isLoading.Value = true;
            _errorMessage.Value = string.Empty;
            _queueManager.AddRequest(LoadFactsCommand);
        }

        private void ElementClickedHandler(FactScrollElementView element)
        {
            _errorMessage.Value = string.Empty;
            
            _elementCancellationToken?.Cancel();
            _elementCancellationToken?.Dispose();
            _elementCancellationToken = new CancellationTokenSource();
            _queueManager.AddRequest(() => LoadSpecificCommand(element, _elementCancellationToken.Token));
        }

        private async UniTask LoadSpecificCommand(FactScrollElementView element, CancellationToken token)
        {
            try
            {
                element.SetLoading(true);
                var fact = await _factsService.GetBreedByIdAsync(element.Id, token);
                element.SetLoading(false);
                
                if (fact.Attributes == null || fact.Attributes.Name.IsNullOrWhitespace() || fact.Attributes.Description.IsNullOrWhitespace())
                {
                    _popupService.ShowConfirmPopup("Error", $"Received Data for\n{element.Id}\nis NULL", "Close");
                    throw new Exception("Received Data is NULL");
                }
                
                var mainText = ComposeBreedDescription(fact);
                _popupService.ShowConfirmPopup($"{fact.Attributes.Name}", mainText, "Close");
            }
            catch (OperationCanceledException)
            {
                _errorMessage.Value = "Request was cancelled";
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                _errorMessage.Value = e.Message;
            }
            finally
            {
                _isLoading.Value = false;
                element.SetLoading(false);
            }
        }

        private async UniTask LoadFactsCommand()
        {
            try
            {
                var breeds = await _factsService.GetBreedsListAsync(_cancellationTokenSource.Token);
                foreach (var breed in breeds)
                {
                    _facts.Add(breed);
                }
            }
            catch (OperationCanceledException)
            {
                _errorMessage.Value = "Request was cancelled";
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                _errorMessage.Value = e.Message;
            }
            finally
            {
                _isLoading.Value = false;
            }
        }

        private static string ComposeBreedDescription(DTOs.Breed breed)
        {
            var attributes = breed.Attributes;
            
            
            string lifeSpan = $"Life Expectancy: {attributes.MinLife}-{attributes.MaxLife} years.";
            string hypoallergenicText = attributes.Hypoallergenic
                ? "This breed is hypoallergenic."
                : "This breed is not hypoallergenic.";

            return $"{attributes.Description}\n\n" +
                   $"{lifeSpan}\n" +
                   $"{hypoallergenicText}";            
        }

        public void Dispose()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            
            _elementCancellationToken?.Cancel();
            _elementCancellationToken?.Dispose();
            
            _cancellationTokenSource = null;
            _elementCancellationToken = null;
            _disposable.Clear();
            
            _popupService.CloseCurrentPopup();
        }
    }
}
using System;
using System.Linq;
using System.Threading;
using _App.Runtime.Services;
using _App.Runtime.UI.Presenters;
using _App.Runtime.Web;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using Zenject;

namespace _App.Runtime.Controllers
{
    [Serializable]
    public class WeatherPresenterSettings
    {
        public float updateInterval = 5f;
    }
    
    public class WeatherPresenter : MonoBehaviour, IDisposable
    {
        private RequestQueueManager _queueManager;
        private IWeatherService _weatherService;
        private ISpriteLoader _spriteLoader;

        [SerializeField] private WeatherPresenterSettings _config;


        private readonly ReactiveProperty<string> _weatherText = new("Загрузка...");
        private readonly ReactiveProperty<bool> _isLoading = new(false);
        private readonly ReactiveProperty<Sprite> _icon = new();


        public IReadOnlyReactiveProperty<string> WeatherText => _weatherText;
        public IReadOnlyReactiveProperty<bool> IsLoading => _isLoading;

        private CancellationTokenSource _weatherCancellationToken;
        
        private readonly CompositeDisposable _disposables = new();
        
        [Inject]
        private void Construct(RequestQueueManager queueManager, IWeatherService weatherService, ISpriteLoader spriteLoader)
        {
            _queueManager = queueManager;
            _weatherService = weatherService;
            _spriteLoader = spriteLoader;
        }

        public void Initialize(IWeatherView view)
        {
            _disposables.Clear();
            
            _weatherText
                .Subscribe(view.SetTemperature)
                .AddTo(_disposables);
            
            _isLoading
                .Where(_ => !_icon.HasValue)
                .Subscribe(view.SetLoading)
                .AddTo(_disposables);
            
            _icon
                .Where(x => x != null)
                .Subscribe(view.SetIcon)
                .AddTo(_disposables);
        }

        public void StartWeatherUpdates()
        {
            _weatherCancellationToken?.Dispose();
            _weatherCancellationToken = new CancellationTokenSource();
            
            _isLoading.Value = true;
            
            Observable
                .Interval(TimeSpan.FromSeconds(_config.updateInterval))
                .Subscribe( _ => RequestWeatherUpdate())
                .AddTo(_weatherCancellationToken.Token);
        }

        public void StopWeatherUpdates()
        {
            _weatherCancellationToken?.Cancel();
            _weatherCancellationToken?.Dispose();
            _weatherCancellationToken = null;
            _isLoading.Value = false;
        }

        private void RequestWeatherUpdate()
        {
            _isLoading.Value = true;
            _queueManager.AddRequest(UpdateWeather);
        }
        
        private async UniTask UpdateWeather()
        {
            var weather = await _weatherService.GetWeatherForecastAsync(_weatherCancellationToken.Token).SuppressCancellationThrow();
            if(weather.IsCanceled) return;
            var todayWeather = weather.Result.FirstOrDefault();
            if (todayWeather != null)
            {
                _weatherText.Value = $"{todayWeather.Temperature}°F";
                var icon = await _spriteLoader.LoadSpriteAsync(todayWeather.IconUrl, _weatherCancellationToken.Token).SuppressCancellationThrow();
                if(icon.IsCanceled) return;
                _icon.Value = icon.Result;
                _isLoading.Value = false;
            }
            else
            {
                _weatherText.Value = "No Data for today";
                _isLoading.Value = false;
            }
        }

        public void Dispose()
        {
            StopWeatherUpdates();
            _disposables.Clear();
        }
    }
    
}
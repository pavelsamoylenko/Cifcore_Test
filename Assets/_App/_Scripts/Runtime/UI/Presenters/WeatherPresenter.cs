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
        private IWeatherProvider _weatherProvider;
        private ISpriteLoader _spriteLoader;

        [SerializeField] private WeatherPresenterSettings _config;


        private readonly ReactiveProperty<string> _weatherDateText = new("...");
        private readonly ReactiveProperty<string> _temperatureText = new("...");
        private readonly ReactiveProperty<bool> _isLoading = new(false);
        private readonly ReactiveProperty<Sprite> _icon = new();


        public IReadOnlyReactiveProperty<string> TemperatureDateText => _temperatureText;
        public IReadOnlyReactiveProperty<string> TemperatureText => _temperatureText;
        public IReadOnlyReactiveProperty<bool> IsLoading => _isLoading;

        private CancellationTokenSource _weatherCancellationToken;
        
        private readonly CompositeDisposable _disposables = new();
        
        [Inject]
        private void Construct(IWeatherProvider weatherProvider, ISpriteLoader spriteLoader)
        {
            _weatherProvider = weatherProvider;
            _spriteLoader = spriteLoader;
        }

        public void Initialize(IWeatherView view)
        {
            _disposables.Clear();
            
            _temperatureText
                .Subscribe(view.SetTemperature)
                .AddTo(_disposables);
            
            _weatherDateText
                .Subscribe(view.SetDateText)
                .AddTo(_disposables);
            
            _isLoading
                .Subscribe(view.SetLoading)
                .AddTo(_disposables);
            
            _icon
                .Where(x => x != default)
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
                .AddTo(_disposables);
        }

        public void StopWeatherUpdates()
        {
            _weatherCancellationToken?.Cancel();
            _weatherCancellationToken?.Dispose();
            _weatherCancellationToken = null;
            _disposables.Clear();
            
            _isLoading.Value = false;
        }

        private void RequestWeatherUpdate()
        {
            _isLoading.Value = true;
            UpdateWeather().Forget();
        }
        
        private async UniTask UpdateWeather()
        {
            var weather = await _weatherProvider.GetWeatherForecastAsync(_weatherCancellationToken.Token).SuppressCancellationThrow();
            if(weather.IsCanceled) return;
            var todayWeather = weather.Result.FirstOrDefault();
            if (todayWeather != null)
            {
                _weatherDateText.Value = todayWeather.Name;
                _temperatureText.Value = $"{todayWeather.Temperature}°F";
                var icon = await _spriteLoader.LoadSpriteAsync(todayWeather.IconUrl, _weatherCancellationToken.Token).SuppressCancellationThrow();
                if(icon.IsCanceled) return;
                _icon.Value = icon.Result;
                _isLoading.Value = false;
            }
            else
            {
                _temperatureText.Value = "No Data for today";
                _isLoading.Value = false;
            }
        }

        public void Dispose()
        {
            StopWeatherUpdates();
        }
    }
    
}
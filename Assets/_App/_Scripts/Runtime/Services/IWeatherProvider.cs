using System.Collections.Generic;
using System.Threading;
using _App.Runtime.UI.Weather.Models;
using Cysharp.Threading.Tasks;

namespace _App.Runtime.Services
{
    public interface IWeatherProvider
    {
        UniTask<IList<WeatherForecast>> GetWeatherForecastAsync(CancellationToken cancellationToken);
    }
}
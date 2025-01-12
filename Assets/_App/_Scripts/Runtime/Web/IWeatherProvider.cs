using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace _App.Runtime.Web
{
    public interface IWeatherProvider
    {
        UniTask<IList<WeatherForecast>> GetWeatherForecastAsync(CancellationToken cancellationToken);
    }
}
using System;
using System.Collections.Generic;
using System.Threading;
using _App.Runtime.UI.Weather.Models;
using _App.Runtime.Web;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace _App.Runtime.Services
{
    [UsedImplicitly]
    public class WeatherProvider  : IWeatherProvider
    {
        private const string WeatherApiUrl = "https://api.weather.gov/gridpoints/TOP/32,81/forecast";

        private readonly RequestQueueManager _requestQueueManager;

        public WeatherProvider(RequestQueueManager requestQueueManager)
        {
            _requestQueueManager = requestQueueManager;
        }

        /// <summary>
        /// Получить прогноз погоды, используя RequestQueueManager.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены запроса.</param>
        /// <returns>Список прогнозов погоды.</returns>
        public async UniTask<IList<WeatherForecast>> GetWeatherForecastAsync(CancellationToken cancellationToken)
        {
            var response = await _requestQueueManager.AddRequest<JObject>(
                    WeatherApiUrl,
                    onResult: null, // Callback не нужен, обработка ниже
                    cancellationToken: cancellationToken)
                .SuppressCancellationThrow();

            if (cancellationToken.IsCancellationRequested)
            {
                throw new OperationCanceledException();
            }
            
            if (response.Result == null)
            {
                Debug.LogError("Не удалось получить данные прогноза погоды.");
                return new List<WeatherForecast>();
            }

            return ParseWeatherForecasts(response.Result);
        }

        /// <summary>
        /// Парсинг данных прогноза погоды из JSON.
        /// </summary>
        /// <param name="response">Объект JSON.</param>
        /// <returns>Список прогнозов погоды.</returns>
        private static IList<WeatherForecast> ParseWeatherForecasts(JObject response)
        {
            if (response["properties"]?["periods"] == null)
            {
                Debug.LogError("Некорректный формат ответа от API погоды.");
                return new List<WeatherForecast>();
            }

            var periods = response["properties"]["periods"];
            var forecasts = new List<WeatherForecast>();

            foreach (var period in periods)
            {
                var forecast = new WeatherForecast
                {
                    Name = period["name"]?.ToString(),
                    StartTime = DateTime.Parse(period["startTime"]?.ToString() ?? ""),
                    EndTime = DateTime.Parse(period["endTime"]?.ToString() ?? ""),
                    Temperature = int.Parse(period["temperature"]?.ToString() ?? "0"),
                    TemperatureUnit = period["temperatureUnit"]?.ToString(),
                    IconUrl = period["icon"]?.ToString(),
                    ShortForecast = period["shortForecast"]?.ToString(),
                    DetailedForecast = period["detailedForecast"]?.ToString(),
                };
                forecasts.Add(forecast);
            }

            return forecasts;
        }
    }
}
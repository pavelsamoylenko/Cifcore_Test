using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class WeatherModel
{
    private const string WeatherApiUrl = "https://api.weather.gov/gridpoints/TOP/32,81/forecast";

    public async UniTask<List<WeatherForecast>> GetWeatherForecastAsync(CancellationToken cancellationToken)
    {
        try
        {
            using var request = UnityWebRequest.Get(WeatherApiUrl);
            var operation = await request.SendWebRequest().WithCancellation(cancellationToken);

            if (operation.result == UnityWebRequest.Result.Success)
            {
                var json = JObject.Parse(request.downloadHandler.text);
                var periods = json["properties"]?["periods"];

                if (periods != null)
                {
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

            Debug.LogError($"Ошибка получения данных с API: {request.error}");
            return new List<WeatherForecast>();
        }
        catch (OperationCanceledException)
        {
            Debug.Log("Запрос отменен.");
            return new List<WeatherForecast>();
        }
        catch (Exception ex)
        {
            Debug.LogError($"Неожиданная ошибка: {ex.Message}");
            return new List<WeatherForecast>();
        }
    }
}

public class WeatherForecast
{
    public string Name { get; set; } 
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int Temperature { get; set; }
    public string TemperatureUnit { get; set; }
    public string IconUrl { get; set; }
    public string ShortForecast { get; set; }
    public string DetailedForecast { get; set; }
}
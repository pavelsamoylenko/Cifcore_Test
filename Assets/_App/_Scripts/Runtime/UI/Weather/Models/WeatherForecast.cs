using System;

namespace _App.Runtime.UI.Weather.Models
{
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
}
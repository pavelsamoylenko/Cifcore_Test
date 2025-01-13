using UnityEngine;

namespace _App.Runtime.UI.Weather
{
    public interface IWeatherView
    {
        void SetTemperature(string temperature);
        void SetIcon(Sprite icon);
        void SetLoading(bool isLoading);
        void SetDateText(string date);
    }
}
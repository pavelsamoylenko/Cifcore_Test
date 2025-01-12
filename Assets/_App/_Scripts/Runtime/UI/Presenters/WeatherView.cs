using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _App.Runtime.UI.Presenters
{
    public class WeatherView : MonoBehaviour, IWeatherView
    {
        [SerializeField, Required] private TMP_Text _temperatureText;
        [SerializeField, Required] private Image _icon;
        [SerializeField, Required] private Sprite _loadingIcon;
        
        private Sprite _lastIcon;

        public void SetTemperature(string temperature)
        {
            _temperatureText.text = temperature;
        }
        
        public void SetIcon(Sprite icon)
        {
            _icon.sprite = icon;
            _lastIcon = icon;
        }
        
        public void SetLoading(bool isLoading)
        {
            _icon.sprite = isLoading ? _loadingIcon : _lastIcon;
        }
    }
}
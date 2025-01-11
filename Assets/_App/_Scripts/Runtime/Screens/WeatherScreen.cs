using _App.Runtime.Controllers;
using _App.Runtime.UI.Base;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _App.Runtime.Screens
{
    public class WeatherScreen : BaseScreen
    {
        [SerializeField, Required] private WeatherPresenter _presenter;
        [SerializeField, Required] private WeatherView _view;
        protected override void OnWillShow()
        {
            base.OnWillShow();
            _presenter.Initialize(_view);
            _presenter.StartWeatherUpdates();
        }
        
        protected override void OnHide()
        {
            base.OnHide();
            _presenter.StopWeatherUpdates();
        }

        private void OnDestroy()
        {
            _presenter.Dispose();
        }
    }

}
using System;
using _App.Runtime.UI.Base;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace _App.Runtime.UI
{
    /*
    public class ScreensManager
    {
        private BaseScreen _currentScreen;

        public async UniTask<T> ShowScreen<T>(Action<DiContainer> contextDecorator = null) where T : BaseScreen
        {
            var type = typeof(T);

            if (_currentScreen != null)
            {
                ClearCurrentScreen(hideAnimate, showFader, savePreviousToHistory);
                _currentScreen.CanvasGroup.blocksRaycasts = false;
            }

            
        }
        
        private void ClearCurrentScreen(bool hideAnimate, bool showFader, bool saveToHistory)
        {
            _currentScreen.StopInteraction();

            _currentScreen.GetComponentInParent<Canvas>().sortingOrder = 0;

            if (hideAnimate && _currentScreen != null)
                _currentScreen.Hide();

            if (showFader)
                _fadeScreen.Show();

            if (_currentScreen.ShowHeader)
            {
                _headerScreen.HideAllButtons();
                _headerDisposable.Clear();
            }
            
            if (saveToHistory)
            {
                if (_screenHistory.Count == 0 || !_screenHistory.Peek().IsEquivalentTo(_currentScreen.GetType()))
                    _screenHistory.Push(_currentScreen.GetType());
            }
        }
    }
*/
}
using UnityEngine;
using Zenject;

namespace _App.Runtime.UI.Popup
{
    public class PopupService : MonoBehaviour
    {
        private IInstantiator _instantiator;
        
        [SerializeField] private SimpleConfirmPopupView _confirmPopupPrefab;
        [SerializeField] private Transform _popupContainer;
        
        private SimpleConfirmPopupView _currentPopup;
        
        
        [Inject]
        private void Construct(IInstantiator instantiator)
        {
            _instantiator = instantiator;
        }
        
        public void ShowConfirmPopup(string title, string mainText, string buttonText)
        {
            var popupInstance = _instantiator.InstantiatePrefabForComponent<SimpleConfirmPopupView>(_confirmPopupPrefab, _popupContainer);
            popupInstance.Initialize(title, mainText, buttonText, () => popupInstance.Close(), () => popupInstance.Close());
            _currentPopup = popupInstance;
        }
        
        public void CloseCurrentPopup()
        {
            if (_currentPopup != null)
            {
                _currentPopup.Close();
            }
        }
    }
}
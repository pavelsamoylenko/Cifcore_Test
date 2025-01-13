using _App.Runtime.Screens.Base;
using _App.Runtime.UI.Facts;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace _App.Runtime.Screens
{
    public class FactsScreen : BaseScreen
    {
        [Inject] private IFactsPresenter _presenter;
        
        [SerializeField, Required] private FactsView _view;
        
        protected override void OnWillShow()
        {
            base.OnWillShow();
            _presenter.Initialize(_view);
        }
        
        protected override void OnHide()
        {
            base.OnHide();
            _presenter.Dispose();
        }

        private void OnDestroy()
        {
            _presenter.Dispose();
        }
    }
}
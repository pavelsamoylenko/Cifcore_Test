using System;
using _App.Runtime.UI;
using _App.Runtime.UI.Data;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using Zenject;

namespace _App.Runtime.Screens.Base
{
    public abstract class BaseScreen : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] private string id;
        
        [Header("Dependencies")]
        [SerializeField, Required] private CanvasGroup _canvasGroup;
        [SerializeField, Required] protected NavigationBarButtonData buttonData;
        
        [Header("Runtime set")]
        [SerializeField] protected NavigationMenuController parentController;

        public string Id => id;

        public NavigationBarButtonData ButtonData => buttonData;

        public CanvasGroup CanvasGroup => _canvasGroup;
        
        private readonly Subject<Unit> _onShow = new Subject<Unit>();
        private readonly Subject<Unit> _onWillShow = new Subject<Unit>();
        private readonly Subject<Unit> _onHide = new Subject<Unit>();
        private readonly Subject<Unit> _onWillHide = new Subject<Unit>();

        public IObservable<Unit> OnShowObservable => _onShow;
        public IObservable<Unit> OnWillShowObservable => _onWillShow;
        public IObservable<Unit> OnHideObservable => _onHide;
        public IObservable<Unit> OnWillHideObservable => _onWillHide;

        [Inject]
        private void Construct(NavigationMenuController menuController)
        {
            parentController = menuController;
        }

        public virtual void Show()
        {
            _onWillShow.OnNext(Unit.Default);
            OnWillShow();
            gameObject.SetActive(true);
            _onShow.OnNext(Unit.Default);
            OnShow();
        }

        public virtual void Hide()
        {
            _onWillHide.OnNext(Unit.Default);
            OnWillHide();
            gameObject.SetActive(false);
            _onHide.OnNext(Unit.Default);
            OnHide();
        }
        
        public void StopInteraction()
        {
            CanvasGroup.blocksRaycasts = false;
        }
        
        protected virtual void OnShow() { }
        protected virtual void OnWillShow() { }
        protected virtual void OnHide() { }
        protected virtual void OnWillHide() { }
    }
}
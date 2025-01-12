using _App.Runtime.Services;
using _App.Runtime.UI.Data;
using _App.Runtime.UI.Factories;
using _App.Runtime.UI.Popup;
using _App.Runtime.Web;
using UnityEngine;
using Zenject;

namespace _App.Runtime
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private ScreensDatabase screensDatabase;
        [SerializeField] private PopupService popupService;
        
        
        public override void InstallBindings()
        {
            Container.BindInstance(screensDatabase).AsSingle();
            BindServices();
        }

        private void BindServices()
        {
            Container.BindInterfacesAndSelfTo<RequestQueueManager>().FromNew().AsSingle().NonLazy();
            Container.Bind<ISpriteLoader>().To<WebSpriteLoader>().AsCached().NonLazy();
            Container.Bind<PopupService>().FromComponentInNewPrefab(popupService).AsSingle().NonLazy();
        }
    }
}
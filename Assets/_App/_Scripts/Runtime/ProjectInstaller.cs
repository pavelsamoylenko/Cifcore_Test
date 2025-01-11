using _App.Runtime.Services;
using _App.Runtime.UI.Data;
using _App.Runtime.UI.Factories;
using _App.Runtime.Web;
using UnityEngine;
using Zenject;

namespace _App.Runtime
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private ScreensDatabase screensDatabase;
        public override void InstallBindings()
        {
            Container.Bind<ScreenFactory>().AsSingle().NonLazy();
            Container.BindInstance(screensDatabase).AsSingle();
            
            BindServices();
        }

        private void BindServices()
        {
            Container.Bind<ISpriteLoader>().To<WebSpriteLoader>().AsCached().NonLazy();
            Container.Bind<WeatherService>().FromNew().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<RequestQueueManager>().FromNew().AsSingle().NonLazy();
            
        }
    }
}
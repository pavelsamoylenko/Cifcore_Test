using _App.Runtime.Web;
using Zenject;

namespace _App.Runtime
{
    public class MainSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IWeatherProvider>().To<WeatherProvider>().FromNew().AsSingle().NonLazy();
            Container.Bind<IFactProvider>().To<FactProvider>().FromNew().AsSingle().NonLazy();
        }

    }
}
using _App.Runtime.Services;
using Zenject;

namespace _App.Runtime.Installers
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
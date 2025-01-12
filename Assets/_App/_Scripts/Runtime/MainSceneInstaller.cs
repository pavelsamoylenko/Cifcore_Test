using _App.Runtime.Web;
using Zenject;

namespace _App.Runtime
{
    public class MainSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IWeatherService>().To<WeatherService>().FromNew().AsSingle().NonLazy();
            Container.Bind<IFactService>().To<FactService>().FromNew().AsSingle().NonLazy();
        }

    }
}
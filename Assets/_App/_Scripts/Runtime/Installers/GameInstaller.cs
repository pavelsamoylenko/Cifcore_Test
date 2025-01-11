using _App.Runtime.Web;
using Zenject;

namespace _App.Runtime.Installers
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<RequestQueueManager>().AsSingle().NonLazy();
            Container.Bind<WeatherService>().AsSingle();
            Container.Bind<FactService>().AsSingle();
        }
    }
}
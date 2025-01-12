using _App.Runtime.UI.Facts;
using Zenject;

namespace _App.Runtime.Installers
{
    public class FactsScreenInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IFactsPresenter>().To<FactsPresenter>().AsSingle().NonLazy();
        }
    }
}
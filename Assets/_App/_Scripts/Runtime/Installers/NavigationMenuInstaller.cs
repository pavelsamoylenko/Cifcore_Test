using _App.Runtime.UI;
using UnityEngine;
using Zenject;

namespace _App.Runtime.Installers
{
    public class NavigationMenuInstaller : MonoInstaller
    {
        [SerializeField] private NavigationMenuController navigationMenuController;
        
        public override void InstallBindings()
        {
            Container.Bind<NavigationMenuController>().FromInstance(navigationMenuController).AsSingle();
        }
    }
}
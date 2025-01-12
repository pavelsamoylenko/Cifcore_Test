using _App.Runtime.UI.Factories;
using _App.Runtime.UI.Facts;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace _App.Runtime
{
    public class UIInstaller : MonoInstaller
    {
        [Header("Pool Elements")]
        [SerializeField, Required] private FactScrollElementView _factScrollElementViewPrefab;

        public override void InstallBindings()
        {
            Container.Bind<ScreenFactory>().AsSingle().NonLazy();
            
            BindPools();
        }

        private void BindPools()
        {
            Container.BindMemoryPool<FactScrollElementView, FactScrollElementView.Pool>()
                .WithInitialSize(10)
                .FromComponentInNewPrefab(_factScrollElementViewPrefab)
                .UnderTransformGroup("FactScrollElementPool");
        }
    }
}
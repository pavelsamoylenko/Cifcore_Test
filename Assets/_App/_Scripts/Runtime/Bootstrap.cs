using System.Linq;
using _App.Runtime.UI;
using _App.Runtime.UI.Data;
using _App.Runtime.UI.Factories;
using UnityEngine;
using Zenject;

namespace _App.Runtime
{
    public class Bootstrap : MonoBehaviour
    {
        [Inject] private ScreenFactory _factory;

        [SerializeField] private NavigationMenuController navigationMenuController;
        [SerializeField] private ScreensSetup setup;

        private void Start()
        {
            FillScreens();
            navigationMenuController.ActivateScreen(setup.ScreensIds.Last());
        }

        private void FillScreens()
        {
            var requestedScreensIds = setup.ScreensIds;
            foreach (string screenId in requestedScreensIds)
            {
                navigationMenuController.RegisterScreen(screenId);
            }
        }
    }
}
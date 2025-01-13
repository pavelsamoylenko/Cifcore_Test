using System.Linq;
using _App.Runtime.Screens.Data;
using _App.Runtime.UI;
using _App.Runtime.UI.Data;
using UnityEngine;

namespace _App.Runtime
{
    public class Bootstrap : MonoBehaviour
    {

        [SerializeField] private NavigationMenuController navigationMenuController;
        [SerializeField] private ScreensSetup setup;

        private void Start()
        {
            FillScreens();
            navigationMenuController.ActivateScreen(setup.ScreensIds.First());
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
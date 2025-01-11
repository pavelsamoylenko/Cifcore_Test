using _App.Runtime.UI.Base;
using _App.Runtime.UI.Elements;
using UnityEngine;
using UnityEngine.UI;

namespace _App.Runtime.UI
{
    public class NavigationBarController : MonoBehaviour
    {
        [SerializeField] private NavigationBarButton buttonPrefab;
        [SerializeField] private ToggleGroup toggleGroup;
        public NavigationBarButton AddButtonForScreen(BaseScreen screen)
        {
            var button = Instantiate(buttonPrefab, transform);
            
            button.Color = screen.ButtonData.Color;
            button.ToggleGroup = toggleGroup;
            
            return button;
        }
    }
}
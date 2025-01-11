using System.Collections.Generic;
using System.Linq;
using _App.Runtime.UI.Base;
using _App.Runtime.UI.Elements;
using _App.Runtime.UI.Factories;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace _App.Runtime.UI
{
    public class NavigationMenuController : MonoBehaviour
    {
        [Inject] private ScreenFactory _screenFactory;
        
        [SerializeField] private NavigationBarController bar;
        [SerializeField] private Transform screensRoot;
        
        private readonly Dictionary<string, BaseScreen> _screens = new();
        private readonly Dictionary<BaseScreen, NavigationBarButton> _buttons = new();
        
        public IReadOnlyList<BaseScreen> CurrentScreens => _screens.Values.ToList(); 

        public Transform ScreensRoot => screensRoot;

        private void Awake()
        {
            InitializeCurrentScreens();
        }

        public void RegisterScreen(string screenId)
        {
            var screen = _screenFactory.Create(screenId, this);
            
            if (screen == null)
            {
                Debug.LogWarning($"Screen with id {screenId} was not created");
                return;
            }
            
            RegisterScreenInstance(screen);
        }

        private void RegisterScreenInstance(BaseScreen screen)
        {
            var screenId = screen.Id;
            if (!_screens.TryAdd(screenId, screen))
            {
                Debug.LogWarning($"Screen with id:{screenId} already exists");
                Destroy(screen.gameObject);
                screen = _screens[screenId];
            }


            if (!_buttons.ContainsKey(screen))
            {
                var button = bar.AddButtonForScreen(screen);

                _buttons.Add(screen, button);

                button.Toggled
                    .AsObservable()
                    .Subscribe(_ => OnToggle(screen, _))
                    .AddTo(screen);

                screen
                    .OnDestroyAsObservable()
                    .Subscribe(_ => Destroy(_buttons[screen].gameObject))
                    .AddTo(button);
            }

            screen.Hide();
        }

        public bool UnregisterScreen(string screenId)
        {
            if (_screens.TryGetValue(screenId, out var screen))
            {
                screen.Hide();
                _screens.Remove(screenId);
                Destroy(screen.gameObject);
                return true;
            }

            Debug.LogWarning($"Cannot unregister screen with id {screenId} because it's not registered");
            return false;
        }

        public void ActivateScreen(string screenId)
        {
            if(_screens.TryGetValue(screenId, out var screen))
            {
                ActivateScreen(screen);
            }
            else
            {
                Debug.LogWarning($"No screen with Id {screenId} is in menu");
            }
        }

        private void OnToggle(BaseScreen screenBase, bool toggleValue)
        {
            if (toggleValue) 
                screenBase.Show();
            else 
                screenBase.Hide();
        }

        private void ActivateScreen(BaseScreen screen)
        {
            _buttons[screen].Select();
        }

        private void InitializeCurrentScreens()
        {
            var currentButtons = screensRoot.GetComponentsInChildren<NavigationBarButton>(true);
            
            foreach (var button in currentButtons)
            {
                Destroy(button.gameObject);
            }
            
            var currentScreens = screensRoot.GetComponentsInChildren<BaseScreen>(true);
            foreach (var screen in currentScreens)
            {
                RegisterScreenInstance(screen);
            }
        }
    }
}
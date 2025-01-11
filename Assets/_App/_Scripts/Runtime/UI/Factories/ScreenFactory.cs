using System.Collections.Generic;
using _App.Runtime.UI.Base;
using _App.Runtime.UI.Data;
using UnityEngine;
using Zenject;

namespace _App.Runtime.UI.Factories
{
    public class ScreenFactory
    {
        private readonly IInstantiator _instantiator;

        private readonly Dictionary<string, BaseScreen> _availableScreens;

        [Inject]
        public ScreenFactory(IInstantiator instantiator, ScreensDatabase data)
        {
            _instantiator = instantiator;
            
            _availableScreens = new Dictionary<string, BaseScreen>();
            
            foreach (var screen in data.Screens)
            {
                _availableScreens.TryAdd(key: screen.Id, screen);
            }
        }

        public BaseScreen Create(string id, NavigationMenuController menu)
        {
            if (_availableScreens.TryGetValue(id, out var screenPrefab))
            {
                screenPrefab.gameObject.SetActive(false);
                var screen = _instantiator.InstantiatePrefabForComponent<BaseScreen>(screenPrefab, menu.ScreensRoot, new [] {menu});
                return screen;
            }
            Debug.LogError($"Failed to create screen with id {id}");
            return null;
        }

    }
}
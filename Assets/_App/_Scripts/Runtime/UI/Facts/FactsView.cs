using System;
using System.Collections.Generic;
using _App.Runtime.UI.Utils;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using Zenject;

namespace _App.Runtime.UI.Facts
{
    public class FactsView : MonoBehaviour, IFactsView
    {   
        [Inject] private FactScrollElementView.Pool _elementPool;

        [SerializeField, Required] private CanvasGroupActivator _loadingElement;
        [SerializeField, Required] private CanvasGroupActivator _contentContainerCG;
        [SerializeField,Required] private Transform _elementsContainer;
        
        private readonly Dictionary<string, FactScrollElementView> _spawnedElements = new();
        
        private readonly Subject<FactScrollElementView> _onElementClicked = new();

        public IObservable<FactScrollElementView> OnElementClicked => _onElementClicked;

        public void Add(BreedModel fact)
        {
            var element = _elementPool.Spawn(_elementsContainer,fact);
            _spawnedElements.Add(fact.Id, element);
            element.SetNumber(_spawnedElements.Count.ToString());
            element.Button.onClick.AddListener(() => _onElementClicked?.OnNext(element));
        }
        
        public void Remove(BreedModel fact)
        {
            if (_spawnedElements.TryGetValue(fact.Id, out var element))
            {
                _elementPool.Despawn(element);
                _spawnedElements.Remove(fact.Id);
            }            
        }
        
        public void Clear()
        {
            foreach (var element in _spawnedElements.Values)
            {
                _elementPool.Despawn(element);
            }
            _spawnedElements.Clear();
        }
        
        public void SetLoading(bool isLoading)
        {
            _loadingElement.SetActive(isLoading);
            _contentContainerCG.SetActive(!isLoading);
        }

        public void SetError(string errorMessage)
        {
            Debug.LogError(errorMessage);
        }
        
    }
}
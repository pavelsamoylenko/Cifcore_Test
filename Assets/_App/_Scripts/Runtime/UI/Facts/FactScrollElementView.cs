using System;
using _App.Runtime.UI.Utils;
using Sirenix.OdinInspector;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _App.Runtime.UI.Facts
{
    public class FactScrollElementView :  MonoBehaviour
    {
        [SerializeField, Required] private TMP_Text _numberText;
        [SerializeField, Required] private TMP_Text _factNameText;
        [SerializeField, Required] private CanvasGroupActivator _loadingImage;
        [SerializeField, Required] private Button _button;
        
        private BreedModel _model;
        
        public Button Button => _button;
        public string Id => _model.Id;

        private void Awake()
        {
            _loadingImage.SetActive(false);
        }
        
        public void SetNumber(string number)
        {
            _numberText.text = number;
        }

        public void SetLoading(bool isLoading)
        {
            _button.interactable = !isLoading;
            _loadingImage.SetActive(isLoading);
        }

        public class Pool : MonoMemoryPool<Transform, BreedModel, FactScrollElementView>
        {
            protected override void Reinitialize(Transform parent, BreedModel model, FactScrollElementView item)
            {
                item._model = model;
                item._factNameText.text = model.Name;
                item.transform.SetParent(parent);
                item.Button.onClick.RemoveAllListeners();
            }
            
            protected override void OnDespawned(FactScrollElementView item)
            {
                item._numberText.text = string.Empty;
                item._factNameText.text = string.Empty;

                base.OnDespawned(item);
            }
        }
    }
}
using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace _App.Runtime.UI.Elements
{
    public class NavigationBarButton : MonoBehaviour
    {
        [SerializeField] private Image toggleImage;
        [SerializeField] private Toggle toggle;

        public IObservable<bool> Toggled => toggle.OnValueChangedAsObservable();

        internal Color Color
        {
            get => toggle.image.color;
            set => toggle.image.color = value;
        }

        internal ToggleGroup ToggleGroup
        {
            set => toggle.group = value;
        }

        internal void Select()
        {
            if (toggle.isOn) toggle.isOn = false; // For force toggle notification
            toggle.isOn = true;
        }
        internal void SetIcon(Sprite icon = null)
        {
            toggleImage.sprite = icon;
            toggleImage.color = icon != null ?  Color.white : Color.clear;
        }
    }
}
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _App.Runtime.UI.Utils
{
    [RequireComponent(typeof(CanvasGroup))]
    public class CanvasGroupActivator : MonoBehaviour
    {
        [SerializeField] private float _fadeDuration = 0.3f;
        [SerializeField, Required] private CanvasGroup _canvasGroup;
        
        private Tween _cgTween;
        
        private void Awake()
        {
            _canvasGroup ??= GetComponent<CanvasGroup>();
        }

        public void SetActive(bool active, bool instant = false)
        {
            _cgTween?.Kill();
            
            if(active) gameObject.SetActive(true);
            _cgTween = _canvasGroup
                .DOFade(active ? 1f : 0f, instant? 0f : _fadeDuration)
                .OnComplete(() =>
                {
                    if (!active) gameObject.SetActive(false);
                })
                .SetLink(gameObject);
        }

    }
}
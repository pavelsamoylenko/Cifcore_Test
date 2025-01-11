using _App.Runtime.UI.Base;
using DG.Tweening;
using UnityEngine;

namespace _App.Runtime.UI
{
    public class FadingScreen : BaseScreen
    {

        [SerializeField] private float fadeInDuration = 0.25f;
        [SerializeField] private float fadeOutDuration = 0.1f;
        [SerializeField] private CanvasGroup canvasGroup;
        
        private Tween _tween;

        public override void Show()
        {
            _tween?.Kill();
            
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 0f;
            
            gameObject.SetActive(true);
            
            _tween = canvasGroup.DOFade(1f, fadeInDuration).SetEase(Ease.OutQuad).SetLink(gameObject);
        }
        public override void Hide()
        {
            _tween?.Kill();
            
            canvasGroup.blocksRaycasts = false;
            
            _tween = canvasGroup.DOFade(0f, fadeOutDuration).SetEase(Ease.InQuad).OnComplete(() => gameObject.SetActive(false)).SetLink(gameObject);
        }
    }
}
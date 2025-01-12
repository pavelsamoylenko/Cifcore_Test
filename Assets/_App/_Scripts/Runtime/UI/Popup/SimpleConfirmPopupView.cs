using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _App.Runtime.UI.Popup
{
    public class SimpleConfirmPopupView : MonoBehaviour
    {
        [SerializeField, Required] private TMP_Text _titleText;
        [SerializeField, Required] private TMP_Text _mainText;
        [SerializeField, Required] private TMP_Text _buttonText;
        [SerializeField, Required] private Button _confirm;
        [SerializeField, Required] private Button _backGroundButton;


        /// <summary>
        /// Инициализация данных в попапе.
        /// </summary>
        /// <param name="title">Заголовок окна.</param>
        /// <param name="mainText">Основной текст окна.</param>
        /// <param name="buttonText">Текст кнопки.</param>
        /// <param name="onConfirm">Действие при закрытии окна через кнопку.</param>
        /// <param name="onClose">Действие при закрытии окна через клик на background.</param>
        public void Initialize(string title, string mainText, string buttonText, System.Action onConfirm = null, System.Action onClose = null)
        {
            _titleText.text = title;
            _mainText.text = mainText;
            _buttonText.text = buttonText;

            _confirm.onClick.RemoveAllListeners();
            _confirm.onClick.AddListener(() =>
            {
                onConfirm?.Invoke();
            });
        
            _backGroundButton.onClick.RemoveAllListeners();
            _backGroundButton.onClick.AddListener(() =>
            {
                onClose?.Invoke();
            });
        }
        
        public void Close()
        {
            Destroy(gameObject);
        }
    }
}
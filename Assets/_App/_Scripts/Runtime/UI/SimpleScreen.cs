using _App.Runtime.UI.Base;

namespace _App.Runtime.UI
{
    public class SimpleScreen : BaseScreen
    {
        public override void Show()
        {
            gameObject.SetActive(true);
        }
        public override void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
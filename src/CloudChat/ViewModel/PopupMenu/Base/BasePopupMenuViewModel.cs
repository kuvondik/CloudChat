using CloudChat.Core;
namespace CloudChat
{
    public class BasePopupMenuViewModel : BaseViewModel
    {
        #region Public Properties
        public string BubbleBackground { get; set; }

        public ElementHorizontalAlignment ArrowAlignment { get; set; }
        public MenuViewModel Menu { get; set; }
        public BaseViewModel Content { get; set; }
        #endregion

        #region Contructor
        public BasePopupMenuViewModel()
        {
            BubbleBackground = "ffffff";
            ArrowAlignment = ElementHorizontalAlignment.Left;
        }
        #endregion
    }
}

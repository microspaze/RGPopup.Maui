using RGPopup.Maui.Pages;

namespace RGPopup.Maui.Events
{
    public class PopupNavigationEventArgs : EventArgs
    {
        public PopupPage Page { get; }

        public bool IsAnimated { get; }

        public PopupNavigationEventArgs(PopupPage page, bool isAnimated)
        {
            Page = page;
            IsAnimated = isAnimated;
        }
    }
}

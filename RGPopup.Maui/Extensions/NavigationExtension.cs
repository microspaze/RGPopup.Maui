using RGPopup.Maui.Pages;
using RGPopup.Maui.Services;

namespace RGPopup.Maui.Extensions
{
    public static class NavigationExtension
    {
        public static Task PushPopupAsync(this INavigation sender, PopupPage page, bool animate = true)
        {
            return PopupNavigation.Instance.PushAsync(page, animate);
        }

        public static Task PopPopupAsync(this INavigation sender, bool animate = true)
        {
            return PopupNavigation.Instance.PopAsync(animate);
        }

        public static Task PopAllPopupAsync(this INavigation sender, bool animate = true)
        {
            return PopupNavigation.Instance.PopAllAsync(animate);
        }

        public static Task RemovePopupPageAsync(this INavigation sender, PopupPage page, bool animate = true)
        {
            return PopupNavigation.Instance.RemovePageAsync(page, animate);
        }
    }
}

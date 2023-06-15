using RGPopup.Maui.Contracts;
using RGPopup.Maui.IOS.Impl;
using RGPopup.Maui.IOS.Renderers;

namespace RGPopup.Maui
{
    public static class Popup
    {
        internal static event EventHandler? OnInitialized;

        internal static bool IsInitialized { get; private set; }

        public static bool Init()
        {
            DependencyService.RegisterSingleton<IPopupPlatform>(new PopupPlatformIos());

            IsInitialized = true;
            OnInitialized?.Invoke(null, EventArgs.Empty);

            return IsInitialized;
        }
    }
}

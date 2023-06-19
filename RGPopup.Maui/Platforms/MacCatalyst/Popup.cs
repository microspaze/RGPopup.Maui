using RGPopup.Maui.Contracts;
using RGPopup.Maui.MacOS.Impl;

namespace RGPopup.Maui.MacOS
{
    public static class Popup
    {
        internal static event EventHandler? OnInitialized;

        internal static bool IsInitialized { get; private set; }

        public static bool Init()
        {
            DependencyService.RegisterSingleton<IPopupPlatform>(new PopupPlatformMacOS());

            IsInitialized = true;
            OnInitialized?.Invoke(null, EventArgs.Empty);

            return IsInitialized;
        }
    }
}

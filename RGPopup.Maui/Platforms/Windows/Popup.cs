using RGPopup.Maui.Contracts;
using RGPopup.Maui.Windows.Impl;

namespace RGPopup.Maui.Windows
{
    public static class Popup
    {
        internal static event EventHandler? OnInitialized;

        internal static bool IsInitialized { get; private set; }

        public static bool Init()
        {
            DependencyService.RegisterSingleton<IPopupPlatform>(new PopupPlatformWindows());

            IsInitialized = true;
            OnInitialized?.Invoke(null, EventArgs.Empty);

            return IsInitialized;
        }
    }
}

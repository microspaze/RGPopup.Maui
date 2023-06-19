using System.Diagnostics;
using Android.Content;
using Android.Widget;
using RGPopup.Maui.Contracts;
using RGPopup.Maui.Droid.Impl;
using RGPopup.Maui.Services;

namespace RGPopup.Maui.Droid
{
    public static class Popup
    {
        internal static event EventHandler? OnInitialized;

        internal static bool IsInitialized { get; private set; }

        internal static Context? Context { get; private set; }
        
        internal static FrameLayout? DecorView => (FrameLayout?)((Android.App.Activity?)Context)?.Window?.DecorView;

        public static bool Init(Context context)
        {
            DependencyService.RegisterSingleton<IPopupPlatform>(new PopupPlatformDroid());

            Context = context;

            IsInitialized = true;
            OnInitialized?.Invoke(null, EventArgs.Empty);

            return IsInitialized;
        }

        public static bool SendBackPressed(Action? backPressedHandler = null)
        {
            var popupNavigationInstance = PopupNavigation.Instance;

            if (popupNavigationInstance.PopupStack.Count > 0)
            {
                var lastPage = popupNavigationInstance.PopupStack.Last();

                var isPreventClose = lastPage.DisappearingTransactionTask != null || lastPage.SendBackButtonPressed();

                if (!isPreventClose)
                {
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        await popupNavigationInstance.RemovePageAsync(lastPage);
                    });
                }

                return true;
            }

            backPressedHandler?.Invoke();

            return false;
        }
    }
}
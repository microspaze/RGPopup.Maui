using Foundation;
using Microsoft.Maui.LifecycleEvents;
using RGPopup.Maui;
using UIKit;

namespace RGPopup.Samples
{
    [Register("AppDelegate")]
    public class AppDelegate : MauiUIApplicationDelegate
    {
        protected override MauiApp CreateMauiApp()
        {
            return MauiProgram.CreateMauiApp();
        }
    }
}
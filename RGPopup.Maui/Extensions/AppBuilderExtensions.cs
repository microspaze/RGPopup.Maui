using Microsoft.Maui.Controls.Compatibility.Hosting;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.LifecycleEvents;
using RGPopup.Maui.Pages;

namespace RGPopup.Maui.Extensions;

public static class AppBuilderExtensions
{
    public static MauiAppBuilder UseMauiRGPopup(this MauiAppBuilder builder, Action? backPressHandler = null)
    {
        builder
            .UseMauiCompatibility()
            .ConfigureLifecycleEvents(lifecycle =>
            {
#if ANDROID
                lifecycle.AddAndroid(b =>
                {
                    b.OnBackPressed(activity => Droid.Popup.SendBackPressed(backPressHandler));
                    b.OnCreate((activity, state) =>
                    {
                        Droid.Popup.Init(activity);
                    });
                });
#elif IOS
                lifecycle.AddiOS(b =>
                {
                    b.FinishedLaunching((application, launchOptions) => IOS.Popup.Init());
                });
#endif
            }).ConfigureMauiHandlers(handlers =>
            {
#if ANDROID
                handlers.AddHandler(typeof(PopupPage), typeof(Droid.Impl.PopupPageHandlerDroid));
#elif IOS
                handlers.AddHandler(typeof(PopupPage), typeof(PopupPageHandler));
#endif
            });
        
        return builder;
    }
}
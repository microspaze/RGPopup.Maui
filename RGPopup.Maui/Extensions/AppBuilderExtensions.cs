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
                    b.OnBackPressed(activity => Popup.SendBackPressed(backPressHandler));
                    b.OnCreate((activity, state) =>
                    {
                        Popup.Init(activity);
                    });
                });
#elif IOS
                lifecycle.AddiOS(b =>
                {
                    b.FinishedLaunching((application, launchOptions) => Popup.Init());
                });
#endif
            }).ConfigureMauiHandlers(handlers =>
            {
#if ANDROID
                handlers.TryAddCompatibilityRenderer(typeof(PopupPage), typeof(Droid.Renderers.PopupPageRenderer));
#elif IOS
                handlers.AddHandler(typeof(PopupPage), typeof(IOS.Renderers.PopupPageHandler));
#endif
            });
        
        return builder;
    }
}
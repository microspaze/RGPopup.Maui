using Microsoft.Maui.Controls.Compatibility.Hosting;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.LifecycleEvents;
using RGPopup.Maui.Effects;
using RGPopup.Maui.Pages;

namespace RGPopup.Maui.Extensions;

public static class AppBuilderExtensions
{
    public static MauiAppBuilder UseMauiRGPopup(this MauiAppBuilder builder, Action<Config>? configUpdate = null)
    {
        //Update config
        configUpdate?.Invoke(Config.Instance);

        builder
            //.UseMauiCompatibility() //This will cause ContentView in Popup not display on Windows platform.
            .ConfigureLifecycleEvents(lifecycle =>
            {
#if ANDROID
                lifecycle.AddAndroid(b =>
                {
                    b.OnBackPressed(activity => Droid.Popup.SendBackPressed(Config.Instance.BackPressHandler));
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
#elif MACCATALYST
                lifecycle.AddiOS(b =>
                {
                    b.FinishedLaunching((application, launchOptions) => MacOS.Popup.Init());
                });
#elif WINDOWS
                lifecycle.AddWindows(b =>
                {
                    b.OnLaunching((application, args) => Windows.Popup.Init());
                });
#endif
            })
            .ConfigureEffects(effects =>
            {
#if IOS
                effects.Add<KeyboardOverlapFixEffect, KeyboardOverlapFixPlatformEffect>();
#endif
            })
            .ConfigureMauiHandlers(handlers =>
            {
#if ANDROID
                handlers.AddHandler(typeof(PopupPage), typeof(Droid.Impl.PopupPageHandlerDroid));
#elif IOS
                handlers.AddHandler(typeof(PopupPage), typeof(PopupPageHandler));
#elif MACCATALYST
                handlers.AddHandler(typeof(PopupPage), typeof(PopupPageHandler));
#elif WINDOWS
                handlers.AddHandler(typeof(PopupPage), typeof(Windows.Impl.PopupPageHandlerWindows));
#endif
            });
        
        return builder;
    }
}
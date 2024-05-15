# RGPopup.Maui

Popup controls for .NET MAUI migrated from Rg.Plugins.Popup for Xamarin with some fixes.


## Setup
* Available on NuGet: [RGPopup.Maui](http://www.nuget.org/packages/RGPopup.Maui) [![NuGet](https://img.shields.io/nuget/v/RGPopup.Maui.svg?label=NuGet)](https://www.nuget.org/packages/RGPopup.Maui)
* Add nuget package to your project.
* Add ```.UseMauiRGPopup()``` to your MauiApp builder.

```csharp
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiRGPopup(config =>
            {
                config.BackPressHandler = null;
                config.FixKeyboardOverlap = true;
            })
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        return builder.Build();
    }
}
```


## Support platforms

- [x] Android      (Migrated from Rg.Plugins.Popup)
- [x] iOS          (Migrated from Rg.Plugins.Popup)
- [x] MacCatalyst  (Migrated from Rg.Plugins.Popup)
- [x] Windows      (Migrated from Mopups)


## Known Issues

1.[Windows] If you need add .UseMauiCompatibility() to your app builder, then you'd better not add ContentView to the PopupPage, otherwise the subview of ContentView will not show correctly (It may be .NET MAUI's bug).


## Documentation

You can find all descriptions of 
[Getting Started](https://github.com/rotorgames/Rg.Plugins.Popup/wiki/Getting-started), 
[How to use](https://github.com/rotorgames/Rg.Plugins.Popup/wiki/PopupPage), 
[Troubleshooting](https://github.com/rotorgames/Rg.Plugins.Popup/wiki/Troubleshooting) and etc in the 
[Wiki](https://github.com/rotorgames/Rg.Plugins.Popup/wiki)


## License
The MIT License (MIT) see [License file](LICENSE)


## Thanks
Thank JetBrains for providing DEV tools in developing. (Especially on MacOS)

![avatar](RGPopup.Samples/Resources/Images/jetbrains_logo.png)


## Release Notes

## 1.1.0

1. Fix popup bounce when keyboard hiding on iOS platform.

2. Fix Android touch event transparent popup content when CloseWhenBackgroundIsClicked=True and touch position isInRegion=True.

3. Fix Error XA3006 : Could not compile native assembly file: typemaps.x86_64.ll on Window when targeting to x86_64 android emulator.

4. Fix With 1.0.9 popup doesnt dismiss whern click outside popup wndow #14 (Thanks Reported by https://github.com/broda02)

## 1.0.9

1. Refine fix The popup with Entry can not show above of the keyboard. #9 (iOS)

2. Fix background unclickable problem for MacCatalyst.

3. Fix Inputsoft resize android not work on PopupPage #7 (Add new bindable property IsPopupWindowResizable to support PopupPage's layout resizing when SoftInput Keyboard popped-up) (Thanks Reported by https://github.com/guilhermeheibel)

4. Try to fix Popup renders behind the Android navigationbar #11 (Thanks Reported by https://github.com/jaison-t-john)

## 1.0.8

1.Fix The popup with Entry can not show above of the keyboard. #9 (iOS) (Thanks Reported by https://github.com/john-heaven)

2.Fix When taking a picture on Android, the popUp Context will be overwritten with IntermediateActivity #10 (Thanks Reported by https://github.com/jeroen-corteville)

## 1.0.7

1.Fix The specified child already has a parent. You must call removeView() on the child's parent first. #6. (Thanks Reported by https://github.com/ntbao17)

## 1.0.5

1.Fix The application eventually crashes #5. (Thanks Reported by https://github.com/andersondamasio)

## 1.0.4

1.Fix getSystemGestureInsets NoSuchMethodError when targeting to android sdk version 29. (Thanks Reported By https://github.com/Kas-code & https://github.com/sakshi-pagematics)

## 1.0.3

1.Add .NET 8 support.

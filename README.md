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
            .UseMauiRGPopup()
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

## Release Notes


## 1.0.7

1.Fix The specified child already has a parent. You must call removeView() on the child's parent first. #6. (Thanks Reported by https://github.com/ntbao17)

## 1.0.5

1.Fix The application eventually crashes #5. (Thanks Reported by https://github.com/andersondamasio)

## 1.0.4

1.Fix getSystemGestureInsets NoSuchMethodError when targeting to android sdk version 29. (Thanks Reported By https://github.com/Kas-code & https://github.com/sakshi-pagematics)

## 1.0.3

1.Add .NET 8 support.


## License
The MIT License (MIT) see [License file](LICENSE)


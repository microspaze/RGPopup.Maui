using Microsoft.Maui.Platform;
using RGPopup.Maui.Pages;
using RGPopup.Maui.Windows.Renders;

namespace RGPopup.Maui.Windows.Impl;

public class PopupPageHandlerWindows : PopupPageHandler
{
    protected override ContentPanel CreatePlatformView()
    {
        return new PopupPageRenderer(this);
    }
}

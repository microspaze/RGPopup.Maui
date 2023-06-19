using Microsoft.Maui.Platform;
using RGPopup.Maui.Droid.Renderers;
using RGPopup.Maui.Pages;

namespace RGPopup.Maui.Droid.Impl;

public class PopupPageHandlerDroid : PopupPageHandler
{
    protected override ContentViewGroup CreatePlatformView()
    {
        return new PopupPageRenderer(Context, VirtualView);
    }
}
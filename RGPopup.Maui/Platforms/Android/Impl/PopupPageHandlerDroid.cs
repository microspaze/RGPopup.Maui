using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using RGPopup.Maui.Pages;
using RGPopup.Maui.Droid.Renderers;

namespace RGPopup.Maui.Droid.Impl;

public class PopupPageHandlerDroid : PopupPageHandler
{
    protected override ContentViewGroup CreatePlatformView()
    {
        return new PopupPageRenderer(Context, VirtualView);
    }
}
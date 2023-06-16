using Microsoft.Maui;
using Microsoft.Maui.Handlers;

namespace RGPopup.Maui.Pages;

public class PopupPageHandler : PageHandler
{
    public PopupPageHandler()
    {
        var mauiContext = IPlatformApplication.Current?.Application.Windows[0].Handler?.MauiContext;
        if (mauiContext != null)
        {
            base.SetMauiContext(mauiContext);
        }
    }
}
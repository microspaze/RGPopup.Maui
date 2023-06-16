using Microsoft.Maui.Handlers;

namespace RGPopup.Maui.IOS.Renderers;

public class PopupPageHandler : PageHandler
{
    public PopupPageHandler()
    {
        var mauiContext = MauiUIApplicationDelegate.Current.Application.Windows[0].Handler?.MauiContext;
        if (mauiContext != null)
        {
            base.SetMauiContext(mauiContext);
        }
    }
}
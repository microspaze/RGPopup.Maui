using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using XFPlatform = Microsoft.Maui.Controls.Compatibility.Platform.Android.Platform;

namespace RGPopup.Maui.Droid.Extensions
{
    internal static class PlatformExtension
    {
        public static IVisualElementRenderer? GetOrCreateRenderer(this VisualElement bindable)
        {
            if (bindable == null) return null;
            IVisualElementRenderer? renderer = null;
            try
            {
                renderer = XFPlatform.GetRenderer(bindable);
                if (renderer == null && bindable.Handler == null)
                {
                    renderer = XFPlatform.CreateRendererWithContext(bindable, Popup.Context);
                    if (renderer != null)
                    {
                        XFPlatform.SetRenderer(bindable, renderer);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return renderer;
        }
    }
}
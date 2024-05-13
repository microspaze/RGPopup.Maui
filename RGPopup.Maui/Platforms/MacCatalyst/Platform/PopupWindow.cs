using CoreGraphics;
using Foundation;
using RGPopup.Maui.MacOS.Renderers;
using UIKit;

namespace RGPopup.Maui.MacOS.Platform
{
    [Preserve(AllMembers = true)]
    [Register("RgPopupWindow")]
    internal class PopupWindow : UIWindow
    {
        public PopupWindow(IntPtr handle) : base(handle)
        {
            // Fix #307
        }

        public PopupWindow()
        {

        }

        public PopupWindow(UIWindowScene uiWindowScene) : base(uiWindowScene)
        {

        }

        public override UIView HitTest(CGPoint point, UIEvent? uievent)
        {
            var hitTestResult = base.HitTest(point, uievent);
            if (uievent == null || uievent.Type != UIEventType.Touches)
            {
                return hitTestResult;
            }
            
            var platformRenderer = (PopupPageRenderer?)RootViewController;
            var pageHandler = platformRenderer?.Handler;
            var formsElement = platformRenderer?.CurrentElement;
            if (formsElement == null)
                return hitTestResult;

            if (formsElement.InputTransparent)
                return null!;

            var nativeView = pageHandler?.PlatformView;
            var contentView = formsElement.Content?.Handler?.PlatformView as UIView;
            var safePadding = formsElement.SafePadding;
            if ((formsElement.BackgroundClickedCommand != null || formsElement.BackgroundInputTransparent || formsElement.CloseWhenBackgroundIsClicked)
                && Math.Max(SafeAreaInsets.Left, safePadding.Left) < point.X && point.X < (Bounds.Width - Math.Max(SafeAreaInsets.Right, safePadding.Right))
                && Math.Max(SafeAreaInsets.Top, safePadding.Top) < point.Y && point.Y < (Bounds.Height - Math.Max(SafeAreaInsets.Bottom, safePadding.Bottom))
                && (hitTestResult.Equals(nativeView) || hitTestResult.Equals(contentView)))
            {
                _ = formsElement.SendBackgroundClick();
                if (formsElement.BackgroundInputTransparent)
                {
                    return null!;
                }
            }

            return hitTestResult;
        }
    }
}

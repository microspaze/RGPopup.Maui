using CoreGraphics;
using Foundation;
using RGPopup.Maui.IOS.Renderers;
using RGPopup.Maui.Pages;
using UIKit;

namespace RGPopup.Maui.IOS.Platform
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
            var platformRenderer = (PopupPageRenderer?)RootViewController;
            var pageHandler = platformRenderer?.Handler;
            var hitTestResult = base.HitTest(point, uievent);

            var formsElement = platformRenderer?.CurrentElement;
            if (formsElement == null)
                return hitTestResult;

            if (formsElement.InputTransparent)
                return null!;

            if ((formsElement.BackgroundInputTransparent || formsElement.CloseWhenBackgroundIsClicked)
                && hitTestResult.Equals(pageHandler?.PlatformView))
            {
                _ = formsElement.SendBackgroundClick();
                return null!;
            }

            return hitTestResult;
        }
    }
}

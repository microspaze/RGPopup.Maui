using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Compatibility.Platform.iOS;
using RGPopup.Maui.IOS.Renderers;
using RGPopup.Maui.Pages;
using UIKit;

using XFPlatform = Microsoft.Maui.Controls.Compatibility.Platform.iOS.Platform;

namespace RGPopup.Maui.IOS.Extensions
{
    internal static class PlatformExtension
    {
        private static bool IsiOS13OrNewer => UIDevice.CurrentDevice.CheckSystemVersion(13, 0);

        public static IVisualElementRenderer GetOrCreateRenderer(this VisualElement bindable)
        {
            var renderer = XFPlatform.GetRenderer(bindable);
            if (renderer == null)
            {
                if (bindable.Handler != null)
                {
                    bindable.Handler.DisconnectHandler();
                }
                
                renderer = XFPlatform.CreateRenderer(bindable);
                XFPlatform.SetRenderer(bindable, renderer);
            }
            return renderer;
        }

        public static T GetOrCreateHandler<T>(this VisualElement bindable) where T : IViewHandler, new()
        {
            return (T)(bindable.Handler ??= new T());
        }
        
        public static void DisposeModelAndChildrenHandlers(this VisualElement? view)
        {
            var handler = view?.Handler;
            if (handler != null && handler.PlatformView is UIView nativeView)
            {
                handler.DisconnectHandler();
                nativeView.RemoveFromSuperview();
                nativeView.Dispose();
            }
        }

        public static void UpdateSize(this PopupPageRenderer renderer)
        {
            var currentElement = renderer.CurrentElement;
            if (renderer.View?.Superview?.Frame == null || currentElement == null)
                return;

            var superviewFrame = renderer.View.Superview.Frame;
            var applicationFrame = UIScreen.MainScreen.ApplicationFrame;
            var keyboardOffset = renderer.KeyboardBounds.Height;

            Thickness systemPadding;

            if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0)
                && UIApplication.SharedApplication.KeyWindow != null)
            {
                var safeAreaInsets = UIApplication.SharedApplication.KeyWindow.SafeAreaInsets;

                systemPadding = new Thickness(
                    safeAreaInsets.Left,
                    safeAreaInsets.Top,
                    safeAreaInsets.Right,
                    safeAreaInsets.Bottom);
            }
            else
            {
                systemPadding = new Thickness
                {
                    Left = applicationFrame.Left,
                    Top = applicationFrame.Top,
                    Right = applicationFrame.Right - applicationFrame.Width - applicationFrame.Left,
                    Bottom = applicationFrame.Bottom - applicationFrame.Height - applicationFrame.Top
                };
            }

            var needForceLayout =
                (currentElement.HasSystemPadding && currentElement.SystemPadding != systemPadding)
                || (currentElement.HasKeyboardOffset && currentElement.KeyboardOffset != keyboardOffset);

            currentElement.SetValueFromRenderer(PopupPage.SystemPaddingProperty, systemPadding);
            currentElement.SetValueFromRenderer(PopupPage.KeyboardOffsetProperty, keyboardOffset);

            var elementSize = new Size(superviewFrame.Width, superviewFrame.Height);
            var elementBounds = currentElement.Bounds;
            if (elementBounds.Size != elementSize)
                currentElement.LayoutTo(new Rect(elementBounds.X, elementBounds.Y, elementSize.Width,
                    elementSize.Height));
            else if (needForceLayout)
                currentElement.ForceLayout();
        }

        public static UIWindow GetKeyWindow(this UIApplication application)
        {
            if (!IsiOS13OrNewer)
                return UIApplication.SharedApplication.KeyWindow;

            var window = application
                .ConnectedScenes
                .ToArray()
                .OfType<UIWindowScene>()
                .SelectMany(scene => scene.Windows)
                .FirstOrDefault(window => window.IsKeyWindow);

            return window;
        }
    }
}

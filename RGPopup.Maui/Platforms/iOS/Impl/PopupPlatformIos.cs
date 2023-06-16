using Foundation;
using RGPopup.Maui.Contracts;
using RGPopup.Maui.Exceptions;
using RGPopup.Maui.Extensions;
using RGPopup.Maui.IOS.Extensions;
using RGPopup.Maui.IOS.Platform;
using RGPopup.Maui.IOS.Renderers;
using RGPopup.Maui.Pages;

using UIKit;

namespace RGPopup.Maui.IOS.Impl
{
    [Preserve(AllMembers = true)]
    internal class PopupPlatformIos : IPopupPlatform
    {
        // It's necessary because GC in Xamarin.iOS 13 removes all UIWindow if there are not any references to them. See #459
        private readonly List<UIWindow> _windows = new List<UIWindow>();

        private static bool IsiOS9OrNewer => UIDevice.CurrentDevice.CheckSystemVersion(9, 0);

        private static bool IsiOS13OrNewer => UIDevice.CurrentDevice.CheckSystemVersion(13, 0);

        public event EventHandler OnInitialized
        {
            add => Popup.OnInitialized += value;
            remove => Popup.OnInitialized -= value;
        }

        public bool IsInitialized => Popup.IsInitialized;

        public bool IsSystemAnimationEnabled => true;

        public Task AddAsync(PopupPage page)
        {
            page.Parent = Application.Current?.MainPage;

            page.DescendantRemoved += HandleChildRemoved;

            var keyWindow = UIApplication.SharedApplication.GetKeyWindow();
            if (keyWindow?.WindowLevel == UIWindowLevel.Normal)
                keyWindow.WindowLevel = -1;
            
            var pageHandler = page.GetOrCreateHandler<PopupPageHandler>();

            PopupWindow window;
            if (IsiOS13OrNewer)
            {
                if (UIApplication.SharedApplication.ConnectedScenes.ToArray()
                    .FirstOrDefault(x => x.ActivationState == UISceneActivationState.ForegroundActive && x is UIWindowScene) is UIWindowScene connectedScene)
                    window = new PopupWindow(connectedScene);
                else
                    window = new PopupWindow();

                _windows.Add(window);
            }
            else
                window = new PopupWindow();

            window.BackgroundColor = UIColor.Clear;
            window.RootViewController = new PopupPageRenderer(page);
            if (window.RootViewController.View != null)
                window.RootViewController.View.BackgroundColor = UIColor.Clear;
            window.WindowLevel = UIWindowLevel.Normal;
            window.MakeKeyAndVisible();
            
            pageHandler.ViewController.ModalPresentationStyle = UIModalPresentationStyle.OverCurrentContext;
            pageHandler.ViewController.ModalTransitionStyle = UIModalTransitionStyle.CoverVertical;
            
            return window.RootViewController.PresentViewControllerAsync(pageHandler.ViewController!, false);
        }

        public async Task RemoveAsync(PopupPage page)
        {
            if (page == null)
                throw new RGPageInvalidException("Popup page is null");
            
            var pageHandler = page.GetHandler<PopupPageHandler>();
            var viewController = pageHandler?.ViewController;

            await Task.Delay(50);

            page.DescendantRemoved -= HandleChildRemoved;

            if (pageHandler != null && viewController != null && !viewController.IsBeingDismissed)
            {
                var window = viewController.View?.Window;
                page.Parent = null;
                if (window != null)
                {
                    var rvc = window.RootViewController;
                    if (rvc != null)
                    {
                        await rvc.DismissViewControllerAsync(false);
                        page.DisposeModelAndChildrenHandlers();
                        rvc.Dispose();
                    }
                    window.RootViewController = null;
                    window.Hidden = true;
                    if (IsiOS13OrNewer && _windows.Contains(window))
                        _windows.Remove(window);
                    window.Dispose();
                    window = null;
                }

                var keyWindow = UIApplication.SharedApplication.GetKeyWindow();
                if (_windows.Count > 0)
                    _windows.Last().WindowLevel = UIWindowLevel.Normal;
                else if (keyWindow?.WindowLevel == -1)
                    keyWindow.WindowLevel = UIWindowLevel.Normal;
            }
        }

        private void HandleChildRemoved(object? sender, ElementEventArgs e)
        {
            if (e.Element is VisualElement view)
            {
                view.DisposeModelAndChildrenHandlers();
            }
        }
    }
}

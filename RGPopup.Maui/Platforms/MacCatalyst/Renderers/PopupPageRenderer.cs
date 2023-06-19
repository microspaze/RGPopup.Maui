
using System.Diagnostics;
using CoreGraphics;

using Foundation;
using RGPopup.Maui.MacOS.Extensions;
using RGPopup.Maui.Pages;

using UIKit;

namespace RGPopup.Maui.MacOS.Renderers
{
    public class PopupPageRenderer : UIViewController
    {
        private readonly UIGestureRecognizer _tapGestureRecognizer;
        private bool _isDisposed;
        
        public PopupPage? CurrentElement { get; }
        public PopupPageHandler? Handler { get; }

        internal CGRect KeyboardBounds { get; private set; } = CGRect.Empty;
        
        #region Main Methods

        public PopupPageRenderer(PopupPage popupPage)
        {
            CurrentElement = popupPage;
            Handler = popupPage?.Handler as PopupPageHandler;
            
            _tapGestureRecognizer = new UITapGestureRecognizer(OnTap)
            {
                CancelsTouchesInView = false
            };
        }
        
        public PopupPageRenderer(IntPtr handle) : base(handle)
        {
            // Fix #307
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                View?.RemoveGestureRecognizer(_tapGestureRecognizer);
            }

            base.Dispose(disposing);

            _isDisposed = true;
        }

        #endregion

        #region Gestures Methods

        private void OnTap(UITapGestureRecognizer e)
        {
            if (CurrentElement == null) return;
            
            var view = e.View;
            var location = e.LocationInView(view);
            var subview = view.HitTest(location, null);
            if (Equals(subview, view))
            {
                _ = CurrentElement.SendBackgroundClick();
            }
        }

        #endregion

        #region Life Cycle Methods

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            ModalPresentationStyle = UIModalPresentationStyle.OverCurrentContext;
            ModalTransitionStyle = UIModalTransitionStyle.CoverVertical;

            View?.AddGestureRecognizer(_tapGestureRecognizer);
        }

        public override void ViewDidUnload()
        {
            base.ViewDidUnload();

            View?.RemoveGestureRecognizer(_tapGestureRecognizer);
        }

        #endregion

        #region Layout Methods

        public override void ViewDidLayoutSubviews()
        {
            if (_isDisposed)
                return;

            base.ViewDidLayoutSubviews();
            this.UpdateSize();
            PresentedViewController?.ViewDidLayoutSubviews();
        }

        #endregion

        #region Override Methods
        
         public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations()
        {
            if ((ChildViewControllers != null) && (ChildViewControllers.Length > 0))
            {
                return ChildViewControllers[0].GetSupportedInterfaceOrientations();
            }
            return base.GetSupportedInterfaceOrientations();
        }

        public override UIInterfaceOrientation PreferredInterfaceOrientationForPresentation()
        {
            if ((ChildViewControllers != null) && (ChildViewControllers.Length > 0))
            {
                return ChildViewControllers[0].PreferredInterfaceOrientationForPresentation();
            }
            return base.PreferredInterfaceOrientationForPresentation();
        }

        public override UIViewController ChildViewControllerForStatusBarHidden()
        {
            return Handler?.ViewController!;
        }

        public override bool PrefersStatusBarHidden()
        {
            return Handler?.ViewController?.PrefersStatusBarHidden() ?? false;
        }

        public override UIViewController ChildViewControllerForStatusBarStyle()
        {
            return Handler?.ViewController!;
        }

        public override UIStatusBarStyle PreferredStatusBarStyle()
        {
            return (UIStatusBarStyle)(Handler?.ViewController?.PreferredStatusBarStyle())!;
        }

        public override bool ShouldAutorotate()
        {
            if ((ChildViewControllers != null) && (ChildViewControllers.Length > 0))
            {
                return ChildViewControllers[0].ShouldAutorotate();
            }
            return base.ShouldAutorotate();
        }

        public override bool ShouldAutorotateToInterfaceOrientation(UIInterfaceOrientation toInterfaceOrientation)
        {
            if ((ChildViewControllers != null) && (ChildViewControllers.Length > 0))
            {
                return ChildViewControllers[0].ShouldAutorotateToInterfaceOrientation(toInterfaceOrientation);
            }
            return base.ShouldAutorotateToInterfaceOrientation(toInterfaceOrientation);
        }

        public override bool ShouldAutomaticallyForwardRotationMethods => true;

        #endregion
    }
}

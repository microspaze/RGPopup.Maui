
using Android.App;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.Maui.Platform;
using RGPopup.Maui.Contracts;
using RGPopup.Maui.Droid.Extensions;
using RGPopup.Maui.Droid.Impl;
using RGPopup.Maui.Droid.Renderers;
using RGPopup.Maui.Exceptions;
using RGPopup.Maui.Extensions;
using RGPopup.Maui.Pages;
using View = Android.Views.View;
using XApplication = Microsoft.Maui.Controls.Application;

namespace RGPopup.Maui.Droid.Impl
{
    internal class PopupPlatformDroid : IPopupPlatform
    {
        private static FrameLayout? DecorView => Popup.DecorView;
        
        public event EventHandler OnInitialized
        {
            add => Popup.OnInitialized += value;
            remove => Popup.OnInitialized -= value;
        }

        public bool IsInitialized => Popup.IsInitialized;

        public bool IsSystemAnimationEnabled => GetIsSystemAnimationEnabled();

        public Task AddAsync(PopupPage page)
        {
            HandleAccessibilityWorkaround(page, ImportantForAccessibility.NoHideDescendants);
            
            page.Parent = XApplication.Current?.MainPage;
            var pageHandler = page.GetOrCreateHandler<PopupPageHandlerDroid>();
            DecorView?.AddView(pageHandler.PlatformView);
            return PostAsync(pageHandler.PlatformView);
        }

        public Task RemoveAsync(PopupPage page)
        {
            if (page == null)
                throw new RGPageInvalidException("Popup page is null");

            var pageHandler = page.GetHandler<PopupPageHandlerDroid>();
            if (pageHandler != null)
            {
                HandleAccessibilityWorkaround(page, ImportantForAccessibility.Auto);

                DecorView?.RemoveView(pageHandler.PlatformView);
                page.Parent = null;
                //If manual dispose the view's renderer, but the view is not disposed at the same time, it will crash when repush the view.
                //renderer.Dispose();
                
                if (DecorView != null)
                    return PostAsync(DecorView);
            }

            return Task.FromResult(true);
        }

        #region System Animation

        private static bool GetIsSystemAnimationEnabled()
        {
            float animationScale;
            var context = Popup.Context;

            if (context == null)
                return false;

            if (Build.VERSION.SdkInt >= BuildVersionCodes.JellyBeanMr1)
            {
                animationScale = Settings.Global.GetFloat(
                    context.ContentResolver,
                    Settings.Global.AnimatorDurationScale,
                    1);
            }
            else
            {
                animationScale = Settings.System.GetFloat(
                    context.ContentResolver,
                    Settings.System.AnimatorDurationScale,
                    1);
            }

            return animationScale > 0;
        }

        #endregion

        #region Helpers

        private static Task PostAsync(View? nativeView)
        {
            if (nativeView == null)
                return Task.FromResult(true);

            var tcs = new TaskCompletionSource<bool>();

            nativeView.Post(() => tcs.SetResult(true));

            return tcs.Task;
        }

        private static void HandleAccessibilityWorkaround(PopupPage page, ImportantForAccessibility accessibility)
        {
            if (page.AndroidTalkbackAccessibilityWorkaround)
            {
                var mainPage = XApplication.Current?.MainPage;
                if (mainPage == null) return;

                var pageHandler = page.GetHandler<PopupPageHandler>();
                if (pageHandler != null)
                {
                    pageHandler.PlatformView.ImportantForAccessibility = accessibility;
                }

                var navCount = mainPage.Navigation.NavigationStack.Count;
                if (navCount > 0)
                {
                    var navPage = mainPage.Navigation.NavigationStack[navCount - 1];
                    if (navPage != null && navPage.Handler?.PlatformView is View navPageView)
                    {
                        navPageView.ImportantForAccessibility = accessibility;
                    }
                }

                var modalCount = mainPage.Navigation.ModalStack.Count;
                if (modalCount > 0)
                {
                    var modalPage = mainPage.Navigation.ModalStack[modalCount - 1];
                    if (modalPage != null && modalPage.Handler?.PlatformView is View modelPageView)
                    {
                        modelPageView.ImportantForAccessibility = accessibility;
                    }
                }

                if (accessibility == ImportantForAccessibility.NoHideDescendants)
                {
                    DisableFocusableInTouchMode(pageHandler?.PlatformView.Parent);
                }
            }
        }

        private static void DisableFocusableInTouchMode(IViewParent? parent)
        {
            var view = parent;
            string className = $"{view?.GetType().Name}";

            while (!className.Contains("PlatformRenderer") && view != null)
            {
                view = view.Parent;
                className = $"{view?.GetType().Name}";
            }

            if (view is Android.Views.View androidView)
            {
                androidView.Focusable = false;
                androidView.FocusableInTouchMode = false;
            }
        }

        #endregion
    }
}

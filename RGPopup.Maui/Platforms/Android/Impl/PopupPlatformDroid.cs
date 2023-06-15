
using System.Diagnostics.CodeAnalysis;
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
using RGPopup.Maui.Exceptions;
using RGPopup.Maui.Pages;

using XApplication = Microsoft.Maui.Controls.Application;

namespace RGPopup.Maui.Droid.Impl
{
    internal class PopupPlatformDroid : IPopupPlatform
    {
        private static FrameLayout? DecoreView => (FrameLayout?)((Activity?)Popup.Context)?.Window?.DecorView;
        
        public event EventHandler OnInitialized
        {
            add => Popup.OnInitialized += value;
            remove => Popup.OnInitialized -= value;
        }

        public bool IsInitialized => Popup.IsInitialized;

        public bool IsSystemAnimationEnabled => GetIsSystemAnimationEnabled();

        public Task AddAsync(PopupPage page)
        {
            var decoreView = DecoreView;

            HandleAccessibilityWorkaround(page);

            page.Parent = XApplication.Current?.MainPage;
            var renderer = page.GetOrCreateRenderer();

            decoreView?.AddView(renderer.View);
            return PostAsync(renderer.View);

            static void HandleAccessibilityWorkaround(PopupPage page)
            {
                if (page.AndroidTalkbackAccessibilityWorkaround)
                {
                    var mainPage = XApplication.Current?.MainPage;
                    if (mainPage == null) return;
                    var navCount = mainPage.Navigation.NavigationStack.Count;
                    var modalCount = mainPage.Navigation.ModalStack.Count;
                    var renderer = mainPage.GetOrCreateRenderer();
                    if (renderer == null) return;
                    
                    renderer.View.ImportantForAccessibility = ImportantForAccessibility.NoHideDescendants;

                    if (navCount > 0)
                    {
                        var navPage = mainPage.Navigation.NavigationStack[navCount - 1];
                        if (navPage != null)
                        {
                            navPage.GetOrCreateRenderer().View.ImportantForAccessibility = ImportantForAccessibility.NoHideDescendants;
                        }
                    }
                    if (modalCount > 0)
                    {
                        var modalPage = mainPage.Navigation.ModalStack[modalCount - 1];
                        if (modalPage != null)
                        {
                            modalPage.GetOrCreateRenderer().View.ImportantForAccessibility = ImportantForAccessibility.NoHideDescendants;
                        }
                    }

                    DisableFocusableInTouchMode(renderer.View.Parent);
                }
            }

            static void DisableFocusableInTouchMode(IViewParent? parent)
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
        }

        public Task RemoveAsync(PopupPage page)
        {
            if (page == null)
                throw new RGPageInvalidException("Popup page is null");

            var renderer = page.GetOrCreateRenderer();
            if (renderer != null)
            {
                //HandleAccessibilityWorkaround(page);

                page.Parent = XApplication.Current?.MainPage;
                var element = renderer.Element;

                DecoreView?.RemoveView(renderer.View);
                //If manual dispose the view's renderer, but the view is not disposed at the same time, it will crash when repush the view.
                //renderer.Dispose();

                if (element != null)
                    element.Parent = null;
                if (DecoreView != null)
                    return PostAsync(DecoreView);
            }

            return Task.FromResult(true);

            static void HandleAccessibilityWorkaround(PopupPage page)
            {
                if (page.AndroidTalkbackAccessibilityWorkaround)
                {
                    var mainPage = XApplication.Current?.MainPage;
                    if (mainPage == null) return;
                    var navCount = mainPage.Navigation.NavigationStack.Count;
                    var modalCount = mainPage.Navigation.ModalStack.Count;
                    var mainPageRenderer = mainPage.GetOrCreateRenderer();

                    // Workaround for https://github.com/rotorgames/Rg.Plugins.Popup/issues/721
                    if (!(mainPage is MultiPage<Page>))
                    {
                        mainPageRenderer.View.ImportantForAccessibility = ImportantForAccessibility.Auto;
                    }

                    if (navCount > 0)
                    {
                        var navPage = mainPage.Navigation.NavigationStack[navCount - 1];
                        if (navPage != null)
                        {
                            navPage.GetOrCreateRenderer().View.ImportantForAccessibility = ImportantForAccessibility.Auto;
                        }
                    }
                    if (modalCount > 0)
                    {
                        var modalPage = mainPage.Navigation.ModalStack[modalCount - 1];
                        if (modalPage != null)
                        {
                            modalPage.GetOrCreateRenderer().View.ImportantForAccessibility = ImportantForAccessibility.Auto;
                        }
                    }
                }
            }
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

        private static Task PostAsync(Android.Views.View nativeView)
        {
            if (nativeView == null)
                return Task.FromResult(true);

            var tcs = new TaskCompletionSource<bool>();

            nativeView.Post(() => tcs.SetResult(true));

            return tcs.Task;
        }
        #endregion
    }
}

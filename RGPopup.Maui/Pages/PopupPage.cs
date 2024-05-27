using System.Windows.Input;

using RGPopup.Maui.Animations;
using RGPopup.Maui.Enums;
using RGPopup.Maui.Interfaces.Animations;
using RGPopup.Maui.Services;

namespace RGPopup.Maui.Pages
{
    [ContentProperty("Content")]
    public class PopupPage : ContentPage
    {
        #region Internal Properties

        private static readonly bool _isIOS = DeviceInfo.Current.Platform == DevicePlatform.iOS;
        
        internal Task? AppearingTransactionTask { get; set; }

        internal Task? DisappearingTransactionTask { get; set; }

        #endregion

        #region Events

        public event EventHandler? BackgroundClicked;

        #endregion

        #region Bindable Properties
        
        public static readonly BindableProperty IsPopupWindowResizableProperty = BindableProperty.Create(nameof(IsPopupWindowResizable), typeof(bool), typeof(PopupPage), false);

        public bool IsPopupWindowResizable
        {
            get => (bool)GetValue(IsPopupWindowResizableProperty);
            set => SetValue(IsPopupWindowResizableProperty, value);
        }
        
        public static readonly BindableProperty IsAnimationEnabledProperty = BindableProperty.Create(nameof(IsAnimationEnabled), typeof(bool), typeof(PopupPage), true);

        public bool IsAnimationEnabled
        {
            get => (bool)GetValue(IsAnimationEnabledProperty);
            set => SetValue(IsAnimationEnabledProperty, value);
        }

        public static readonly BindableProperty HasSystemPaddingProperty = BindableProperty.Create(nameof(HasSystemPadding), typeof(bool), typeof(PopupPage), true);

        public bool HasSystemPadding
        {
            get => (bool)GetValue(HasSystemPaddingProperty);
            set => SetValue(HasSystemPaddingProperty, value);
        }

        public static readonly BindableProperty AnimationProperty = BindableProperty.Create(nameof(Animation), typeof(IPopupAnimation), typeof(PopupPage), new ScaleAnimation());

        public IPopupAnimation? Animation
        {
            get => (IPopupAnimation?)GetValue(AnimationProperty);
            set => SetValue(AnimationProperty, value);
        }
        
        public static readonly BindableProperty SafePaddingProperty = BindableProperty.Create(nameof(SafePadding), typeof(Thickness), typeof(PopupPage), new Thickness(10), BindingMode.OneWayToSource);

        public Thickness SafePadding
        {
            get => (Thickness)GetValue(SafePaddingProperty);
            internal set => SetValue(SafePaddingProperty, value);
        }

        public static readonly BindableProperty SystemPaddingProperty = BindableProperty.Create(nameof(SystemPadding), typeof(Thickness), typeof(PopupPage), default(Thickness), BindingMode.OneWayToSource);

        public Thickness SystemPadding
        {
            get => (Thickness)GetValue(SystemPaddingProperty);
            internal set => SetValue(SystemPaddingProperty, value);
        }

        public static readonly BindableProperty SystemPaddingSidesProperty = BindableProperty.Create(nameof(SystemPaddingSides), typeof(PaddingSide), typeof(PopupPage), PaddingSide.All);

        public PaddingSide SystemPaddingSides
        {
            get => (PaddingSide)GetValue(SystemPaddingSidesProperty);
            set => SetValue(SystemPaddingSidesProperty, value);
        }

        public static readonly BindableProperty CloseWhenBackgroundIsClickedProperty = BindableProperty.Create(nameof(CloseWhenBackgroundIsClicked), typeof(bool), typeof(PopupPage), true);

        public bool CloseWhenBackgroundIsClicked
        {
            get => (bool)GetValue(CloseWhenBackgroundIsClickedProperty);
            set => SetValue(CloseWhenBackgroundIsClickedProperty, value);
        }

        public static readonly BindableProperty BackgroundInputTransparentProperty = BindableProperty.Create(nameof(BackgroundInputTransparent), typeof(bool), typeof(PopupPage), false);

        public bool BackgroundInputTransparent
        {
            get => (bool)GetValue(BackgroundInputTransparentProperty);
            set => SetValue(BackgroundInputTransparentProperty, value);
        }

        public static readonly BindableProperty HasKeyboardOffsetProperty = BindableProperty.Create(nameof(HasKeyboardOffset), typeof(bool), typeof(PopupPage), true);

        public bool HasKeyboardOffset
        {
            get => (bool)GetValue(HasKeyboardOffsetProperty);
            set => SetValue(HasKeyboardOffsetProperty, value);
        }

        public static readonly BindableProperty KeyboardOffsetProperty = BindableProperty.Create(nameof(KeyboardOffset), typeof(double), typeof(PopupPage), 0d, BindingMode.OneWayToSource);

        public double KeyboardOffset
        {
            get => (double)GetValue(KeyboardOffsetProperty);
            private set => SetValue(KeyboardOffsetProperty, value);
        }

        public static readonly BindableProperty BackgroundClickedCommandProperty = BindableProperty.Create(nameof(BackgroundClickedCommand), typeof(ICommand), typeof(PopupPage));

        public ICommand? BackgroundClickedCommand
        {
            get => (ICommand?)GetValue(BackgroundClickedCommandProperty);
            set => SetValue(BackgroundClickedCommandProperty, value);
        }

        public static readonly BindableProperty BackgroundClickedCommandParameterProperty = BindableProperty.Create(nameof(BackgroundClickedCommandParameter), typeof(object), typeof(PopupPage));

        public object BackgroundClickedCommandParameter
        {
            get => GetValue(BackgroundClickedCommandParameterProperty);
            set => SetValue(BackgroundClickedCommandParameterProperty, value);
        }

        public static readonly BindableProperty AndroidTalkbackAccessibilityWorkaroundProperty = BindableProperty.Create(nameof(AndroidTalkbackAccessibilityWorkaround), typeof(bool), typeof(PopupPage), false);

        public bool AndroidTalkbackAccessibilityWorkaround
        {
            get => (bool)GetValue(AndroidTalkbackAccessibilityWorkaroundProperty);
            set => SetValue(AndroidTalkbackAccessibilityWorkaroundProperty, value);
        }

        public new static readonly BindableProperty ContentProperty = BindableProperty.Create(nameof(Content), typeof(View), typeof(ContentPage), null, propertyChanged: OnContentChanged);
        
        public new View Content
        {
            get => (View)GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        public static readonly BindableProperty DisableScrollViewProperty = BindableProperty.Create(nameof(DisableScrollView), typeof(bool), typeof(PopupPage), null);

        public bool DisableScrollView
        {
            get => (bool)GetValue(DisableScrollViewProperty);
            set => SetValue(DisableScrollViewProperty, value);
        }

        public View CoreContent { get; private set; }
        
        public View WrappedContent { get; private set; }
        
        public static void OnContentChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is not PopupPage popupPage || bindable is not ContentPage contentPage || newValue == contentPage.Content) return;
            var coreContent = newValue as View;
            if (coreContent is not ContentView)
            {
                coreContent = new ContentView() { Content = coreContent, InputTransparent = false };
            }
            var wrappedContent = _isIOS && !popupPage.DisableScrollView ? new ScrollView(){ Content = coreContent, InputTransparent = false } : coreContent;
            contentPage.Content = wrappedContent;
            popupPage.CoreContent = coreContent;
            popupPage.WrappedContent = wrappedContent;
        }
        
        #endregion

        #region Main Methods

        public PopupPage()
        {
            BackgroundColor = Color.FromArgb("#80000000");
            InputTransparent = false;
        }
        
        protected override void OnParentSet()
        {
            if (this.Parent != null)
            {
                this.SendAppearing();
            }
            else
            {
                this.SendDisappearing();
            }

            base.OnParentSet();
        }

        protected override void OnPropertyChanged(string? propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(HasSystemPadding):
                case nameof(HasKeyboardOffset):
                case nameof(SystemPaddingSides):
                    ForceLayout();
                    break;
            }
        }

        protected override bool OnBackButtonPressed()
        {
            return false;
        }

        #endregion

        #region Size Methods

        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            base.LayoutChildren(x, y, width, height);
            return;
            //UNCOMMENT THIS TO FIX POPUP BOUNCE WHEN KEYBOARD HIDING
            if (HasSystemPadding)
            {
                var systemPadding = SystemPadding;
                var systemPaddingSide = SystemPaddingSides;
                var left = 0d;
                var top = 0d;
                var right = 0d;
                var bottom = 0d;

                if (systemPaddingSide.HasFlag(PaddingSide.Left))
                    left = systemPadding.Left;
                if (systemPaddingSide.HasFlag(PaddingSide.Top))
                    top = systemPadding.Top;
                if (systemPaddingSide.HasFlag(PaddingSide.Right))
                    right = systemPadding.Right;
                if (systemPaddingSide.HasFlag(PaddingSide.Bottom))
                    bottom = systemPadding.Bottom;

                x += left;
                y += top;
                width -= left + right;

                if (HasKeyboardOffset)
                    height -= top + Math.Max(bottom, KeyboardOffset);
                else
                    height -= top + bottom;
            }
            else if (HasKeyboardOffset)
            {
                height -= KeyboardOffset;
            }
            
            base.LayoutChildren(x, y, width, height);
        }

        #endregion

        #region Animation Methods

        internal void PreparingAnimation()
        {
            if (IsAnimationEnabled)
                Animation?.Preparing(Content, this);
        }

        internal void DisposingAnimation()
        {
            if (IsAnimationEnabled)
                Animation?.Disposing(Content, this);
        }

        internal async Task AppearingAnimation()
        {
            await OnAppearingAnimationBeginAsync();

            if (IsAnimationEnabled && Animation != null)
                await Animation.Appearing(Content, this);
            
            await OnAppearingAnimationEndAsync();
        }

        internal async Task DisappearingAnimation()
        {
            await OnDisappearingAnimationBeginAsync();

            if (IsAnimationEnabled && Animation != null)
                await Animation.Disappearing(Content, this);
            
            await OnDisappearingAnimationEndAsync();
        }

        #endregion

        #region Override Animation Methods

        protected virtual void OnAppearingAnimationBegin()
        {
        }

        protected virtual void OnAppearingAnimationEnd()
        {
        }

        protected virtual void OnDisappearingAnimationBegin()
        {
        }

        protected virtual void OnDisappearingAnimationEnd()
        {
        }

        protected virtual Task OnAppearingAnimationBeginAsync()
        {
            OnAppearingAnimationBegin();
            return Task.FromResult(0);
        }

        protected virtual Task OnAppearingAnimationEndAsync()
        {
            OnAppearingAnimationEnd();
            return Task.FromResult(0);
        }

        protected virtual Task OnDisappearingAnimationBeginAsync()
        {
            OnDisappearingAnimationBegin();
            return Task.FromResult(0);
        }

        protected virtual Task OnDisappearingAnimationEndAsync()
        {
            OnDisappearingAnimationEnd();
            return Task.FromResult(0);
        }

        #endregion

        #region Background Click

        protected virtual bool OnBackgroundClicked()
        {
            return CloseWhenBackgroundIsClicked;
        }

        #endregion

        #region Internal Methods

        internal async Task SendBackgroundClick()
        {
            BackgroundClicked?.Invoke(this, EventArgs.Empty);
            if (BackgroundClickedCommand?.CanExecute(BackgroundClickedCommandParameter) == true)
            {
                BackgroundClickedCommand.Execute(BackgroundClickedCommandParameter);
            }
            var isClose = OnBackgroundClicked();
            if (isClose)
            {
                await PopupNavigation.Instance.RemovePageAsync(this);
            }
        }

        #endregion
    }
}

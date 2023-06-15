using RGPopup.Maui.Pages;

namespace RGPopup.Maui.Contracts
{
    public interface IPopupPlatform
    {
        event EventHandler OnInitialized;

        bool IsInitialized { get; }

        bool IsSystemAnimationEnabled { get; }

        Task AddAsync(PopupPage page);

        Task RemoveAsync(PopupPage page);
    }
}

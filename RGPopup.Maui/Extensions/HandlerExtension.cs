using Microsoft.Maui.Handlers;

namespace RGPopup.Maui.Extensions;

public static class HandlerExtension
{
    public static T? GetHandler<T>(this VisualElement bindable) where T : IViewHandler, new()
    {
        return (T?)bindable.Handler;
    }
    
    public static T GetOrCreateHandler<T>(this VisualElement bindable) where T : IViewHandler, new()
    {
        return (T)(bindable.Handler ??= new T());
    }
}
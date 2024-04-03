using RGPopup.Samples.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGPopup.Samples.Helpers
{
    public class PopupHelper
    {
        private static BasePopupPage? _popup = null;

        public static async Task ShowPopup(ContentView content, string title = "", bool staticMode = false)
        {
            if (staticMode)
            {
                _popup ??= new BasePopupPage(content, title);
                _popup.UpdateContent(content, title);
                await _popup.ShowPopup();
            }
            else
            {
                var popup = new BasePopupPage(content, title);
                await popup.ShowPopup();
            }
        }
    }
}

using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Xam.Zero.Popups;
using Xam.Zero.Services;
using Xam.Zero.ViewModels;

namespace Xam.Zero.RGPopup
{
    public class RGPopupNavigator : IPopupNavigator
    {
        private readonly ObjectIDGenerator _objectIDGenerator;
        private readonly Dictionary<long, object> _popupMapper;

        private RGPopupNavigator()
        {
            this._objectIDGenerator = new ObjectIDGenerator();
            this._popupMapper = new Dictionary<long,object>();
        }

        public static RGPopupNavigator Build()
        {
            return new RGPopupNavigator();
        }

        public Task ShowPopup(IXamZeroPopup popup)
        {
            var popupPage = (PopupPage)popup;
            return PopupNavigation.Instance.PushAsync(popupPage);
        }

        public async Task<T> ShowPopup<T>(IXamZeroPopup<T> popup)
        {
            var popupPage = (PopupPage)popup;
            var completionSource = new TaskCompletionSource<T>();
            var popupId = this._objectIDGenerator.GetId(popupPage, out var _);
            if (this._popupMapper.ContainsKey(popupId)) 
                this._popupMapper.Remove(popupId);
            this._popupMapper.Add(popupId, completionSource);
            await PopupNavigation.Instance.PushAsync(popupPage);
            var result = await completionSource.Task;
            return result;
        }

        public Task DismissPopup(IXamZeroPopup popup)
        {
            var popupPage = (PopupPage)popup;
            return PopupNavigation.Instance.RemovePageAsync(popupPage);
        }

        public Task DismissPopup<T>(IXamZeroPopup<T> popup, T result)
        {
            var popupPage = (PopupPage)popup;
            var popupId = this._objectIDGenerator.GetId(popupPage, out var _);
            var completionSource = (TaskCompletionSource<T>)this._popupMapper[popupId];
            this._popupMapper.Remove(popupId);
            completionSource.SetResult(result);
            return PopupNavigation.Instance.RemovePageAsync(popupPage);
        }
    }
}

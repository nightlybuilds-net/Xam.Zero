using System;
using System.Threading.Tasks;
using Xam.Zero.Classes;
using Xam.Zero.Ioc;
using Xam.Zero.Popups;
using Xam.Zero.Services;
using Xamarin.Forms;

namespace Xam.Zero.ViewModels
{
    public class ZeroBaseModel : NotifyBaseModel
    {
        /// <summary>
        /// Previous model hydrated by zeronavigator
        /// </summary>
        public ZeroBaseModel PreviousModel { get; set; }

        
        private Page _currentPage;
        /// <summary>
        /// Current Page setted from Zero framework
        /// </summary>
        public Page CurrentPage
        {
            get => this._currentPage;
            set
            {
                this._currentPage = value;
                
                this._currentPage.Appearing += new WeakEventHandler<EventArgs>(this.CurrentPageOnAppearing).Handler;
                this._currentPage.Disappearing += new WeakEventHandler<EventArgs>(this.CurrentPageOnDisappearing).Handler; 
            }
        }

        #region VIRTUALS

        protected virtual void CurrentPageOnDisappearing(object sender, EventArgs e)
        {
        }

        protected virtual void CurrentPageOnAppearing(object sender, EventArgs e)
        {
        }
        
        protected virtual void PrepareModel(object data)
        {
        }
        
        protected virtual void ReversePrepareModel(object data)
        {
        }

        #endregion

        #region ALERTS

        public Task<bool> DisplayAlert(string title, string message, string accept, string cancel)
        {
            return ZeroApp.Builded.App.MainPage.DisplayAlert(title, message, accept, cancel);
        }
        
        public Task DisplayAlert(string title, string message, string cancel)
        {
            return ZeroApp.Builded.App.MainPage.DisplayAlert(title, message, cancel);
        }

        public Task<string> DisplayActionSheet(string title, string cancel, string destruction, string[] buttons)
        {
            return ZeroApp.Builded.App.MainPage.DisplayActionSheet(title, cancel, destruction, buttons);
        }

        public Task<string> DisplayPrompt(string title, string message, string accept = "OK", string cancel = "Cancel", string placeholder = null, int maxLength = -1, Keyboard keyboard = null, string initialValue = "")
        {
            return ZeroApp.Builded.App.MainPage.DisplayPromptAsync(title, message, accept, cancel, placeholder, maxLength, keyboard, initialValue);
        }
        

        #endregion

        #region PAGE Navigation

        /// <summary>
        /// Go to page
        /// </summary>
        /// <param name="data">pass data</param>
        /// <param name="animated">animated?</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Task Push<T>(object data = null, bool animated = true) where T : Page
        {
            var page = this.ResolvePageWithContext<T>(data);
            return this.CurrentPage.Navigation.PushAsync(page, animated);
        }

        /// <summary>
        /// Go to page by explicit type
        /// </summary>
        /// <param name="pageType">This type must be a Page</param>
        /// <param name="data"></param>
        /// <param name="animated"></param>
        public Task Push(Type pageType, object data = null, bool animated = true)
        {
            if (!pageType.IsSubclassOf(typeof(Page)))
                throw new Exception("Parameter [pageType] must be a Page");

            var page = this.ResolvePageWithContext(pageType, data);
            return this.CurrentPage.Navigation.PushAsync(page, animated);

        }

        

        /// <summary>
        /// Go to page modally
        /// </summary>
        /// <param name="data">pass data</param>
        /// <param name="animated">animated</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Task PushModal<T>(object data = null, bool animated = true) where T : Page
        {
            var page = this.ResolvePageWithContext<T>(data);
            return this.CurrentPage.Navigation.PushModalAsync(page, animated);
        }
        
        /// <summary>
        /// Pop page from stack
        /// </summary>
        /// <param name="data">data to pass back</param>
        /// <param name="animated"></param>
        /// <returns></returns>
        public Task Pop(object data = null, bool animated = true)
        {
            this.PreviousModel?.ReversePrepareModel(data);
            return this.CurrentPage.Navigation.PopAsync(animated);
        }
        
        /// <summary>
        /// Popo modal page from stack
        /// </summary>
        /// <param name="data">data to pass back</param>
        /// <param name="animated"></param>
        /// <returns></returns>
        public Task PopModal(object data = null, bool animated = true)
        {
            this.PreviousModel?.ReversePrepareModel(data);
            return this.CurrentPage.Navigation.PopModalAsync(animated);
        }
        
        /// <summary>
        /// Resolve Page with context
        /// </summary>
        /// <param name="data"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private Page ResolvePageWithContext<T>(object data) where T : Page
        {
            var resolver = ZeroIoc.Container.Resolve<IPageResolver>();
            var page = resolver.ResolvePage<T>(this, data);
            return page;
        }
        
        /// <summary>
        /// Resolve a page with context using explicit type
        /// </summary>
        /// <param name="pageType"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private Page ResolvePageWithContext(Type pageType, object data)
        {
            var resolver = ZeroIoc.Container.Resolve<IPageResolver>();
            var page = resolver.ResolvePage(pageType,this, data);
            return page;
        }

        #endregion

        #region POPUP Navigation
        public Task ShowPopup<P>(object data = null) where P : IXamZeroPopup
        {
            var popup = ResolvePopupWithContext<P>(data);
            return this.CurrentPage.Navigation.ShowPopupAsync(popup);
        }

        public Task<T> ShowPopup<P, T>(object data = null) where P : IXamZeroPopup<T>
        {
            var popup = ResolvePopupWithContext<P, T>(data);
            return this.CurrentPage.Navigation.ShowPopupAsync(popup);
        }

        private IXamZeroPopup ResolvePopupWithContext<P>(object data) where P : IXamZeroPopup
        {
            var resolver = ZeroIoc.Container.Resolve<IPopupResolver>();
            var popup = resolver.ResolvePopup<P>(data);
            return popup;
        }

        private IXamZeroPopup<T> ResolvePopupWithContext<P, T>(object data) where P : IXamZeroPopup<T>
        {
            var resolver = ZeroIoc.Container.Resolve<IPopupResolver>();
            var popup = resolver.ResolvePopup<P, T>(data);
            return popup;
        }
        #endregion
    }
}
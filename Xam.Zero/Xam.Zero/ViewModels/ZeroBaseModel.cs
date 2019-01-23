using System;
using System.Threading.Tasks;
using Xam.Zero.Ioc;
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
                if (this._currentPage != null)
                {
                    this._currentPage.Appearing -= this.CurrentPageOnAppearing;
                    this._currentPage.Disappearing -= this.CurrentPageOnDisappearing;
                }
                
                this._currentPage = value;
                
                this._currentPage.Appearing += this.CurrentPageOnAppearing;
                this._currentPage.Disappearing += this.CurrentPageOnDisappearing;
            }
        }

        #region VIRTUALS

        protected virtual void CurrentPageOnDisappearing(object sender, EventArgs e)
        {}

        protected virtual void CurrentPageOnAppearing(object sender, EventArgs e)
        {}
        
        protected virtual Task Init(object data)
        {
            return Task.CompletedTask;
        }
        
        protected virtual Task ReverseInit(object data)
        {
            return Task.CompletedTask;
        }

        #endregion

        #region ALERTS

        public Task<bool> DisplayAlert(string title, string message, string accept, string cancel)
        {
            return this.CurrentPage?.DisplayAlert(title, message, accept, cancel);
        }
        
        public Task DisplayAlert(string title, string message, string cancel)
        {
            return this.CurrentPage?.DisplayAlert(title, message, cancel);
        }

        public Task<string> DisplayActionSheet(string title, string cancel, string destruction, string[] buttons)
        {
            return this.CurrentPage?.DisplayActionSheet(title, cancel, destruction, buttons);
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
        public async Task GoTo<T>(object data, bool animated = true) where T : Page
        {
            var page = await this.ResolvePageWithContext<T>(data);
            await this.CurrentPage.Navigation.PushAsync(page, animated);
        }
        
        /// <summary>
        /// Go to page modally
        /// </summary>
        /// <param name="data">pass data</param>
        /// <param name="animated">animated</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task GoModalTo<T>(object data, bool animated = true) where T : Page
        {
            var page = await this.ResolvePageWithContext<T>(data);
            await this.CurrentPage.Navigation.PushModalAsync(page, animated);
        }
        
        /// <summary>
        /// Pop page from stack
        /// </summary>
        /// <param name="data">data to pass back</param>
        /// <param name="animated"></param>
        /// <returns></returns>
        public async Task Pop(object data = null, bool animated = true)
        {
            if (this.PreviousModel != null) await this.PreviousModel.ReverseInit(data);
            await this.CurrentPage.Navigation.PopAsync(animated);
        }
        
        /// <summary>
        /// Popo modal page from stack
        /// </summary>
        /// <param name="data">data to pass back</param>
        /// <param name="animated"></param>
        /// <returns></returns>
        public async Task PopModal(object data = null, bool animated = true)
        {
            if (this.PreviousModel != null) await this.PreviousModel.ReverseInit(data);
            await this.CurrentPage.Navigation.PopModalAsync(animated);
        }
        
        /// <summary>
        /// Resolve Page with context
        /// </summary>
        /// <param name="data"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private async Task<Page> ResolvePageWithContext<T>(object data) where T : Page
        {
            var page = ZeroIoc.Container.Resolve<T>();
            var context = (ZeroBaseModel) page.BindingContext;
            context.CurrentPage = page;
            context.PreviousModel = this;
            await context.Init(data);
            return page;
        }

        #endregion


        #region VM Navigation

//        public async Task GoToVm<T>(object data, bool animated = true) where T : ZeroBaseModel
//        {
//            var page = await this.ResolvePageFromModel<T>(data);
//            await this.CurrentPage.Navigation.PushAsync(page, animated);
//        }
//        
//        public async Task GoModalToVm<T>(object data, bool animated = true) where T : ZeroBaseModel
//        {
//            var page = await this.ResolvePageFromModel<T>(data);
//            await this.CurrentPage.Navigation.PushModalAsync(page, animated);
//        }
//
//        private async Task<Page> ResolvePageFromModel<T>(object data) where T : ZeroBaseModel
//        {
//            var serviceKey = typeof(T).Name.Replace("Model", "");
//            var page = (Page) Ioc.Container.Resolve<IPageController>(serviceKey: serviceKey);
//            var context = (ZeroBaseModel) page.BindingContext;
//            context.CurrentPage = page;
//            context.PreviousModel = this;
//            await context.Init(data);
//            return page;
//        }
        

        #endregion

      
    }
}
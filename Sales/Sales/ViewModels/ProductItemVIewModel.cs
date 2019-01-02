

namespace Sales.ViewModels
{
    using System;
    using System.Linq;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Sales.Common.Models;
    using Sales.Helpers;
    using Sales.Services;
    using Sales.Views;
    using Xamarin.Forms;

    public class ProductItemViewModel : Product
    {
        #region Attributes
        private ApiService apiService;
        #endregion
            
        #region Constructors
        public ProductItemViewModel()
        {
            apiService = new ApiService();
        }
        #endregion

        #region Commands
        public ICommand DeleteProductCommand
        {
            get
            {
                return new RelayCommand(DeleteProduct);
            }
        }

        private async void DeleteProduct()
        {
            var answer = await Application.Current.MainPage.DisplayAlert
                (Languages.ConfirmLabel, 
                Languages.DeleteConfirmation, 
                Languages.Yes,
                Languages.No
                );

            if (!answer) return;

            //this.IsRunning = true;
            //this.IsEnabled = false;

            var connection = await this.apiService.CheckConnection();

            if (!connection.IsSuccess)
            {
                //this.isRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.Accept
                    );
                //await Application.Current.MainPage.Navigation.PopAsync();
                return;
            }
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var prefix = Application.Current.Resources["UrlPrefix"].ToString();
            var controller = Application.Current.Resources["UrlProductsController"].ToString();

            var response = await this.apiService.Delete(
                url,
                prefix,
                controller,
                this.ProductId);

            if (!response.IsSuccess)
            {
                //this.IsRunning = false;
                //this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    response.Message,
                    Languages.Accept);
                return;
            }

            var ProductsViewModelInstance = ProductsViewModel.GetInstance();

            var deletedProduct =
                ProductsViewModelInstance.MyProducts.Where(
                    p => p.ProductId == this.ProductId).FirstOrDefault();

            if (deletedProduct != null)
                ProductsViewModelInstance.MyProducts.Remove(deletedProduct);

            ProductsViewModelInstance.RefreshList();
        }

        public ICommand EditProductCommand
        {
            get
            {
                return new RelayCommand(EditProduct);
            }
        }

        private async void EditProduct()
        {
            MainViewModel.GetInstance().EditProduct = new EditProductViewModel(this);
            await Application.Current.MainPage.Navigation.PushAsync(new EditProductPage());
        }
        #endregion
    }
}

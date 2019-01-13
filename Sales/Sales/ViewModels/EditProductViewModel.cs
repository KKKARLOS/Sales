﻿namespace Sales.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Plugin.Media;
    using Plugin.Media.Abstractions;
    using Sales.Common.Models;
    using Sales.Helpers;
    using Sales.Services;
    using Xamarin.Forms;

    public class EditProductViewModel : BaseViewModel
    {
        #region Attributes
        private Product product;
        private ApiService apiService;

        private ImageSource imageSource;
        private MediaFile file;

        private bool isRunning;
        private bool isEnabled;

        private ObservableCollection<Category> categories;
        private Category category;

        #endregion
        #region Properties
        public Product Product
        {
            get { return this.product; }
            set { this.SetValue(ref this.product, value); }
        }
        public bool IsRunning
        {
            get { return this.isRunning; }
            set { this.SetValue(ref this.isRunning, value); }
        }
        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { this.SetValue(ref this.isEnabled, value); }
        }
        public ImageSource ImageSource
        {
            get { return this.imageSource; }
            set { this.SetValue(ref this.imageSource, value); }
        }
        public List<Category> MyCategories { get; set; }

        public Category Category
        {
            get { return this.category; }
            set { this.SetValue(ref this.category, value); }
        }
        public ObservableCollection<Category> Categories
        {
            get { return this.categories; }
            set { this.SetValue(ref this.categories, value); }
        }

        #endregion

        #region Constructors
        public EditProductViewModel(Product product)
        {
            this.product = product;
            apiService = new ApiService();
            this.IsEnabled = true;
            this.ImageSource = product.ImageFullPath;
            this.LoadCategories();
        }
        #endregion
        #region Methods
        private async void LoadCategories()
        {
            this.IsRunning = true;
            this.IsEnabled = false;

            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, connection.Message, Languages.Accept);
                return;
            }

            var answer = await this.LoadCategoriesFromAPI();
            if (answer)
            {
                this.RefreshList();
            }

            this.Category = this.MyCategories.FirstOrDefault(c => c.CategoryId == this.Product.CategoryId);

            this.IsRunning = false;
            this.IsEnabled = true;
        }

        private void RefreshList()
        {
            this.Categories = new ObservableCollection<Category>(this.MyCategories.OrderBy(c => c.Description));
        }

        private async Task<bool> LoadCategoriesFromAPI()
        {
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var prefix = Application.Current.Resources["UrlPrefix"].ToString();
            var controller = Application.Current.Resources["UrlCategoriesController"].ToString();
            var response = await this.apiService.GetList<Category>(url, prefix, controller, Settings.TokenType, Settings.AccessToken);
            if (!response.IsSuccess)
            {
                return false;
            }

            this.MyCategories = (List<Category>)response.Result;
            return true;
        }
        #endregion

        #region Commands
        public ICommand SaveCommand
        {
            get
            {
                return new RelayCommand(Save);
            }
        }
        private async void Save()
        {
            if (string.IsNullOrEmpty(Product.Description))
            {
                await Application.Current.MainPage.DisplayAlert(Languages.Error, Languages.DescriptionError, Languages.Accept);
                return;
            }

            if (this.Product.Price < 0)
            {
                await Application.Current.MainPage.DisplayAlert(Languages.Error, Languages.PriceError, Languages.Accept);
                return;
            }

            if (this.Category == null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.CategoryError,
                    Languages.Accept);
                return;
            }

            this.IsRunning = true;
            this.IsEnabled = false;

            var connection = await this.apiService.CheckConnection();

            if (!connection.IsSuccess)
            {
                this.isRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.Accept
                    );
                //await Application.Current.MainPage.Navigation.PopAsync();
                await App.Navigator.PopAsync();
                return;
            }

            this.Product.CategoryId = this.Category.CategoryId;

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var prefix = Application.Current.Resources["UrlPrefix"].ToString();
            var controller = Application.Current.Resources["UrlProductsController"].ToString();


            byte[] imageArray = null;
            if (this.file != null)
            {
                imageArray = FilesHelper.ReadFully(this.file.GetStream());
                this.Product.ImageArray = imageArray;
            }

            var response = await this.apiService.Put(
                url,
                prefix,
                controller,
                this.Product,
                this.Product.ProductId,
                Settings.TokenType,
                Settings.AccessToken
                );

            if (!response.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    response.Message,
                    Languages.Accept);
                return;
            }
            var ProductsViewModelInstance = ProductsViewModel.GetInstance();

            var oldProduct =
                ProductsViewModelInstance.MyProducts.Where(
                    p => p.ProductId == this.Product.ProductId).FirstOrDefault();

            if (oldProduct != null)
                ProductsViewModelInstance.MyProducts.Remove(oldProduct);

            var newProduct = (Product)response.Result;
            ProductsViewModelInstance.MyProducts.Add(newProduct);
            ProductsViewModelInstance.RefreshList();

            this.IsRunning = false;
            this.IsEnabled = true;

            //await Application.Current.MainPage.Navigation.PopAsync();
            await App.Navigator.PopAsync();
        }
        public ICommand DeleteCommand
        {
            get
            {
                return new RelayCommand(Delete);
            }
        }

        private async void Delete()
        {
            var answer = await Application.Current.MainPage.DisplayAlert
                (Languages.Confirm,
                Languages.DeleteConfirmation,
                Languages.Yes,
                Languages.No
                );

            if (!answer) return;

            this.IsRunning = true;
            this.IsEnabled = false;

            var connection = await this.apiService.CheckConnection();

            if (!connection.IsSuccess)
            {
                this.isRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.Accept
                    );
                //await Application.Current.MainPage.Navigation.PopAsync();
                await App.Navigator.PopAsync();
                return;
            }
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var prefix = Application.Current.Resources["UrlPrefix"].ToString();
            var controller = Application.Current.Resources["UrlProductsController"].ToString();

            var response = await this.apiService.Delete(
                url,
                prefix,
                controller,
                this.Product.ProductId,
                Settings.TokenType,
                Settings.AccessToken
                );

            if (!response.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    response.Message,
                    Languages.Accept);
                //await Application.Current.MainPage.Navigation.PopAsync();
                await App.Navigator.PopAsync();
                return;
            }

            var ProductsViewModelInstance = ProductsViewModel.GetInstance();

            var deletedProduct =
                ProductsViewModelInstance.MyProducts.Where(
                    p => p.ProductId == this.Product.ProductId).FirstOrDefault();

            if (deletedProduct != null)
                ProductsViewModelInstance.MyProducts.Remove(deletedProduct);

            ProductsViewModelInstance.RefreshList();

            this.IsRunning = false;
            this.IsEnabled = true;
            //await Application.Current.MainPage.Navigation.PopAsync();
            await App.Navigator.PopAsync();
        }
        public ICommand ChangeImageCommand
        {
            get
            {
                return new RelayCommand(ChangeImage);
            }
        }
        private async void ChangeImage()
        {
            await CrossMedia.Current.Initialize();

            var source = await Application.Current.MainPage.DisplayActionSheet(
                Languages.ImageSource,
                Languages.Cancel,
                null,
                Languages.FromGallery,
                Languages.NewPicture);

            if (source == Languages.Cancel)
            {
                this.file = null;
                return;
            }

            if (source == Languages.NewPicture)
            {
                this.file = await CrossMedia.Current.TakePhotoAsync(
                    new StoreCameraMediaOptions
                    {
                        Directory = "Sample",
                        Name = "test.jpg",
                        PhotoSize = PhotoSize.Small,
                    }
                );
            }
            else
            {
                this.file = await CrossMedia.Current.PickPhotoAsync();
            }

            if (this.file != null)
            {
                this.ImageSource = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    return stream;
                });
            }
        }
        #endregion
    }
}

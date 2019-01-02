﻿
namespace Sales.ViewModels
{
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Sales.Helpers;
    using Xamarin.Forms;
    using Services;
    using Sales.Common.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Collections.ObjectModel;
    using Plugin.Media.Abstractions;
    using Plugin.Media;

    public class AddProductViewModel : BaseViewModel
    {
        #region Attributes
        private ApiService apiService;

        private ImageSource imageSource;
        private MediaFile file;

        private bool isRunning;
        private bool isEnabled;
        #endregion

        #region Properties
        public string Description { get; set; }
        public string Price { get; set; }
        public string Remarks { get; set; }
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
        #endregion
        #region Constructors
        public AddProductViewModel()
        {
            apiService = new ApiService();
            this.IsEnabled = true;
            this.ImageSource = "noproduct";
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
            if (string.IsNullOrEmpty(this.Description))
            {
                await Application.Current.MainPage.DisplayAlert(Languages.Error, Languages.DescriptionError, Languages.Accept);
                return;
            }
            if (string.IsNullOrEmpty(this.Price))
            {
                await Application.Current.MainPage.DisplayAlert(Languages.Error, Languages.PriceError, Languages.Accept);
                return;
            }
            var price = decimal.Parse(this.Price);

            if (price < 0)
            {
                await Application.Current.MainPage.DisplayAlert(Languages.Error, Languages.PriceError, Languages.Accept);
                return;
            }
            this.IsRunning = true;
            this.IsEnabled= false;

            var connection = await this.apiService.CheckConnection();

            if (!connection.IsSuccess)
            {
                this.isRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.Accept
                    );
                await Application.Current.MainPage.Navigation.PopAsync();
                return;
            }
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var prefix = Application.Current.Resources["UrlPrefix"].ToString();
            var controller = Application.Current.Resources["UrlProductsController"].ToString();


            byte[] imageArray = null;
            if (this.file != null)
            {
                imageArray = FilesHelper.ReadFully(this.file.GetStream());
            }

            var product = new Product
            {
                Description = this.Description,
                Price = price,
                Remarks = this.Remarks,
                ImageArray = imageArray,
            };

            var response = await this.apiService.Post(
                url,
                prefix,
                controller,
                product);

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

            var newProduct = (Product)response.Result;
            var ProductsViewModelInstance = ProductsViewModel.GetInstance();
            ProductsViewModelInstance.MyProducts.Add(newProduct);
            ProductsViewModelInstance.RefreshList();
/*
            ProductsViewModelInstance.Products.Add(new ProductItemViewModel
            {
                Description = newProduct.Description,
                ImageArray = newProduct.ImageArray,
                ImagePath = newProduct.ImagePath,
                IsAvailable = newProduct.IsAvailable,
                Price = newProduct.Price,
                ProductId = newProduct.ProductId,
                PublishOn = newProduct.PublishOn,
                Remarks = newProduct.Remarks
            });


            IEnumerable<ProductItemViewModel> ProductosOrdenados = ProductsViewModelInstance.Products.OrderBy(p => p.Description);
            var myObservableCollection = new ObservableCollection<ProductItemViewModel>(ProductosOrdenados);
            ProductsViewModelInstance.Products = myObservableCollection;
*/
            this.IsRunning = false;
            this.IsEnabled = true;

            //await Application.Current.MainPage.DisplayAlert(
            //    Languages.ConfirmLabel,
            //    Languages.InsertOK,
            //    Languages.Accept);

            await Application.Current.MainPage.Navigation.PopAsync();
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

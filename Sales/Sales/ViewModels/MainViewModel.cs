﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Sales.Views;
using Xamarin.Forms;

namespace Sales.ViewModels
{
    public class MainViewModel
    {
        #region Properties (ViewModels)
        public ProductsViewModel Products { get; set; }
        public AddProductViewModel AddProduct { get; set; }
        public EditProductViewModel EditProduct { get; set; }
        #endregion

        #region Constructors
        public MainViewModel()
        {
            instance = this;
            this.Products = new ProductsViewModel();
        }
        #endregion
        #region Command
        public ICommand AddProductCommand
        {
            get
            {
                return new RelayCommand(GoToAddProduct);
            }
        }

        private async void GoToAddProduct()
        {
            //MainPage = new NavigationPage(new ProductsPage());
            this.AddProduct = new AddProductViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new AddProductPage());
        }
        #endregion
        #region Singleton
        private static MainViewModel instance;

        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                return new MainViewModel();
            }
            return instance;
        }
        #endregion
    }
}
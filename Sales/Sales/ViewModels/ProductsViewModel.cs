namespace Sales.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Sales.Common.Models;
    using Sales.Helpers;
    using Sales.Services;
    using Sales.Common;
    using Xamarin.Forms;

    public  class ProductsViewModel : BaseViewModel
    {
        #region Services
        private ApiService apiService;
        private DataService dataservice;
        #endregion

        #region Attributes
        private bool isRefreshing;
        private ObservableCollection<ProductItemViewModel> products;
        private string filter;
        #endregion

        #region Properties
        public Category Category { get; set; }
        public List<Product> MyProducts { get; set; }
        public ObservableCollection<ProductItemViewModel> Products
        {
            get { return this.products; }
            set { this.SetValue(ref this.products, value); }
        }
        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { SetValue(ref this.isRefreshing, value); }
        }
        public string Filter
        {
            get { return this.filter; }
            set
            {
                SetValue(ref this.filter, value);
                this.RefreshList();
            }
        }
        #endregion

        #region Constructors

        public ProductsViewModel(Category category)
        {
            instance = this;
            apiService = new ApiService();
            dataservice = new DataService();
            this.LoadProducts();
            this.Category = category;
        }
        #endregion
        #region Singleton
        private static ProductsViewModel instance;
        private CategoryItemViewModel categoryItemViewModel;

        public static ProductsViewModel GetInstance()
        {
             return instance;
        }
        #endregion
        #region Methods
        /* Grabando en base de datos local con SQLITE

                private async void LoadProducts()
                {
                    this.IsRefreshing = true;
                    var connection = await this.apiService.CheckConnection();

                    if (connection.IsSuccess)
                    {
                        var answer = await LoadProductsFromAPI();
                        if (answer) await SaveProductsToDB();
                    }
                    else
                    {
                        await this.LoadProductsFromBD();
                    }

                    if (this.MyProducts==null || this.MyProducts.Count == 0)
                    {
                        this.IsRefreshing = false;

                        await Application.Current.MainPage.DisplayAlert(
                                        Languages.Error,
                                        Languages.NoProductsMessage,
                                        Languages.Accept
                                        );
                        return;
                    }

                    this.RefreshList();
                    this.IsRefreshing = false;
                }

                private async Task LoadProductsFromBD()
                {
                    this.MyProducts = await dataservice.GetAllProducts();
                }

                private async Task SaveProductsToDB()
                {
                    await dataservice.DeleteAllProducts();
                    await dataservice.Insert(MyProducts);
                }

                private async Task<bool> LoadProductsFromAPI()
                {
                    var url = Application.Current.Resources["UrlAPI"].ToString();
                    var prefix = Application.Current.Resources["UrlPrefix"].ToString();
                    var controller = Application.Current.Resources["UrlProductsController"].ToString();
                    this.IsRefreshing = true;

                    var response = await apiService.GetList<Product>
                        (
                        url,
                        prefix,
                        controller,
                        Settings.TokenType,
                        Settings.AccessToken
                        );
                    if (!response.IsSuccess)
                    {
                        return false;
                    }
                    this.MyProducts = (List<Product>)response.Result;
                    return true;
                }
        */
        private async void LoadProducts()
        {
            this.IsRefreshing = true;

            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, connection.Message, Languages.Accept);
                return;
            }

            var answer = await this.LoadProductsFromAPI();
            if (answer)
            {
                this.RefreshList();
            }

            this.IsRefreshing = false;
        }

        private async Task<bool> LoadProductsFromAPI()
        {
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var prefix = Application.Current.Resources["UrlPrefix"].ToString();
            var controller = Application.Current.Resources["UrlProductsController"].ToString();
            var response = await this.apiService.GetList<Product>(url, prefix, controller, this.Category.CategoryId, Settings.TokenType, Settings.AccessToken);
            if (!response.IsSuccess)
            {
                return false;
            }

            this.MyProducts = (List<Product>)response.Result;
            return true;
        }

        public void RefreshList()
        {
            if (string.IsNullOrEmpty(this.Filter))
            {
                var myListProductItemViewModel = MyProducts.Select(p => new ProductItemViewModel
                {
                    Description = p.Description,
                    ImageArray = p.ImageArray,
                    ImagePath = p.ImagePath,
                    IsAvailable = p.IsAvailable,
                    Price = p.Price,
                    ProductId = p.ProductId,
                    PublishOn = p.PublishOn,
                    Remarks = p.Remarks,
                    CategoryId = p.CategoryId,
                    UserId = p.UserId
                });
                this.Products = new ObservableCollection<ProductItemViewModel>(myListProductItemViewModel.OrderBy(p => p.Description));
            }
            else
            {
                var myListProductItemViewModel = MyProducts.Select(p => new ProductItemViewModel
                {
                    Description = p.Description,
                    ImageArray = p.ImageArray,
                    ImagePath = p.ImagePath,
                    IsAvailable = p.IsAvailable,
                    Price = p.Price,
                    ProductId = p.ProductId,
                    PublishOn = p.PublishOn,
                    Remarks = p.Remarks,
                    CategoryId = p.CategoryId,
                    UserId = p.UserId
                }).Where(p => p.Description.ToLower().Contains(this.Filter.ToLower())).ToList();
                this.Products = new ObservableCollection<ProductItemViewModel>(myListProductItemViewModel.OrderBy(p => p.Description));
            }
        }
        #endregion
        #region Commands
        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(LoadProducts);
            }
        }
        public ICommand SearchCommand
        {
            get
            {
                return new RelayCommand(RefreshList);
            }
        }
        #endregion
    }
}

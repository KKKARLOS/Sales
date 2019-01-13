namespace Sales.Helpers
{
    using Xamarin.Forms;
    using Interfaces;
    using Resources;

    public static class Languages
    {
        static Languages()
        {
            var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            Resource.Culture = ci;
            DependencyService.Get<ILocalize>().SetLocale(ci);
        }

        public static string Accept
        {
            get { return Resource.Accept; }
        }

        public static string EmailValidation
        {
            get { return Resource.EmailValidation; }
        }

        public static string EmailPasswordValidation
        {
            get { return Resource.EmailPasswordValidation; }
        }
        public static string Error
        {
            get { return Resource.Error; }
        }

        public static string EmailPlaceHolder
        {
            get { return Resource.EmailPlaceHolder; }
        }

        public static string Rememberme
        {
            get { return Resource.Rememberme; }
        }

        public static string PasswordValidation
        {
            get { return Resource.PasswordValidation; }
        }

        public static string SomethingWrong
        {
            get { return Resource.SomethingWrong; }
        }

        public static string Login
        {
            get { return Resource.Login; }
        }

        public static string EMail
        {
            get { return Resource.EMail; }
        }

        public static string Password
        {
            get { return Resource.Password; }
        }

        public static string PasswordPlaceHolder
        {
            get { return Resource.PasswordPlaceHolder; }
        }

        public static string Forgot
        {
            get { return Resource.Forgot; }
        }

        public static string Register
        {
            get { return Resource.Register; }
        }

        public static string Search
        {
            get { return Resource.Search; }
        }

        public static string Information
        {
            get { return Resource.Information; }
        }

        public static string Menu
        {
            get { return Resource.Menu; }
        }

        public static string MyProfile
        {
            get { return Resource.MyProfile; }
        }

        public static string Statics
        {
            get { return Resource.Statics; }
        }

        public static string LogOut
        {
            get { return Resource.LogOut; }
        }
        public static string RegisterTitle
        {
            get { return Resource.RegisterTitle; }
        }

        public static string ChangeImage
        {
            get { return Resource.ChangeImage; }
        }

        public static string FirstName
        {
            get { return Resource.FirstName; }
        }

        public static string FirstNamePlaceHolder
        {
            get { return Resource.FirstNamePlaceHolder; }
        }

        public static string FirstNameValidation
        {
            get { return Resource.FirstNameValidation; }
        }

        public static string LastName
        {
            get { return Resource.LastName; }
        }

        public static string LastNamePlaceHolder
        {
            get { return Resource.LastNamePlaceHolder; }
        }

        public static string LastNameValidation
        {
            get { return Resource.LastNameValidation; }
        }

        public static string Phone
        {
            get { return Resource.Phone; }
        }

        public static string PhonePlaceHolder
        {
            get { return Resource.PhonePlaceHolder; }
        }

        public static string PhoneValidation
        {
            get { return Resource.PhoneValidation; }
        }

        public static string Confirm
        {
            get { return Resource.Confirm; }
        }

        public static string ConfirmPlaceHolder
        {
            get { return Resource.ConfirmPlaceHolder; }
        }

        public static string ConfirmValidation
        {
            get { return Resource.ConfirmValidation; }
        }

        public static string EmailValidation2
        {
            get { return Resource.EmailValidation2; }
        }

        public static string PasswordValidation2
        {
            get { return Resource.PasswordValidation2; }
        }

        public static string ConfirmValidation2
        {
            get { return Resource.ConfirmValidation2; }
        }

        public static string UserRegisteredMessage
        {
            get { return Resource.UserRegisteredMessage; }
        }

        public static string SourceImageQuestion
        {
            get { return Resource.SourceImageQuestion; }
        }

        public static string Cancel
        {
            get { return Resource.Cancel; }
        }

        public static string FromGallery
        {
            get { return Resource.FromGallery; }
        }

        public static string FromCamera
        {
            get { return Resource.FromCamera; }
        }

        public static string Save
        {
            get { return Resource.Save; }
        }

        public static string ChangePassword
        {
            get { return Resource.ChangePassword; }
        }

        public static string CurrentPassword
        {
            get { return Resource.CurrentPassword; }
        }

        public static string CurrentPasswordPlaceHolder
        {
            get { return Resource.CurrentPasswordPlaceHolder; }
        }

        public static string NewPassword
        {
            get { return Resource.NewPassword; }
        }

        public static string NewPasswordPlaceHolder
        {
            get { return Resource.NewPasswordPlaceHolder; }
        }

        public static string TurnOnInternet
        {
            get { return Resource.TurnOnInternet; }
        }

        public static string NoInternet
        {
            get { return Resource.NoInternet; }
        }

        public static string LoginError
        {
            get { return Resource.LoginError; }
        }

        public static string ChagePasswordConfirm
        {
            get { return Resource.ChagePasswordConfirm; }
        }

        public static string PasswordError
        {
            get { return Resource.PasswordError; }
        }

        public static string ErrorChangingPassword
        {
            get { return Resource.ErrorChangingPassword; }
        }
        public static string Products
        {
            get { return Resource.Products; }
        }
        public static string AddProduct
        {
            get { return Resource.AddProduct; }
        }
        public static string Description
        {
            get { return Resource.Description; }
        }
        public static string DescriptionPlaceHolder
        {
            get { return Resource.DescriptionPlaceHolder; }
        }
        public static string Price
        {
            get { return Resource.Price; }
        }
        public static string PricePlaceHolder
        {
            get { return Resource.PricePlaceHolder; }
        }
        public static string Remarks
        {
            get { return Resource.Remarks; }
        }
        public static string DescriptionError
        {
            get { return Resource.DescriptionError; }
        }
        public static string PriceError
        {
            get { return Resource.PriceError; }
        }
        public static string InsertOK
        {
            get { return Resource.InsertOK; }
        }
        public static string ImageSource
        {
            get { return Resource.ImageSource; }
        }
        public static string NewPicture
        {
            get { return Resource.NewPicture; }
        }
        public static string Edit
        {
            get { return Resource.Edit; }
        }
        public static string Delete
        {
            get { return Resource.Delete; }
        }
        public static string DeleteConfirmation
        {
            get { return Resource.DeleteConfirmation; }
        }
        public static string Yes
        {
            get { return Resource.Yes; }
        }
        public static string No
        {
            get { return Resource.No; }
        }
        public static string IsAvailable
        {
            get { return Resource.IsAvailable; }
        }
        public static string EditProduct
        {
            get { return Resource.EditProduct; }
        }
        public static string About
        {
            get { return Resource.About; }
        }
        public static string Setup
        {
            get { return Resource.Setup; }
        }
        public static string Exit
        {
            get { return Resource.Exit; }
        }
        public static string NoProductsMessage
        {
            get { return Resource.NoProductsMessage; }
        }
        public static string Address
        {
            get { return Resource.Address; }
        }
        public static string AddressPlaceHolder
        {
            get { return Resource.AddressPlaceHolder; }
        }
      
        public static string PasswordsNoMatch
        {
            get { return Resource.PasswordsNoMatch; }
        }
        public static string RegisterConfirmation
        {
            get { return Resource.RegisterConfirmation; }
        }
        public static string Categories
        {
            get { return Resource.Categories; }
        }
        public static string Category
        {
            get { return Resource.Category; }
        }
        public static string CategoryPlaceHolder
        {
            get { return Resource.CategoryPlaceHolder; }
        }
        public static string CategoryError
        {
            get { return Resource.CategoryError; }
        }


    }
}
using System;
using System.Collections.Generic;
using System.Text;
using Sales.ViewModels;

namespace Sales.Infrastructure
{
    public class instanceLocator
    {
        public MainViewModel Main { get; set; }
        public instanceLocator()
        {
            Main = new MainViewModel();
        }
    }
}

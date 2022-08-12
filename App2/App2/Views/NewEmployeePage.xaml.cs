using App2.Models;
using App2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewEmployeePage : ContentPage
    {
        public Employee Employee { get; set; }
        public NewEmployeePage()
        {
            InitializeComponent();
            this.BindingContext = new NewEmployeeViewModel();
        }
    }
}
using App2.Services;
using App2.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace App2.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command LoginCommand { get; }
        private string _userName;
        private string _password;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Username should not be empty")]
        public string UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                _userName = value;
                LoginCommand.ChangeCanExecute();
            }
        }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password should not be empty")]
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
                LoginCommand.ChangeCanExecute();
            }
        }

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked, IsAllowLogin);
        }
        public bool IsAllowLogin(object obj) => !string.IsNullOrEmpty(_userName) && !string.IsNullOrEmpty(_password);
        private async void OnLoginClicked(object obj)
        {
            this.IsBusy = true;
            if (Connectivity.NetworkAccess == NetworkAccess.None)
            {
                await App.Current.MainPage.DisplayAlert("Cannot connect", "Check your internet connection", "Ok");
                this.IsBusy = false;
                return;
            }
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            if (!string.IsNullOrEmpty(_userName) && !string.IsNullOrEmpty(_password))
            {
                LoginService loginService = new LoginService();
                var res = await loginService.Login(_userName, _password);
                if (res != String.Empty)
                {
                    this.IsBusy = false;
                    await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
                }
            }
            

        }
    }
}

using App2.Views;
using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace App2.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "About";
            OpenWebCommand = new Command(async () => await Shell.Current.GoToAsync("///ItemsPage"));
            _imageProfile = ImageSource.FromResource("App2.Imgs.BingWallpaper.jpg", typeof(LoginPage).Assembly);
        }

        public ICommand OpenWebCommand { get; }
        private ImageSource _imageProfile;
        public ImageSource ImageProfile
        {
            get 
            { 
                if( _imageProfile == null )
                    _imageProfile = ImageSource.FromResource("App2.Imgs.BingWallpaper.jpg", typeof(LoginPage).Assembly);
                return _imageProfile; 
            }
            set 
            { 
                _imageProfile = value;
                OnPropertyChanged("ImageProfile"); 
            }
        }

    }
}
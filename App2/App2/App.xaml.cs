using App2.Models;
using App2.Services;
using App2.Views;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2
{
    public partial class App : Application
    {
        private static string _baseUrl = "http://10.1.11.100:8000/api/";
        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            DependencyService.Register<EmployeeService>();
            DependencyService.RegisterSingleton<HttpClient>(GetHttpClient());
            MainPage = new AppShell();
        }
        private static HttpClient GetHttpClient()
        {

#if DEBUG
            HttpClientHandler insecureHandler = GetInsecureHandler();
            var httpclient = new HttpClient(insecureHandler);
            httpclient.BaseAddress = new Uri(_baseUrl);
            return httpclient;
#else
            var httpclient = new HttpClient();
            httpclient.BaseAddress = new Uri(_baseUrl);
            return httpclient;
#endif

        }
        public static HttpClientHandler GetInsecureHandler()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            {
                if (cert.Issuer.Equals("CN=localhost"))
                    return true;
                return errors == System.Net.Security.SslPolicyErrors.None;
            };
            return handler;
        }
        protected override void OnStart()
        {
            Shell.Current.GoToAsync("//LoginPage");
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}

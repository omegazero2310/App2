using App2.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace App2.Services
{
    public class LoginService
    {
        private static HttpClient _httpClient;
        private string baseUrl = "http://10.1.11.100:8000/api/";
        private void GetClient()
        {
            if (_httpClient == null)
            {
            #if DEBUG
                HttpClientHandler insecureHandler = GetInsecureHandler();
                _httpClient = new HttpClient(insecureHandler);
            #else
                HttpClient client = new HttpClient();
            #endif
            }
        }
        public HttpClientHandler GetInsecureHandler()
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
        public async Task<string> Login(string userName, string password)
        {
            try
            {
                GetClient();
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, baseUrl + "Account");
                UserLogin userLogin = new UserLogin();
                userLogin.UserName = userName;
                userLogin.Password = password;
                message.Content = new StringContent(JsonConvert.SerializeObject(userLogin), Encoding.UTF8, "application/json");
                var respone = await _httpClient.SendAsync(message);
                respone.EnsureSuccessStatusCode();
                //lấy token lưu tạm để dùng cho các lần sau
                await SecureStorage.SetAsync("JWT", respone.Content.ReadAsStringAsync().Result);
                return respone.Content.ReadAsStringAsync().Result;
            }
            catch (Exception)
            {

                throw;
            }
            
        }
    }
}

using App2.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace App2.Services
{
    /// <summary>Check user login and get JWT token</summary>
    /// <Modified>
    /// Name Date Comments
    /// annv3 15/08/2022 created
    /// </Modified>
    public class LoginService
    {
        private HttpClient _httpClient = DependencyService.Get<HttpClient>();
        public async Task<string> Login(string userName, string password)
        {
            try
            {
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, "Account");
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

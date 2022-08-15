using App2.Models;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace App2.Services
{
    internal class EmployeeService : IDataStore<Employee>
    {
        private static HttpClient _httpClient;
        private string _baseUrl = "http://10.1.11.100:8000/api/";
        private UserTokens _userTokens;
        private SQLiteAsyncConnection _conn;
        private string _localDBPath = System.IO.Path.Combine(FileSystem.AppDataDirectory, typeof(EmployeeService).Assembly.GetName().Name + ".db3");
        private void GetHttpClient()
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
            if (_userTokens == null)
            {
                var getJwtString = SecureStorage.GetAsync("JWT").Result;
                _userTokens = JsonConvert.DeserializeObject<UserTokens>(SecureStorage.GetAsync("JWT").Result);
                //if (_userTokens.ExpiredTime.CompareTo(DateTime.Now) < 0)
                //{
                //    throw new Exception("MSG_SESSION_EXPIRED");
                //}
            }
        }
        ~EmployeeService()
        {
            _httpClient?.Dispose();
            _conn?.CloseAsync();
        }
        private async Task InitDB()
        {
            if (_conn != null)
                return;

            _conn = new SQLiteAsyncConnection(this._localDBPath);
            await _conn.CreateTableAsync<Employee>();
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
        public async Task<bool> AddItemAsync(Employee item)
        {
            try
            {
                GetHttpClient();
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, _baseUrl + "Employee");
                message.Content = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");
                //Get Token from SecureStorage
                message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _userTokens.Token);
                var respone = await _httpClient.SendAsync(message);
                respone.EnsureSuccessStatusCode();
                await this.InitDB();
                await _conn.InsertOrReplaceAsync(item);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            try
            {
                GetHttpClient();
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Delete, _baseUrl + "Employee/" + id);
                //Get Token from SecureStorage
                message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _userTokens.Token);
                var respone = await _httpClient.SendAsync(message);
                respone.EnsureSuccessStatusCode();
                await this.InitDB();
                await _conn.DeleteAsync(new Employee { Id = id});
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Employee> GetItemAsync(string id)
        {
            try
            {
                if (Connectivity.NetworkAccess == NetworkAccess.None)
                {
                    await this.InitDB();
                    return await _conn.Table<Employee>().Where(o => o.Id == id).FirstAsync();
                }
                else
                {
                    GetHttpClient();
                    HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, _baseUrl + "Employee/" + id);
                    //Get Token from SecureStorage
                    message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _userTokens.Token);
                    var respone = await _httpClient.SendAsync(message);
                    respone.EnsureSuccessStatusCode();
                    await this.InitDB();
                    await _conn.InsertOrReplaceAsync(JsonConvert.DeserializeObject<Employee>(respone.Content.ReadAsStringAsync().Result));
                    return JsonConvert.DeserializeObject<Employee>(respone.Content.ReadAsStringAsync().Result);
                }               
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<Employee>> GetItemsAsync(bool forceRefresh = false)
        {
            try
            {
                if (Connectivity.NetworkAccess == NetworkAccess.None)
                {
                    await this.InitDB();
                    return await _conn.Table<Employee>().ToListAsync();
                }
                else
                {
                    GetHttpClient();
                    HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, _baseUrl + "Employee");
                    //Get Token from SecureStorage
                    message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _userTokens.Token);
                    var respone = await _httpClient.SendAsync(message);
                    respone.EnsureSuccessStatusCode();
                    await this.InitDB();
                    await _conn.DeleteAllAsync<Employee>();
                    await _conn.InsertAllAsync(JsonConvert.DeserializeObject<IEnumerable<Employee>>(respone.Content.ReadAsStringAsync().Result));
                    return JsonConvert.DeserializeObject<IEnumerable<Employee>>(respone.Content.ReadAsStringAsync().Result);
                }                   
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> UpdateItemAsync(Employee item)
        {
            try
            {
                GetHttpClient();
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Put, _baseUrl + "Employee");
                message.Content = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");
                //Get Token from SecureStorage
                message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _userTokens.Token);
                var respone = await _httpClient.SendAsync(message);
                respone.EnsureSuccessStatusCode();
                await this.InitDB();
                await _conn.UpdateAsync(item);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Configuration;
using System.Security.Policy;

namespace EmployeeMgmt
{
    public class WebAPI: IEmployeeRepository
    {

        /// <summary>  
        /// Get employee details  
        /// </summary>  
        /// <param name="url"></param>  
        /// <returns></returns>  
        public async Task<HttpResponseMessage> GetEmployees(string url)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    SetupHttpClient(url, client);
                    var response =await client.GetAsync(url);
                    return response;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Get Employee details by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> GetEmployeebyID(string url)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    SetupHttpClient(url, client);
                    var response =await client.GetAsync(url);
                    return response;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>  
        /// creates a new employee  
        /// </summary>  
        /// <typeparam name="T"></typeparam>  
        /// <param name="url"></param>  
        /// <param name="model"></param>  
        /// <returns></returns>  
        public async Task<HttpResponseMessage> CreateEmployee<T>(string url, T model) where T : class
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    SetupHttpClient(url, client);
                    var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    var response =await client.PostAsJsonAsync(url, jsonString);
                    return response;
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>  
        /// Updates emplyees details  
        /// </summary>  
        /// <typeparam name="T"></typeparam>  
        /// <param name="url"></param>  
        /// <param name="model"></param>  
        /// <returns></returns>  
        public async Task<HttpResponseMessage> UpdateEmployee<T>(string url, T model) where T : class
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    SetupHttpClient(url, client);
                    var response = await client.PutAsJsonAsync(url, model);
                    return response;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static void SetupHttpClient(string url, HttpClient client)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            client.BaseAddress = new Uri(url);
            client.Timeout = TimeSpan.FromSeconds(900);
            client.DefaultRequestHeaders.Accept.Clear();
            var APIToken = ConfigurationManager.AppSettings["APItoken"];
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", APIToken);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>  
        /// Delete employee's record  
        /// </summary>  
        /// <param name="url"></param>  
        /// <returns></returns>  
        public async Task<HttpResponseMessage> DeleteEmployee(string url)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    SetupHttpClient(url, client);
                    var response = await client.DeleteAsync(url);
                    return response;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}

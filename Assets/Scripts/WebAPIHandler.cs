using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using WEDX.Core.ViewModels;

namespace WEDX.Services
{
    public class WebApiHandler
    {
        #region Variables
        private readonly string _baseUrl;     
        private readonly string _accessToken;
        #endregion

        public WebApiHandler()
        {         
            this._baseUrl = "http://localhost:60856/api/";
            //this._baseUrl = "http://m2.cdnsolutionsgroup.com/FishEye_api/api/";
            this._accessToken = "";
        }

        public ResponseMessage Get(string url)
        {
            ResponseMessage responseMessage = new ResponseMessage();

            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(30);
                client.BaseAddress = new Uri(_baseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _accessToken);

                HttpResponseMessage httpResponseMessage = client.GetAsync(url).Result;
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    responseMessage.Content = httpResponseMessage.Content.ReadAsStringAsync().Result;
                    responseMessage.Status = "Success";
                }
                else
                {
                    responseMessage.Content = httpResponseMessage.Content.ReadAsStringAsync().Result;
                    responseMessage.Status = "Error";                  
                }
            }

            return responseMessage;
        }

        public ResponseMessage Post(string url, object data)
        {
            ResponseMessage responseMessage = new ResponseMessage();

            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(30);
                client.BaseAddress = new Uri(_baseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _accessToken);

                var myContent = JsonConvert.SerializeObject(data);
                var httpContent = new StringContent(myContent, System.Text.Encoding.UTF8, "application/json");          
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

                HttpResponseMessage httpResponseMessage = client.PostAsync(url, httpContent).Result;
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    responseMessage.Content = httpResponseMessage.Content.ReadAsStringAsync().Result;
                    responseMessage.Status = "Success";
                }
                else
                {
                    responseMessage.Content = httpResponseMessage.Content.ReadAsStringAsync().Result;
                    responseMessage.Status = "Error";
                }
            }
            return responseMessage;
        }

        public ResponseMessage Put(string url, object data)
        {
            ResponseMessage responseMessage = new ResponseMessage();

            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(30);
                client.BaseAddress = new Uri(_baseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _accessToken);

                var myContent = JsonConvert.SerializeObject(data);
                var httpContent = new StringContent(myContent, System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage httpResponseMessage = client.PostAsync(url, httpContent).Result;
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    responseMessage.Content = httpResponseMessage.Content.ReadAsStringAsync().Result;
                    responseMessage.Status = "Success";
                }
                else
                {
                    responseMessage.Content = httpResponseMessage.Content.ReadAsStringAsync().Result;
                    responseMessage.Status = "Error";
                }
            }

            return responseMessage;
        }

        public ResponseMessage Delete(string url)
        {
            ResponseMessage responseMessage = new ResponseMessage();

            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(30);
                client.BaseAddress = new Uri(_baseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _accessToken);

                HttpResponseMessage httpResponseMessage = client.DeleteAsync(url).Result;
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    responseMessage.Content = httpResponseMessage.Content.ReadAsStringAsync().Result;
                    responseMessage.Status = "Success";
                }
                else
                {
                    responseMessage.Content = httpResponseMessage.Content.ReadAsStringAsync().Result;
                    responseMessage.Status = "Error";
                }
            }

            return responseMessage;
        }
    }
}

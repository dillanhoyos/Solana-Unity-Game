using UnityEngine;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

public class WebApiHandler
{
    #region Variables
    private readonly string _baseUrl;     
    private readonly string _accessToken;
    #endregion

    public WebApiHandler(string baseURL, string accessToken)
    {         
        this._baseUrl = baseURL;
        this._accessToken = accessToken;
    }

    public HttpResponseMessage Get(string url)
    {
        HttpResponseMessage httpResponseMessage;

        using (var client = new HttpClient())
        {
            client.Timeout = TimeSpan.FromMinutes(30);
            client.BaseAddress = new Uri(_baseUrl);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _accessToken);

            httpResponseMessage = client.GetAsync(url).Result;
        }

        return httpResponseMessage;
    }

    public HttpResponseMessage Post(string url, object data)
    {
        HttpResponseMessage httpResponseMessage;
        using (var client = new HttpClient())
        {
            client.Timeout = TimeSpan.FromMinutes(30);
            client.BaseAddress = new Uri(_baseUrl);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("multipart/form-data"));
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _accessToken);

            MultipartFormDataContent multipartContent = new MultipartFormDataContent();
            var ba = (byte[])data;
            multipartContent.Add(new ByteArrayContent(ba, 0, ba.Length), "image", "image.jpg");
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            httpResponseMessage = client.PostAsync(url, multipartContent).Result;
        }
        return httpResponseMessage;
    }

    public HttpResponseMessage Put(string url, object data)
    {
        HttpResponseMessage httpResponseMessage;
        using (var client = new HttpClient())
        {
            client.Timeout = TimeSpan.FromMinutes(30);
            client.BaseAddress = new Uri(_baseUrl);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _accessToken);

            var myContent = JsonConvert.SerializeObject(data);
            var httpContent = new StringContent(myContent, System.Text.Encoding.UTF8, "application/json");
            httpResponseMessage = client.PostAsync(url, httpContent).Result;
        }
        return httpResponseMessage;
    }

    public HttpResponseMessage Delete(string url)
    {
        HttpResponseMessage httpResponseMessage;
        using (var client = new HttpClient())
        {
            client.Timeout = TimeSpan.FromMinutes(30);
            client.BaseAddress = new Uri(_baseUrl);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _accessToken);
            httpResponseMessage = client.DeleteAsync(url).Result;
        }

        return httpResponseMessage;
    }
}
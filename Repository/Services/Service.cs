using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Entitites;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Repository.Services
{
    public class Service
    {
        public async Task<Owner> LoginApi(string email, string password)
        {
            Owner oOwner = Owner.GetInstance();
            Token oToken = Token.GetInstance();

            HttpClient client = new HttpClient();

            var datos = new
            {
                email = email,
                password = password
            };


            try
            {
                //var restUrl = $"http://52.171.34.15/api/login";
                var stringContent = new StringContent(JsonConvert.SerializeObject(datos), Encoding.UTF8, "application/json");
                var restUrl = $"http://192.168.0.18:8080/api/login";

                HttpResponseMessage response = await client.PostAsync(restUrl, stringContent);
                if (response.StatusCode != HttpStatusCode.OK) return null;

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var algo = JObject.Parse(jsonResponse);
                oOwner = JsonConvert.DeserializeObject<Owner>(algo["owner"].ToString());
                oToken.accessTokenApi = algo["token"].ToString();
                return oOwner;
            }
            catch (Exception ex)
            {
                oOwner.MessageStatusCode = $"Error al conectarse con el servicio {ex.Message}";
                return oOwner;
            }
        }
    }
}
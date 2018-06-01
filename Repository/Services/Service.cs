using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Telephony;
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
                var stringContent = new StringContent(JsonConvert.SerializeObject(datos), Encoding.UTF8, "application/json");
                var restUrl = $"{Config.Endpoint}/login";

                HttpResponseMessage response = await client.PostAsync(restUrl, stringContent);
                if (response.StatusCode != HttpStatusCode.OK) return null;

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var algo = JObject.Parse(jsonResponse);
                oOwner = JsonConvert.DeserializeObject<Owner>(algo["owner"].ToString());
                Owner.SetInstance(oOwner);
                oToken.accessTokenApi = algo["token"].ToString();
                Token.SetInstance(oToken);
                return oOwner;
            }
            catch (Exception ex)
            {
                oOwner.MessageStatusCode = $"Error al conectarse con el servicio {ex.Message}";
                return oOwner;
            }
        }

        public async Task<Owner> RegisterApi(Owner oOwner)
        {
            Owner _oOwner = Owner.GetInstance();
            Token oToken = Token.GetInstance();
            HttpClient client = new HttpClient();

            var datos = new
            {
                firstName = oOwner.firstName,
                lastName = "lastName",
                identificationNumber = oOwner.identificationNumber,
                location = oOwner.location,
                address = oOwner.address,
                phone = oOwner.phone,
                email = oOwner.email,
                password = "qwerty",
            };

            try
            {
                var stringContent = new StringContent(JsonConvert.SerializeObject(datos), Encoding.UTF8, "application/json");
                var _urlRegister = $"{Config.Endpoint}/register";

                var response = await client.PostAsync(_urlRegister, stringContent);

                var jsonResponse = await response.Content.ReadAsStringAsync();

                if (response.StatusCode != HttpStatusCode.Created)
                    throw new Exception(jsonResponse);

                var algo = JObject.Parse(jsonResponse);
                oOwner = JsonConvert.DeserializeObject<Owner>(algo["owner"].ToString());
                Owner.SetInstance(oOwner);
                oToken.accessTokenApi = algo["token"].ToString();
                Token.SetInstance(oToken);
                return oOwner;
            }
            catch (Exception ex)
            {
                oOwner.MessageStatusCode = $"Error al conectarse con el servicio {ex.Message}";
                return oOwner;
            }

        }

        public async Task<Owner> GetOwnerApi(Token oToken)
        {
            Owner oOwner = Owner.GetInstance();
            HttpClient client = new HttpClient();
            var stringContent = new StringContent("", Encoding.UTF8, "application/json");
            var _urlProfile = $"{Config.Endpoint}/profile";
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", oToken.accessTokenApi);

            try
            {

                using (HttpClient oclient = new HttpClient())
                {
                    oclient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", oToken.accessTokenApi);
                    using (HttpResponseMessage response = await client.GetAsync(_urlProfile))
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            using (HttpContent content = response.Content)
                            {
                                string result = await content.ReadAsStringAsync();

                                if (result != null && result.Length >= 50)
                                {
                                    var _json = JsonConvert.DeserializeObject<Owner>(result.ToString());
                                    Owner.SetInstance(_json);
                                }
                            }
                        }
                    }

                    //var algo = JObject.Parse(jsonResponse);
                    //oOwner = JsonConvert.DeserializeObject<Owner>(algo["owner"].ToString());
                    //Owner.SetInstance(oOwner);
                    //oToken.accessTokenApi = algo["token"].ToString();
                    //Token.SetInstance(oToken);

                    return oOwner;

                }
            }
            catch (Exception ex)
            {
                oOwner.MessageStatusCode = $"Error al conectarse con el servicio {ex.Message}";
                return oOwner;
            }

        }

        public async Task<Pet> RegisterPetApi(Pet oPet, Token oToken)
        {

            HttpClient client = new HttpClient();

            var datos = new
            {
                breedId = oPet.breedId,
                ownerId = oPet.ownerId,
                sizeId = oPet.sizeId,
                name = oPet.name,
                dateOfBirth = oPet.dateOfBirth,
                gender = oPet.gender,
                photo = oPet.photo,
            };

            try
            {
                var stringContent = new StringContent(JsonConvert.SerializeObject(datos), Encoding.UTF8, "multipart/form-data");
                var _urlRegister = $"{Config.Endpoint}/profile/pets";
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", oToken.accessTokenApi);
                var response = await client.PostAsync(_urlRegister, stringContent);

                var jsonResponse = await response.Content.ReadAsStringAsync();

                //if (response.StatusCode != HttpStatusCode.Created)
                //    throw new Exception(jsonResponse);

                //var algo = JObject.Parse(jsonResponse);
                //oOwner = JsonConvert.DeserializeObject<Owner>(algo["owner"].ToString());
                //Owner.SetInstance(oOwner);
                //oToken.accessTokenApi = algo["token"].ToString();
                //Token.SetInstance(oToken);
                return oPet;
            }
            catch (Exception ex)
            {
                return oPet;
            }

        }
    }

    public static class Config
    {
        //lost-pets.cloudapp.net
        public static string Endpoint = "http://lost-pets.cloudapp.net/api";//"http://192.168.0.18:8080/api";
    }
}
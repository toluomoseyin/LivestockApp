using AnimalFarmsMarket.Data.DTOs;
using AnimalFarmsMarket.Data.ViewModels;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AnimalFarmsMarket.Commons
{
    public static class FlutterwavePayment
    {

        /// <summary>
        /// This method is used to Authorize the charge with a pin
        /// </summary>
        /// <param name="cardDetails"></param>
        /// <param name="baseUrl"></param>
        /// <param name="partOfUrl"></param>
        /// <param name="token"></param>
        /// <param name="encryptionKey"></param>
        /// <returns></returns>
        public static async Task<FlutterwaveResponseDto> AuthorizePayment(CardDetails cardDetails, string baseUrl, string partOfUrl, string token, string encryptionKey)
        {
            //Re-serialize card details and encrypt
            var responseObject = await PostContentWithAuthorizationAsync<CardDetails>(baseUrl, cardDetails, partOfUrl, token, encryptionKey);

            if (responseObject.Item2.IsSuccessStatusCode)
            {
                var flutterResponse = JsonConvert.DeserializeObject<FlutterwaveResponseDto>
                                        (await responseObject.Item2.Content.ReadAsStringAsync());
                return flutterResponse;
            }
            return null;
        }

        /// <summary>
        /// This Method is used to validate the payment using otp
        /// </summary>
        /// <param name="validationDto"></param>
        /// <param name="baseUrl"></param>
        /// <param name="partOfUrl"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<FlutterwaveResponseDto> ValidatePayment(FlutterwaveValidationDto validationDto, string baseUrl, string partOfUrl,string token)
        {
            //Re-serialize card details and encrypt
            var responseObject = await PostContentWithAuthorizationAsync(baseUrl, validationDto, partOfUrl, token,"");

            if (responseObject.Item2.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<FlutterwaveResponseDto>(await responseObject.Item2.Content.ReadAsStringAsync());
            }
            return null;
        }

        /// <summary>
        /// This Method is used to verify the transaction before value is given to the customer
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="amount"></param>
        /// <param name="baseUrl"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<bool> VerifyPayment(int transactionId, decimal amount, string baseUrl, string token)
        {
            using (var _client = new HttpClient())
            {
                _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                var response = await _client.GetAsync(baseUrl + "/transactions/" + transactionId + "/verify");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var responseObject = JsonConvert.DeserializeObject<FlutterwaveResponseDto>(result);

                    if (responseObject.status != "success" && responseObject.data.amount != amount)
                        return false;
                }
                return true;
            }
        }




        private static Tuple<string, StringContent> BuildReqContent<T>(HttpClient client, string _baseURL, T model, string PartOfUrl,string encryptionKey)
        {
            client.BaseAddress = new Uri(_baseURL);
            client.DefaultRequestHeaders.Accept.Clear();
            string returnContent = "";

            var serializedModel = JsonConvert.SerializeObject(model);
            

            if(!string.IsNullOrEmpty(encryptionKey))
            {
                var response = EncryptData(encryptionKey, serializedModel);
                var payload = JsonConvert.SerializeObject(new { client = response });
                returnContent = payload;
            }
            else
            {
                returnContent = serializedModel;
            }
            var currentUrl = Path.Combine(client.BaseAddress.ToString(), PartOfUrl);

            var content = new StringContent(returnContent, Encoding.UTF8, "application/json");
            return new Tuple<string, StringContent>(currentUrl, content);
        }

        private static async Task<Tuple<string, HttpResponseMessage>> PostContentWithAuthorizationAsync<T>(string _baseURL, T model, string PartOfUrl, string token, string encryptionKey)
        {
            var response = new HttpResponseMessage();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                var contents = BuildReqContent<T>(client, _baseURL, model, PartOfUrl,encryptionKey);
                response = await client.PostAsync(contents.Item1, contents.Item2);
            }
            var responseObj = await response.Content.ReadAsStringAsync();
            return new Tuple<string, HttpResponseMessage>(responseObj, response);
        }

        /// <summary>
        /// Encryption algorithm that allows posting of card details to flutterwave
        /// It is required that calls to flutterwave API be called using 3DE Encryption type
        /// </summary>
        /// <param name="encryptionKey"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private static string EncryptData(string encryptionKey, string data)
        {
            TripleDES des = new TripleDESCryptoServiceProvider
            {
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7,
                Key = Encoding.UTF8.GetBytes(encryptionKey),
            };
            ICryptoTransform cryptoTransform = des.CreateEncryptor();
            byte[] dataBytes = ASCIIEncoding.UTF8.GetBytes(data);
            byte[] encryptedDataBytes = cryptoTransform.TransformFinalBlock(dataBytes, 0, dataBytes.Length);
            des.Clear();
            return Convert.ToBase64String(encryptedDataBytes);
        }
    }
}

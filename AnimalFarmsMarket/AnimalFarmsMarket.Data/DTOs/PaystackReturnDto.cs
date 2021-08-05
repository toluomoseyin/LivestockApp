using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalFarmsMarket.Data.DTOs
{
    public class PaystackReturnDto
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public Data Data { get; set; }
    }

    public class Data
    {
        [JsonProperty("authorization_url")]
        public string AuthorizationUrl { get; set; }
        [JsonProperty("access_code")]
        public string AccessCode { get; set; }
        public string Reference { get; set; }

    }
}

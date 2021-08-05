using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalFarmsMarket.Data.ViewModels
{
    public class CardDetails
    {
        [JsonProperty("card_number")]
        public string cardNumber { get; set; }
        public string cvv { get; set; }
        [JsonProperty("expiry_month")]
        public string ExpirationMonth { get; set; }
        [JsonProperty("expiry_year")]
        public string ExpirationYear { get; set; }
        public string currency { get; set; }
        public string amount { get; set; }
        public string email { get; set; }
        public string fullname { get; set; }
        [JsonProperty("tx_ref")]
        public string tx_ref { get; set; }
        [JsonProperty("redirect_url")]
        public string redirect_url { get; set; }
        public Authorization authorization { get; set; }
    }

    public class Authorization
    {
        public string mode { get; set; }
        public string pin { get; set; }

    }
}

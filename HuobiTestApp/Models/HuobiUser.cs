using System.Text.Json.Serialization;

namespace HuobiTestApp.Models
{
    public class HuobiUser
    {
        [JsonPropertyName("account_name")]
        public object AccountName { get; set; }

        [JsonPropertyName("auth_country_id")]
        public long AuthCountryId { get; set; }

        [JsonPropertyName("country_code")]
        public string CountryCode { get; set; }

        [JsonPropertyName("country_id")]
        public long[] CountryId { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("fullname")]
        public string Fullname { get; set; }

        [JsonPropertyName("gmt_created")]
        public long GmtCreated { get; set; }

        [JsonPropertyName("kyc_level")]
        public string KycLevel { get; set; }

        [JsonPropertyName("kyc_type")]
        public object KycType { get; set; }

        [JsonPropertyName("lp_type")]
        public object LpType { get; set; }

        [JsonPropertyName("options")]
        public Options Options { get; set; }

        [JsonPropertyName("phone")]
        public string Phone { get; set; }

        [JsonPropertyName("reg_channel")]
        public object RegChannel { get; set; }

        [JsonPropertyName("register_ip_country_id")]
        public long RegisterIpCountryId { get; set; }

        [JsonPropertyName("state")]
        public long State { get; set; }

        [JsonPropertyName("uid")]
        public long Uid { get; set; }

        [JsonPropertyName("user_type")]
        public long UserType { get; set; }

        [JsonPropertyName("uuid")]
        public string Uuid { get; set; }

        [JsonPropertyName("vip_expired")]
        public bool VipExpired { get; set; }

        [JsonPropertyName("vip_expiry_date")]
        public object VipExpiryDate { get; set; }

        [JsonPropertyName("vip_level")]
        public long VipLevel { get; set; }
    }

    public class Options
    {
        [JsonPropertyName("conversion_currency")]
        public string ConversionCurrency { get; set; }
    }
}
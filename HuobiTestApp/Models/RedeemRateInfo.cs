using System.Text.Json.Serialization;

namespace HuobiTestApp.Models
{
    public class RedeemRateInfo
    {
        [JsonPropertyName("currRateInfo")]
        public object CurrRateInfo { get; set; }

        [JsonPropertyName("nextRateInfo")]
        public object NextRateInfo { get; set; }
    }
}
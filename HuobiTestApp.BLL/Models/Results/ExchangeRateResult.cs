using System.Text.Json.Serialization;

namespace HuobiTestApp.BLL.Models.Results
{
    public class ExchangeRateResult
    {
        [JsonPropertyName("data_time")]
        public long DataTime { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("rate")]
        public decimal Rate { get; set; }

        [JsonPropertyName("time")]
        public long Time { get; set; }
    }
}
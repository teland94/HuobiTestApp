using System.Text.Json.Serialization;

namespace HuobiTestApp.BLL.Models.Results
{
    public class HuobiResult<T>
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("data")]
        public T Data { get; set; }

        [JsonPropertyName("success")]
        public bool Success { get; set; }
    }
}

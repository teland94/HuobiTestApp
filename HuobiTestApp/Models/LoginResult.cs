using System.Text.Json.Serialization;

namespace HuobiTestApp.Models
{
    public class LoginResult
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }
    }
}
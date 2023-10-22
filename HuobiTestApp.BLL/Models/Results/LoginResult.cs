using System.Text.Json.Serialization;

namespace HuobiTestApp.BLL.Models.Results
{
    public class LoginResult
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }
    }
}
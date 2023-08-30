using System.Text.Json.Serialization;

namespace HuobiTestApp.Models
{
    public class TicketResult
    {
        [JsonPropertyName("ticket")]
        public string Ticket { get; set; }
    }
}
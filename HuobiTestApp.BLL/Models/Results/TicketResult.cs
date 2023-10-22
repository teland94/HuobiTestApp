using System.Text.Json.Serialization;

namespace HuobiTestApp.BLL.Models.Results
{
    public class TicketResult
    {
        [JsonPropertyName("ticket")]
        public string Ticket { get; set; }
    }
}
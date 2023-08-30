using System.Text.Json.Serialization;

namespace HuobiTestApp.Models
{
    public class BalanceProfitResult
    {
        [JsonPropertyName("updated")]
        public Updated Updated { get; set; }

        [JsonPropertyName("todayCoinProfit")]
        public decimal? TodayCoinProfit { get; set; }

        [JsonPropertyName("spotRiskLevel")]
        public object SpotRiskLevel { get; set; }

        [JsonPropertyName("spotRiskRate")]
        public object SpotRiskRate { get; set; }

        [JsonPropertyName("todayCoinProfitRate")]
        public decimal? TodayCoinProfitRate { get; set; }

        [JsonPropertyName("todayProfit")]
        public decimal? TodayProfit { get; set; }

        [JsonPropertyName("userId")]
        public long UserId { get; set; }

        [JsonPropertyName("totalAccumulateProfit")]
        public decimal? TotalAccumulateProfit { get; set; }

        [JsonPropertyName("totalBalanceUsdt")]
        public decimal TotalBalanceUsdt { get; set; }

        [JsonPropertyName("totalBalance")]
        public decimal TotalBalance { get; set; }

        [JsonPropertyName("accountState")]
        public string AccountState { get; set; }

        [JsonPropertyName("profitAccountBalanceList")]
        public ProfitAccountBalanceList[] ProfitAccountBalanceList { get; set; }

        [JsonPropertyName("todayProfitState")]
        public long? TodayProfitState { get; set; }

        [JsonPropertyName("todayProfitRate")]
        public decimal? TodayProfitRate { get; set; }
    }

    public class ProfitAccountBalanceList
    {
        [JsonPropertyName("spotBalanceState")]
        public long? SpotBalanceState { get; set; }

        [JsonPropertyName("distributionType")]
        public int DistributionType { get; set; }

        [JsonPropertyName("balance")]
        public double Balance { get; set; }

        [JsonPropertyName("accountBalanceUsdt")]
        public decimal AccountBalanceUsdt { get; set; }

        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("accountBalance")]
        public decimal AccountBalance { get; set; }
    }

    public class Updated
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("time")]
        public long Time { get; set; }
    }
}

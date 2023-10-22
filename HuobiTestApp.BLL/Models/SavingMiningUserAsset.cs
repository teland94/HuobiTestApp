namespace HuobiTestApp.BLL.Models
{
    public class SavingMiningUserAsset
    {
        public string Currency { get; set; }

        public ProjectType ProjectType { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal TotalAmountFiat { get; set; }

        public decimal YesterdayIncome { get; set; }

        public decimal YesterdayIncomeFiat { get; set; }
    }
}

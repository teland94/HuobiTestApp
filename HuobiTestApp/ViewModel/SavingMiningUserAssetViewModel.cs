using HuobiTestApp.Models;

namespace HuobiTestApp.ViewModel
{
    public class SavingMiningUserAssetViewModel
    {
        public string Currency { get; set; }

        public ProjectType ProjectType { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal TotalAmountFiat { get; set; }

        public decimal YesterdayIncome { get; set; }

        public decimal YesterdayIncomeFiat { get; set; }
    }
}

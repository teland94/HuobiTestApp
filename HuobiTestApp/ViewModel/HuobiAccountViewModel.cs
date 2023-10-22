using HuobiTestApp.BLL.Models;

namespace HuobiTestApp.ViewModel
{
    public class HuobiAccountViewModel
    {
        public string Name { get; set; }

        public decimal TotalBalance { get; set; }

        public decimal TodayProfit { get; set; }

        public IEnumerable<SavingMiningUserAsset> Assets { get; set; }

        public Guid SettingsId { get; set; }
    }
}
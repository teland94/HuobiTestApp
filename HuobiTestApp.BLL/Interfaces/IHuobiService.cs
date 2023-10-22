using Huobi.Net.Objects.Models;
using HuobiTestApp.BLL.Models;

namespace HuobiTestApp.BLL.Interfaces;

public interface IHuobiService
{
    Task Login(string ucToken);

    Task<decimal> GetFiatCurrencyRate(string currency);

    Task<HuobiUserData> GetHuobiUserData(decimal fiatCurrencyRate);

    Task<IEnumerable<SavingMiningUserAsset>> GetSavingMiningUserAssetData(decimal fiatCurrencyRate);

    Task<IEnumerable<HuobiOrder>> GetHuobiClosedOrders(string symbol, DateTime startTime, DateTime endTime);
}
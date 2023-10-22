using System.Net.Http.Json;
using Huobi.Net.Interfaces.Clients;
using Huobi.Net.Objects.Models;
using HuobiTestApp.BLL.Converters;
using HuobiTestApp.BLL.Exceptions;
using HuobiTestApp.BLL.Interfaces;
using HuobiTestApp.BLL.Models;
using HuobiTestApp.BLL.Models.Results;

namespace HuobiTestApp.BLL.Services
{
    public class HuobiService : IHuobiService
    {
        private HttpClient HttpClient { get; }

        private IHuobiRestClient HuobiRestClient { get; }

        public HuobiService(HttpClient httpClient,
            IHuobiRestClient huobiRestClient)
        {
            HttpClient = httpClient;
            HuobiRestClient = huobiRestClient;
        }

        public async Task Login(string ucToken)
        {
            HttpClient.DefaultRequestHeaders.Remove("HB-UC-TOKEN");
            HttpClient.DefaultRequestHeaders.Add("HB-UC-TOKEN", ucToken);

            var ticketResponse = await HttpClient.GetFromJsonAsync<HuobiResult<TicketResult>>("/-/x/uc/uc/open/ticket/get");

            if (!ticketResponse.Success)
            {
                throw new ApiException(ticketResponse.Message, ticketResponse.Code);
            }
            var loginResponse =
                await HttpClient.PostAsJsonAsync("/-/x/pro/v1/users/login", new { ticket = ticketResponse.Data.Ticket });

            loginResponse.EnsureSuccessStatusCode();

            var loginResult = await loginResponse.Content.ReadFromJsonAsync<HuobiResult<LoginResult>>();

            HttpClient.DefaultRequestHeaders.Remove("HB-PRO-TOKEN");
            HttpClient.DefaultRequestHeaders.Add("HB-PRO-TOKEN", loginResult.Data.Token);
        }

        public async Task<decimal> GetFiatCurrencyRate(string currency)
        {
            decimal fiatCurrencyRate = 1;

            if (currency != "USD")
            {
                var exchangeRateList = await HttpClient.GetFromJsonAsync<HuobiResult<IEnumerable<ExchangeRateResult>>>("/-/x/general/exchange_rate/list", Converter.Settings);

                var exchangeRateUsdCurrency = exchangeRateList.Data.FirstOrDefault(e => e.Name == $"usd_{currency.ToLower()}");

                fiatCurrencyRate = exchangeRateUsdCurrency?.Rate ?? 1;
            }

            return fiatCurrencyRate;
        }

        public async Task<HuobiUserData> GetHuobiUserData(decimal fiatCurrencyRate)
        {
            var userData = await HttpClient.GetFromJsonAsync<HuobiResult<HuobiUserResult>>("/-/x/uc/uc/open/user/get");

            var balanceProfit = await HttpClient.GetFromJsonAsync<HuobiResult<BalanceProfitResult>>("/-/x/hbg/v1/asset/profit/balance-profit-loss");

            var symbolData = await HuobiRestClient.SpotApi.ExchangeData.GetSymbolDetails24HAsync("btcusdt");

            if (!symbolData.Success)
            {
                throw new InvalidOperationException(symbolData.Error?.Code.ToString());
            }

            return new HuobiUserData
            {
                Name = userData.Data.Email,
                TotalBalance = balanceProfit.Data.TotalBalanceUsdt * fiatCurrencyRate,
                TodayProfit = balanceProfit.Data.TodayProfit != null
                    ? balanceProfit.Data.TodayProfit.Value * (symbolData.Data.ClosePrice ?? 0) * fiatCurrencyRate
                    : 0,
            };
        }

        public async Task<IEnumerable<SavingMiningUserAsset>> GetSavingMiningUserAssetData(decimal fiatCurrencyRate)
        {
            var projectTypes = Enum.GetValues<ProjectType>();

            var savingMiningUserAssetsResult = new List<SavingMiningUserAssetResult>();

            foreach (var projectType in projectTypes)
            {
                var earnProjectTypeResult = await HttpClient.GetFromJsonAsync<HuobiResult<IEnumerable<SavingMiningUserAssetResult>>>($"/-/x/hbg/v1/saving/mining/user/assets/list?projectType={(int)projectType}", Converter.Settings);

                savingMiningUserAssetsResult.AddRange(earnProjectTypeResult.Data);
            }

            var symbols = savingMiningUserAssetsResult
                .Where(a => a.Currency != "USDT")
                .Select(a => $"{a.Currency.ToLower()}usdt")
                .Distinct();

            var exchangePrices = await GetSymbolsExchangePrice(symbols);

            var assets = new List<SavingMiningUserAsset>();

            foreach (var savingMiningUserAsset in savingMiningUserAssetsResult)
            {
                decimal totalAmountFiat;
                decimal yesterdayIncomeFiat;

                var yesterdayIncome = savingMiningUserAsset.ProjectType switch
                {
                    ProjectType.Flexible => savingMiningUserAsset.YesterdayIncome,
                    ProjectType.Fixed => savingMiningUserAsset.EstFixedTodayInterest ?? 0,
                    _ => throw new ArgumentOutOfRangeException()
                };

                if (savingMiningUserAsset.Currency == "USDT")
                {
                    totalAmountFiat = savingMiningUserAsset.TotalAmount * fiatCurrencyRate;
                    yesterdayIncomeFiat = savingMiningUserAsset.YesterdayIncome * fiatCurrencyRate;
                }
                else
                {
                    var exchangePrice = exchangePrices[$"{savingMiningUserAsset.Currency.ToLower()}usdt"] ?? 0;

                    totalAmountFiat = savingMiningUserAsset.TotalAmount * exchangePrice * fiatCurrencyRate;
                    yesterdayIncomeFiat = yesterdayIncome * exchangePrice * fiatCurrencyRate;
                }

                assets.Add(new SavingMiningUserAsset
                {
                    Currency = savingMiningUserAsset.Currency,
                    ProjectType = savingMiningUserAsset.ProjectType,
                    TotalAmount = savingMiningUserAsset.TotalAmount,
                    TotalAmountFiat = totalAmountFiat,
                    YesterdayIncome = yesterdayIncome,
                    YesterdayIncomeFiat = yesterdayIncomeFiat
                });
            }

            return assets.OrderByDescending(a => a.TotalAmountFiat);
        }

        public async Task<IEnumerable<HuobiOrder>> GetHuobiClosedOrders(string symbol, DateTime startTime, DateTime endTime)
        {
            var startTimeRange = endTime.AddDays((endTime - startTime).Days > 1 ? -2 : -1);
            var endTimeRange = endTime;

            var allClosedOrders = new List<HuobiOrder>();

            while (startTime <= startTimeRange)
            {
                var closedOrders = await HuobiRestClient.SpotApi.Trading.GetClosedOrdersAsync(symbol,
                    startTime: startTimeRange, endTime: endTimeRange);

                allClosedOrders.AddRange(closedOrders.Data);

                startTimeRange = startTimeRange.AddDays((startTimeRange - startTime).Days > 1 ? -2 : -1);
                endTimeRange = endTimeRange.AddDays(-2);
            }

            return allClosedOrders;
        }

        private async Task<Dictionary<string, decimal?>> GetSymbolsExchangePrice(IEnumerable<string> symbols)
        {
            var exchangeData = new Dictionary<string, decimal?>();

            foreach (var symbol in symbols)
            {
                var symbolData = await HuobiRestClient.SpotApi.ExchangeData.GetSymbolDetails24HAsync(symbol);

                exchangeData.Add(symbol, symbolData.Data.ClosePrice);
            }

            return exchangeData;
        }
    }
}

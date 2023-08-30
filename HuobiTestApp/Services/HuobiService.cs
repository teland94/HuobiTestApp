using System.Net;
using System.Net.Http.Json;
using Huobi.Net.Interfaces.Clients;
using Huobi.Net.Objects.Models;
using HuobiTestApp.Converters;
using HuobiTestApp.Models;
using HuobiTestApp.ViewModel;

namespace HuobiTestApp.Services
{
    public interface IHuobiService
    {
        Task<IEnumerable<SavingMiningUserAssetViewModel>> GetSavingMiningUserAssetData(HttpClient httpClient,
            IHuobiRestClient huobiRestClient, decimal fiatCurrencyRate);

        HttpClient CreateHttpClient(string ssoToken);

        Task Login(HttpClient httpClient);
    }

    public class HuobiService : IHuobiService
    {
        public async Task<IEnumerable<SavingMiningUserAssetViewModel>> GetSavingMiningUserAssetData(HttpClient httpClient, IHuobiRestClient huobiRestClient, decimal fiatCurrencyRate)
        {
            //var orders = await GetHuobiClosedOrdersAsync(huobiRestClient, "usddusdt", new DateTime(2023, 8, 4), new DateTime(2023, 8, 12));

            var projectTypes = Enum.GetValues<ProjectType>();

            var savingMiningUserAssetsResult = new List<SavingMiningUserAsset>();

            foreach (var projectType in projectTypes)
            {
                var earnProjectTypeResult = await httpClient.GetFromJsonAsync<HuobiResult<IEnumerable<SavingMiningUserAsset>>>($"https://www.huobi.com/-/x/hbg/v1/saving/mining/user/assets/list?projectType={(int)projectType}", Converter.Settings);

                savingMiningUserAssetsResult.AddRange(earnProjectTypeResult.Data);
            }

            var symbols = savingMiningUserAssetsResult
                .Where(a => a.Currency != "USDT")
                .Select(a => $"{a.Currency.ToLower()}usdt")
                .Distinct();

            var exchangePrices = await GetSymbolsExchangePrice(huobiRestClient, symbols);

            var assets = new List<SavingMiningUserAssetViewModel>();

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

                assets.Add(new SavingMiningUserAssetViewModel
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

        public async Task Login(HttpClient httpClient)
        {
            var ticketResponse = await httpClient.GetFromJsonAsync<HuobiResult<TicketResult>>("https://www.huobi.com/-/x/uc/uc/open/ticket/get");

            if (!ticketResponse.Success)
            {
                throw new InvalidOperationException(ticketResponse.Message);
            }
            var loginResponse =
                await httpClient.PostAsJsonAsync("https://www.huobi.com/-/x/pro/v1/users/login", new { ticket = ticketResponse.Data.Ticket });

            loginResponse.EnsureSuccessStatusCode();

            var loginResult = await loginResponse.Content.ReadFromJsonAsync<HuobiResult<LoginResult>>();

            httpClient.DefaultRequestHeaders.Add("Hb-Pro-Token", loginResult.Data.Token);
        }

        public HttpClient CreateHttpClient(string ssoToken)
        {
            var cookieContainer = new CookieContainer();

            cookieContainer.Add(new Cookie("HB_SSO", ssoToken, "/", "huobi.com"));

            var httpClient = new HttpClient(new HttpClientHandler
            {
                CookieContainer = cookieContainer
            })
            {
                Timeout = TimeSpan.FromSeconds(60)
            };

            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.0.0 YaBrowser/23.7.2.765 Yowser/2.5 Safari/537.36");

            return httpClient;
        }

        private static async Task<Dictionary<string, decimal?>> GetSymbolsExchangePrice(IHuobiRestClient huobiRestClient, IEnumerable<string> symbols)
        {
            var exchangeData = new Dictionary<string, decimal?>();

            foreach (var symbol in symbols)
            {
                var symbolData = await huobiRestClient.SpotApi.ExchangeData.GetSymbolDetails24HAsync(symbol);

                exchangeData.Add(symbol, symbolData.Data.ClosePrice);
            }

            return exchangeData;
        }

        private async Task<IEnumerable<HuobiOrder>> GetHuobiClosedOrdersAsync(IHuobiRestClient huobiRestClient, string symbol, DateTime startTime, DateTime endTime)
        {
            var startTimeRange = endTime.AddDays((endTime - startTime).Days > 1 ? -2 : -1);
            var endTimeRange = endTime;

            var allClosedOrders = new List<HuobiOrder>();

            while (startTime <= startTimeRange)
            {
                var closedOrders = await huobiRestClient.SpotApi.Trading.GetClosedOrdersAsync(symbol,
                    startTime: startTimeRange, endTime: endTimeRange);

                allClosedOrders.AddRange(closedOrders.Data);

                startTimeRange = startTimeRange.AddDays((startTimeRange - startTime).Days > 1 ? -2 : -1);
                endTimeRange = endTimeRange.AddDays(-2);
            }

            return allClosedOrders;
        }

    }
}

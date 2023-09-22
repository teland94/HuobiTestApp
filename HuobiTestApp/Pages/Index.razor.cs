using System.Net.Http.Json;
using CryptoExchange.Net.Authentication;
using Huobi.Net.Interfaces.Clients;
using HuobiTestApp.Base;
using HuobiTestApp.Configuration;
using HuobiTestApp.Helpers;
using HuobiTestApp.Models;
using Microsoft.AspNetCore.Components;
using HuobiTestApp.ViewModel;
using Microsoft.Extensions.Options;
using MudBlazor;
using HuobiUser = HuobiTestApp.Models.HuobiUser;
using HuobiTestApp.Converters;
using HuobiTestApp.Dialogs;
using HuobiTestApp.Services;

namespace HuobiTestApp.Pages
{
    public partial class Index : RefreshablePageBase
    {
        public bool Loading { get; set; }

        public string SelectedCurrency { get; set; } = "USD";
        public string CurrencyFormat { get; set; }
        public string CurrencyProfitFormat { get; set; }

        public decimal TotalBalance { get; set; }
        public decimal TodayProfit { get; set; }
        public List<HuobiAccountViewModel> Accounts { get; set; } = new();

        [Inject] public IHuobiRestClient HuobiRestClient { get; set; }

        [Inject] public ISnackbar Snackbar { get; set; }

        [Inject] public IDialogService DialogService { get; set; }

        [Inject] public IOptions<AppSettings> AppSettings { get; set; }

        [Inject] public IHuobiService HuobiService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await Refresh();
        }

        public async Task SelectedCurrencyChanged()
        {
            await Refresh();
        }

        public async Task SymbolClicked(TableRowClickEventArgs<SavingMiningUserAssetViewModel> tableRowClickEventArgs, Guid accountSettingsId)
        {
            var symbol = tableRowClickEventArgs.Item;

            if (symbol.Currency == "USDT")
            {
                return;
            }

            var accountSettings = AppSettings.Value.Accounts.FirstOrDefault(a => a.Id == accountSettingsId)!;

            HuobiRestClient.SetApiCredentials(new ApiCredentials(accountSettings.ApiKey, accountSettings.ApiSecret));

            await DialogService.ShowAsync<SymbolDetailsDialog>("Symbol Details", new DialogParameters<SymbolDetailsDialog>
            {
                { p => p.Currency, symbol.Currency },
                { p => p.CurrencyFormat, CurrencyFormat }
            });
        }

        public async Task Refresh()
        {
            try
            {
                Loading = true;

                TotalBalance = 0;
                TodayProfit = 0;
                Accounts.Clear();

                var currencySymbol = CurrencyTools.GetCurrencySymbol(SelectedCurrency);

                CurrencyFormat = Constants.CurrencyInvariantFormat.Replace("¤", currencySymbol);
                CurrencyProfitFormat = Constants.CurrencyProfitInvariantFormat.Replace("¤", currencySymbol);

                foreach (var accountSettings in AppSettings.Value.Accounts)
                {
                    await ProcessAccount(accountSettings);
                }

                foreach (var account in Accounts)
                {
                    TotalBalance += account.TotalBalance;
                    TodayProfit += account.TodayProfit;
                }
            }
            catch (Exception e)
            {
                Snackbar.Add(e.Message, Severity.Error);
            }
            finally
            {
                Loading = false;
            }
        }

        private async Task ProcessAccount(HuobiAccountSettings accountSettings)
        {
            var httpClient = HuobiService.CreateHttpClient(accountSettings.SsoToken, AppSettings.Value.UserAgent);

            HuobiRestClient.SetApiCredentials(new ApiCredentials(accountSettings.ApiKey, accountSettings.ApiSecret));

            await HuobiService.Login(httpClient);

            var balanceProfit = await httpClient.GetFromJsonAsync<HuobiResult<BalanceProfitResult>>("https://www.huobi.com/-/x/hbg/v1/asset/profit/balance-profit-loss");

            var symbolData = await HuobiRestClient.SpotApi.ExchangeData.GetSymbolDetails24HAsync("btcusdt");

            if (!symbolData.Success)
            {
                throw new InvalidOperationException(symbolData.Error?.Code.ToString());
            }

            var userData = await httpClient.GetFromJsonAsync<HuobiResult<HuobiUser>>("https://www.huobi.com/-/x/uc/uc/open/user/get");

            decimal fiatCurrencyRate = 1;

            if (SelectedCurrency != "USD")
            {
                var exchangeRateList = await httpClient.GetFromJsonAsync<HuobiResult<IEnumerable<ExchangeRateResult>>>("https://www.huobi.com/-/x/general/exchange_rate/list", Converter.Settings);

                var exchangeRateUsdCurrency = exchangeRateList.Data.FirstOrDefault(e => e.Name == $"usd_{SelectedCurrency.ToLower()}");

                fiatCurrencyRate = exchangeRateUsdCurrency?.Rate ?? 1;
            }

            var assets = (await HuobiService.GetSavingMiningUserAssetData(httpClient, HuobiRestClient, fiatCurrencyRate)).ToList();

            Accounts.Add(new HuobiAccountViewModel
            {
                Name = userData.Data.Email,
                TotalBalance = balanceProfit.Data.TotalBalanceUsdt * fiatCurrencyRate,
                TodayProfit = balanceProfit.Data.TodayProfit != null ? balanceProfit.Data.TodayProfit.Value * (symbolData.Data.ClosePrice ?? 0) * fiatCurrencyRate : 0,
                Assets = assets,
                SettingsId = accountSettings.Id
            });
        }
    }
}

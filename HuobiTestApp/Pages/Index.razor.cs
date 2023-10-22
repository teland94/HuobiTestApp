using CryptoExchange.Net.Authentication;
using Huobi.Net.Interfaces.Clients;
using HuobiTestApp.Base;
using HuobiTestApp.BLL.Configuration;
using HuobiTestApp.BLL.Helpers;
using HuobiTestApp.BLL.Models;
using Microsoft.AspNetCore.Components;
using HuobiTestApp.ViewModel;
using Microsoft.Extensions.Options;
using MudBlazor;
using HuobiTestApp.Dialogs;
using HuobiTestApp.BLL.Interfaces;

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

        [Inject] public IHttpClientFactory HttpClientFactory { get; set; }

        [Inject] public IHuobiService HuobiService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await Refresh();
        }

        public async Task SelectedCurrencyChanged()
        {
            await Refresh();
        }

        public async Task SymbolClicked(TableRowClickEventArgs<SavingMiningUserAsset> tableRowClickEventArgs, Guid accountSettingsId)
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
            HuobiRestClient.SetApiCredentials(new ApiCredentials(accountSettings.ApiKey, accountSettings.ApiSecret));

            await HuobiService.Login(accountSettings.SsoToken);

            var fiatCurrencyRate = await HuobiService.GetFiatCurrencyRate(SelectedCurrency);

            var userData = await HuobiService.GetHuobiUserData(fiatCurrencyRate);

            var assets = (await HuobiService.GetSavingMiningUserAssetData(fiatCurrencyRate)).ToList();

            Accounts.Add(new HuobiAccountViewModel
            {
                Name = userData.Name,
                TotalBalance = userData.TotalBalance,
                TodayProfit = userData.TodayProfit,
                Assets = assets,
                SettingsId = accountSettings.Id
            });
        }
    }
}

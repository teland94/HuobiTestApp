using System.Text.Json.Serialization;

namespace HuobiTestApp.BLL.Models.Results
{
    public class SavingMiningUserAssetResult
    {
        [JsonPropertyName("interestToGet")]
        public object InterestToGet { get; set; }

        [JsonPropertyName("btnStatus")]
        public object BtnStatus { get; set; }

        [JsonPropertyName("projectId")]
        public long ProjectId { get; set; }

        [JsonPropertyName("orderId")]
        public long OrderId { get; set; }

        [JsonPropertyName("projectName")]
        public string ProjectName { get; set; }

        [JsonPropertyName("projectType")]
        public ProjectType ProjectType { get; set; }

        [JsonPropertyName("shelfType")]
        public long ShelfType { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("miningAmount")]
        public string MiningAmount { get; set; }

        [JsonPropertyName("preMiningAmount")]
        public string PreMiningAmount { get; set; }

        [JsonPropertyName("yesterTotalRate")]
        public string YesterTotalRate { get; set; }

        [JsonPropertyName("miningYearRate")]
        public string MiningYearRate { get; set; }

        [JsonPropertyName("yesterdayIncome")]
        public decimal YesterdayIncome { get; set; }

        [JsonPropertyName("couponStatus")]
        public long CouponStatus { get; set; }

        [JsonPropertyName("couponRate")]
        public string CouponRate { get; set; }

        [JsonPropertyName("effectTime")]
        public long EffectTime { get; set; }

        [JsonPropertyName("incomeTime")]
        public long IncomeTime { get; set; }

        [JsonPropertyName("totalIncomeAmount")]
        public string TotalIncomeAmount { get; set; }

        [JsonPropertyName("totalAmount")]
        public decimal TotalAmount { get; set; }

        [JsonPropertyName("allowMiningStatus")]
        public long AllowMiningStatus { get; set; }

        [JsonPropertyName("allowRedemptionStatus")]
        public long AllowRedemptionStatus { get; set; }

        [JsonPropertyName("term")]
        public long Term { get; set; }

        [JsonPropertyName("miningStartTime")]
        public long MiningStartTime { get; set; }

        [JsonPropertyName("miningEndTime")]
        public long MiningEndTime { get; set; }

        [JsonPropertyName("proIncomeAmount")]
        public string ProIncomeAmount { get; set; }

        [JsonPropertyName("fixedToActiveAutoStatus")]
        public long FixedToActiveAutoStatus { get; set; }

        [JsonPropertyName("balanceAutoStatus")]
        public long BalanceAutoStatus { get; set; }

        [JsonPropertyName("balanceAutoTime")]
        public DateTimeOffset BalanceAutoTime { get; set; }

        [JsonPropertyName("redeemLimitStartTime")]
        public DateTimeOffset RedeemLimitStartTime { get; set; }

        [JsonPropertyName("redeemLimitEndTime")]
        public DateTimeOffset RedeemLimitEndTime { get; set; }

        [JsonPropertyName("redeemAmount")]
        public string RedeemAmount { get; set; }

        [JsonPropertyName("isCouponFullAmount")]
        public object IsCouponFullAmount { get; set; }

        [JsonPropertyName("isCouponFullTime")]
        public object IsCouponFullTime { get; set; }

        [JsonPropertyName("couponMaxAmount")]
        public object CouponMaxAmount { get; set; }

        [JsonPropertyName("couponValidDaysCount")]
        public object CouponValidDaysCount { get; set; }

        [JsonPropertyName("isRedeemOrder")]
        public bool IsRedeemOrder { get; set; }

        [JsonPropertyName("estFixedTodayInterest")]
        public decimal? EstFixedTodayInterest { get; set; }

        [JsonPropertyName("confirmedFixedTotalInterest")]
        public decimal? ConfirmedFixedTotalInterest { get; set; }

        [JsonPropertyName("translateAutoInvest")]
        public string TranslateAutoInvest { get; set; }

        [JsonPropertyName("translateFixed")]
        public string TranslateFixed { get; set; }

        [JsonPropertyName("translateFlexible")]
        public string TranslateFlexible { get; set; }

        [JsonPropertyName("translateToken")]
        public string TranslateToken { get; set; }

        [JsonPropertyName("translateMv")]
        public string TranslateMv { get; set; }

        [JsonPropertyName("translateQty")]
        public string TranslateQty { get; set; }

        [JsonPropertyName("translateYesterdayInterests")]
        public string TranslateYesterdayInterests { get; set; }

        [JsonPropertyName("translateTotalInterests")]
        public string TranslateTotalInterests { get; set; }

        [JsonPropertyName("translateEstEarning")]
        public string TranslateEstEarning { get; set; }

        [JsonPropertyName("translateConfirmedInterest")]
        public string TranslateConfirmedInterest { get; set; }

        [JsonPropertyName("translateEstTodayInterest")]
        public string TranslateEstTodayInterest { get; set; }

        [JsonPropertyName("translateEstTotalInterest")]
        public string TranslateEstTotalInterest { get; set; }

        [JsonPropertyName("isStEth")]
        public bool IsStEth { get; set; }

        [JsonPropertyName("orderShowLabelType")]
        public long OrderShowLabelType { get; set; }

        [JsonPropertyName("translateLabelShowType")]
        public object TranslateLabelShowType { get; set; }

        [JsonPropertyName("isPeProject")]
        public bool IsPeProject { get; set; }

        [JsonPropertyName("isSupportYbb")]
        public bool IsSupportYbb { get; set; }

        [JsonPropertyName("isShowFix2Active")]
        public bool IsShowFix2Active { get; set; }

        [JsonPropertyName("projectInfoUrl")]
        public string ProjectInfoUrl { get; set; }

        [JsonPropertyName("receiptDay")]
        public long ReceiptDay { get; set; }

        [JsonPropertyName("redeemRateInfo")]
        public RedeemRateInfo RedeemRateInfo { get; set; }

        [JsonPropertyName("translateSharkLabel")]
        public object TranslateSharkLabel { get; set; }

        [JsonPropertyName("subscribeUrl")]
        public object SubscribeUrl { get; set; }

        [JsonPropertyName("status")]
        public object Status { get; set; }

        [JsonPropertyName("callPutFlag")]
        public object CallPutFlag { get; set; }

        [JsonPropertyName("baseCurrency")]
        public object BaseCurrency { get; set; }

        [JsonPropertyName("minYearRate")]
        public object MinYearRate { get; set; }

        [JsonPropertyName("maxYearRate")]
        public object MaxYearRate { get; set; }
    }

    public class RedeemRateInfo
    {
        [JsonPropertyName("currRateInfo")]
        public object CurrRateInfo { get; set; }

        [JsonPropertyName("nextRateInfo")]
        public object NextRateInfo { get; set; }
    }
}
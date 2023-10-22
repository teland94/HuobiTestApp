using System.Globalization;

namespace HuobiTestApp.BLL.Helpers
{
    public static class CurrencyTools
    {
        private static readonly IDictionary<string, string> Map;

        static CurrencyTools()
        {
            Map = CultureInfo
                .GetCultures(CultureTypes.AllCultures)
                .Where(c => !c.IsNeutralCulture)
                .Select(culture => {
                    try
                    {
                        return new RegionInfo(culture.Name);
                    }
                    catch
                    {
                        return null;
                    }
                })
                .Where(ri => ri != null)
                .GroupBy(ri => ri.ISOCurrencySymbol)
                .ToDictionary(x => x.Key, x => x.First().CurrencySymbol);
            
            Map["RUB"] = "₽";
            Map["UAH"] = "₴";
        }

        public static string GetCurrencySymbol(string isoCurrencySymbol)
        {
            return Map[isoCurrencySymbol];
        }
    }
}
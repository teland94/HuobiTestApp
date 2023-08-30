using System.Text.Json;
using System.Text.Json.Serialization;

namespace HuobiTestApp.Converters
{
    internal static class Converter
    {
        public static readonly JsonSerializerOptions Settings = new(JsonSerializerDefaults.General)
        {
            Converters =
            {
                new DateOnlyConverter(),
                new TimeOnlyConverter(),
                IsoDateTimeOffsetConverter.Singleton
            },
            NumberHandling = JsonNumberHandling.AllowReadingFromString
        };
    }
}
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using CsvHelper;

namespace ProductServiceAPI.CustomConverters
{
    public class CustomVatRateConverter : DefaultTypeConverter
    {
        public override object? ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (string.IsNullOrWhiteSpace(text) || text == "O")
            {
                return null;
            }

            if (decimal.TryParse(text, out decimal result))
            {
                return result;
            }

            return base.ConvertFromString(text, row, memberMapData);
        }
    }
}

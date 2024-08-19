using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace Infrastructure.Converters;

internal sealed class DateConverter : DefaultTypeConverter
{
    private readonly string[] _formats = new[]
    {
        "dd/MM/yyyy", "d/M/yyyy", "M/d/yyyy" // Add more formats if needed
    };

    public override object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
    {
        return DateOnly.TryParseExact(text, _formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date)
            ? date
            : base.ConvertFromString(text, row, memberMapData);
    }
}
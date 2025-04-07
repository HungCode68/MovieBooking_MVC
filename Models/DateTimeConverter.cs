using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MovieBookingMVC.Models
{
    public class DateTimeConverter : JsonConverter<DateTime>
    {
        private readonly string[] formats =
        {
            "yyyy-MM-ddTHH:mm:ss",
            "yyyy-MM-dd",
            "MM/dd/yyyy HH:mm:ss",
            "HH:mm"  // Hỗ trợ định dạng giờ:phút
        };

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string value = reader.GetString();
            if (string.IsNullOrEmpty(value))
            {
                throw new JsonException("DateTime value is null or empty.");
            }

            // Nếu chỉ có giờ và phút (HH:mm), giả định ngày hiện tại
            if (TimeSpan.TryParseExact(value, "hh\\:mm", CultureInfo.InvariantCulture, out TimeSpan time))
            {
                return DateTime.Today.Add(time); // Gán ngày hiện tại
            }

            // Nếu là DateTime đầy đủ, parse bình thường
            if (DateTime.TryParseExact(value, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
            {
                return date;
            }

            throw new JsonException($"Invalid date format: {value}");
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("yyyy-MM-ddTHH:mm:ss"));
        }
    }
}
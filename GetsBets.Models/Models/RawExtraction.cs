using System.Text.Json.Serialization;

namespace GetsBets.Models
{
    public class RawExtraction
    {
        [JsonPropertyName("data_extragere")]
        public DateOnly ExtractionDate { get; set; }
        [JsonPropertyName("ora_extragere")]
        public TimeOnly ExtractionTime { get; set; }
        [JsonPropertyName("numere")]
        public string Numbers { get; set; }
        [JsonPropertyName("bonus")]
        public string Bonus { get; set; }
    }
}

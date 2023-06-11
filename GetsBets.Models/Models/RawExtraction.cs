using System.Text.Json.Serialization;

namespace GetsBets.Models
{
    public class RawExtraction
    {
        [JsonPropertyName("data_extragere")]
        public string Data_Extragere { get; set; }
        [JsonPropertyName("ora_extragere")]
        public string Ora_Extragere { get; set; }
        [JsonPropertyName("numere")]
        public string Numere { get; set; }
        [JsonPropertyName("bonus")]
        public string Bonus { get; set; }
    }
}

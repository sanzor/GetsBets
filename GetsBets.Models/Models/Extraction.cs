using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GetsBets.Models
{
    public class Extraction
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }
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

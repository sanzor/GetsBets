using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
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

        public static bool operator ==(Extraction left, Extraction right)
        {
            if ((object)left == null) return (object)right==null;
            return left.Equals(right);
        }
        public static bool operator !=(Extraction left, Extraction right)
        {
            return !(left == right);
        }
        public override bool Equals(object? obj)
        {
            if(obj==null || obj.GetType() != GetType())
            {
                return false;
            }
            var ext2 = (Extraction)obj;
            return (ext2.ExtractionTime == this.ExtractionTime &&
                ext2.ExtractionDate == this.ExtractionDate &&
                ext2.Numbers == this.Numbers &&
                ext2.Bonus == this.Bonus);
        }
        public override int GetHashCode()
        {
            return this.ExtractionDate.GetHashCode() ^ 
                this.ExtractionTime.GetHashCode() ^
                this.Numbers.GetHashCode() ^
                this.Bonus.GetHashCode();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GetsBets.Models
{
    public class GetExtractionsForDateDto
    {
        [JsonPropertyName("date_filter")]
        public DateOnly DateFilter { get; set; }
    }
}

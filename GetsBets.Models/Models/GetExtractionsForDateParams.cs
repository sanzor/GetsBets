using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GetsBets.Models
{
    [Serializable]
    public class GetExtractionsForDateParams
    {
       
        public DateOnly DateFilter { get; set; }
    }
}

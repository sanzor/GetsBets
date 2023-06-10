using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetsBets.Models
{
    public class GetTopExtractedNumbersParams
    {
        public DateOnly Date { get; set; }
        public ushort TopLeastExtractedNumbersCount { get; set; }
        public ushort TopMostExtractedNumbersCount { get; set; }
    }
}

using GetsBets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetsBets.Services
{
    public class CalculateTopExtractedNumbersParams
    {
        public ushort TopMostExtractedNumbersCount { get; set; }
        public ushort TopLeastExtractedNumbersCount { get; set; }
        public IEnumerable<Extraction> Extractions { get; set; }
    }
}

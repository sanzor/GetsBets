using GetsBets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetsBets.Services
{
    public interface IExtractionAdapter
    {
        public Either<Error, List<Extraction>> ParseExtractions(IEnumerable<RawExtraction> extractions);
        public Either<Error, List<AggregatedExtraction>> ParseAggregatedExtractions(IEnumerable<Extraction> extractions);
    }
}

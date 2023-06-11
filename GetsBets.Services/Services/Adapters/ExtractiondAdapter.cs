using GetsBets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetsBets.Services
{
    public class ExtractiondAdapter : IExtractionAdapter
    {
        public Either<Error, List<AggregatedExtraction>> ParseAggregatedExtractions(IEnumerable<Extraction> extractions)
        {
            var result = Try(() =>
            {
                List<AggregatedExtraction> ls = new List<AggregatedExtraction>();
                foreach (var rawExtraction in extractions)
                {
                    var extraction = ParseExtraction(rawExtraction);
                    ls.Add(extraction);
                }
                return ls;
            }).ToEither(exc => Error.New(exc));
            return result;
        }

        public Either<Error, List<Extraction>> ParseExtractions(IEnumerable<RawExtraction> extractions)
        {
            var result = Try(() =>
              {
                  List<Extraction> ls = new List<Extraction>();
                  foreach(var rawExtraction in extractions)
                  {
                      var extraction=ParseExtraction(rawExtraction);
                      ls.Add(extraction);
                  }
                  return ls;
              }).ToEither(exc=>Error.New(exc));
            return result;
        }
        private Extraction ParseExtraction(RawExtraction extraction)
        {
            return new Extraction
            {
                ExtractionDate =DateOnly.Parse(extraction.Data_Extragere),
                ExtractionTime =TimeOnly.Parse(extraction.Ora_Extragere),
                Bonus = extraction.Bonus,
                Numbers = extraction.Numere,
            };
        }
        private AggregatedExtraction ParseExtraction(Extraction extraction)
        {
            int year = extraction.ExtractionDate.Year;
            int month = extraction.ExtractionDate.Month;
            int day = extraction.ExtractionDate.Day;
            int hour = extraction.ExtractionTime.Hour;
            int minute = extraction.ExtractionTime.Minute;
            int second = extraction.ExtractionTime.Second;
            int milisecond = extraction.ExtractionTime.Millisecond;
            var date = new DateTime(year, month, day, hour, minute, second, milisecond);
            return new AggregatedExtraction
            {
                Date = $"{date.Year}-{date.Month}-{date.Day} {date.Hour}:{date.Minute}:{date.Second}",
                Numbers =extraction.Numbers
               
            };
        }
      
    }
}

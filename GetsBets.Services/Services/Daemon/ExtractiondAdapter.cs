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
            int year = extraction.ExtractionDate.Year;
            int month = extraction.ExtractionDate.Month;
            int day = extraction.ExtractionDate.Day;
            int hour = extraction.ExtractionTime.Hour;
            int minute = extraction.ExtractionTime.Minute;
            int second = extraction.ExtractionTime.Second;
            int milisecond = extraction.ExtractionTime.Millisecond;
            return new Extraction
            {
                ExtractionTime = new DateTime(year, month, day, hour, minute, second, milisecond)
            };
        }
      
    }
}

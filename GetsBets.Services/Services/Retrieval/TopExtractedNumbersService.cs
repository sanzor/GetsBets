using GetsBets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetsBets.Services
{
    internal class TopExtractedNumbersService:ITopExtractedNumbersService
    {
        public Either<Error,GetTopExtractedNumbersResult> CalculateTopExtractedNumbers(CalculateTopExtractedNumbersParams @params)
        {
            var rez=Try(() =>
            {
                var map = CreateFrequencyMap(@params.Extractions);
                var topCount = (from elem in map
                                orderby elem.Value
                                descending
                                select elem).Take(@params.TopMostExtractedNumbersCount)
                               ;
                var leastCount = (from elem in map
                                  orderby elem.Value
                                  ascending
                                  select elem).Take(@params.TopLeastExtractedNumbersCount);

                return new GetTopExtractedNumbersResult
                {
                    TopMostExtractedNumbers = topCount,
                    TopLeastExtractedNumbers = leastCount,
                    TopMostCount = @params.TopMostExtractedNumbersCount,
                    TopLeastCount = @params.TopLeastExtractedNumbersCount
                };
            }).ToEither(exc=>Error.New(exc));
            return rez;
            
        }
        private Dictionary<string,int> CreateFrequencyMap(IEnumerable<Extraction> extractions)
        {
            Dictionary<string, int> dict = new Dictionary<string, int>();
            var numbers = extractions.SelectMany(x => x.Numbers.Split(','));
            foreach (var item in numbers)
            {
                dict = AddOrUpdate(dict, item);
            }
            return dict;
        }
       
        private Dictionary<string,int> AddOrUpdate(Dictionary<string,int> dict, string key)
        {
            if(!dict.TryGetValue(key,out var value))
            {
                dict[key] = 1;
            }
            else
            {
                dict[key]++;
            }
            return dict;
        }
    }
}

using GetsBets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetsBets
{
    public class Adapter
    {
        public static Either<Error,GetExtractionsForDateParams> Adapt(GetExtractionsForDateDto dto)
        {
            var rez=Try(() =>
            {
                return new GetExtractionsForDateParams
                {
                    DateFilter = dto.DateFilter
                };
            }).ToEither(exc => Error.New(exc));
            return rez;
        }
        public static Either<Error, GetTopExtractedNumbersParams> Adapt(GetTopExtractedNumbersDto dto)
        {
            var rez = Try(() =>
            {
                return new GetTopExtractedNumbersParams
                {
                    Date=dto.Date,
                    TopLeastExtractedNumbersCount=dto.TopLeastExtractedNumbersCount,
                    TopMostExtractedNumbersCount=dto.TopMostExtractedNumbersCount
                };
            }).ToEither(exc => Error.New(exc));
            return rez;
        }
    }
}

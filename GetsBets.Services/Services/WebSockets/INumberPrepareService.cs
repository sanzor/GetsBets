using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetsBets.Services
{
    internal interface INumberPrepareService
    {
        Either<Error, List<SendNumber>> PrepareRandomizedNumbers(string numbers);
    }
}

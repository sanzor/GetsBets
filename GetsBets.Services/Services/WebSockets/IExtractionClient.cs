using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetsBets.Services
{
    public interface IExtractionClient
    {
        EitherAsync<Error, Unit> RunAsync();
    }
}

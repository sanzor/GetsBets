
using GetsBets.Models;
using LanguageExt;
using LanguageExt.Common;

namespace GetsBets.DataAccess.Contracts
{
    public interface IExtractionRepository
    {
        EitherAsync<Error, Unit> InsertExtractionAsync(Extraction extraction);

    }
}

using LanguageExt;
using LanguageExt.Common;

namespace GetsBets.Services
{
    public interface IExtractionDaemonService
    {
        EitherAsync<Error, Unit> InsertWinnersAsync();
    }
}

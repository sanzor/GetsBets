using GetsBets.Models;

namespace GetsBets.Services
{
    public interface IFetchExtractionService
    {
        EitherAsync<Error, List<RawExtraction>> FetchExtractionsService();
    }
}
using FizzSeven.Core.FizzBuzz;

namespace FizzSeven.Api.Features.Statistics;

public interface IFizzBuzzStatisticsService
{
    Task RecordRequestAsync(FizzBuzzRequest request, CancellationToken cancellationToken);

    Task<MostUsedFizzBuzzRequestResponse?> GetMostFrequentRequestAsync(CancellationToken cancellationToken);
}

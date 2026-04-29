using FizzSeven.Core.FizzBuzz;

namespace FizzSeven.Api.Features.Statistics;

public sealed class InMemoryFizzBuzzStatisticsService : IFizzBuzzStatisticsService
{
    private readonly object _syncRoot = new();
    private readonly Dictionary<string, long> _ranking = new(StringComparer.Ordinal);

    public Task RecordRequestAsync(FizzBuzzRequest request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var fingerprint = FizzBuzzRequestFingerprint.Create(request);

        lock (_syncRoot)
        {
            _ranking[fingerprint] = _ranking.GetValueOrDefault(fingerprint) + 1;
        }

        return Task.CompletedTask;
    }

    public Task<MostUsedFizzBuzzRequestResponse?> GetMostFrequentRequestAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        KeyValuePair<string, long>? winner = null;

        lock (_syncRoot)
        {
            foreach (var entry in _ranking)
            {
                if (winner is null || entry.Value > winner.Value.Value)
                {
                    winner = entry;
                }
            }
        }

        if (winner is null)
        {
            return Task.FromResult<MostUsedFizzBuzzRequestResponse?>(null);
        }

        if (!FizzBuzzRequestFingerprint.TryParse(winner.Value.Key, out var request) || request is null)
        {
            throw new InvalidOperationException($"Stored statistics fingerprint '{winner.Value.Key}' could not be parsed.");
        }

        return Task.FromResult<MostUsedFizzBuzzRequestResponse?>(
            new MostUsedFizzBuzzRequestResponse(
                request.Int1,
                request.Int2,
                request.Limit,
                request.Str1,
                request.Str2,
                winner.Value.Value));
    }
}

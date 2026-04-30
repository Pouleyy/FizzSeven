using FizzSeven.Core.FizzBuzz;

namespace FizzSeven.Api.Features.Statistics;

public sealed class InMemoryFizzBuzzStatisticsService : IFizzBuzzStatisticsService
{
    private readonly SemaphoreSlim _gate = new(1, 1);
    private readonly Dictionary<string, long> _ranking = new(StringComparer.Ordinal);

    public async Task RecordRequestAsync(FizzBuzzRequest request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var fingerprint = FizzBuzzRequestFingerprint.Create(request);

        await _gate.WaitAsync(cancellationToken);

        try
        {
            _ranking[fingerprint] = _ranking.GetValueOrDefault(fingerprint) + 1;
        }
        finally
        {
            _gate.Release();
        }
    }

    public async Task<MostUsedFizzBuzzRequestResponse?> GetMostFrequentRequestAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        KeyValuePair<string, long>? winner = null;

        await _gate.WaitAsync(cancellationToken);

        try
        {
            foreach (var entry in _ranking)
            {
                if (winner is null || entry.Value > winner.Value.Value)
                {
                    winner = entry;
                }
            }
        }
        finally
        {
            _gate.Release();
        }

        if (winner is null)
        {
            return null;
        }

        if (!FizzBuzzRequestFingerprint.TryParse(winner.Value.Key, out var request) || request is null)
        {
            throw new InvalidOperationException($"Stored statistics fingerprint '{winner.Value.Key}' could not be parsed.");
        }

        return
            new MostUsedFizzBuzzRequestResponse(
                request.Int1,
                request.Int2,
                request.Limit,
                request.Str1,
                request.Str2,
                winner.Value.Value);
    }
}

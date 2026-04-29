using FizzSeven.Core.FizzBuzz;
using StackExchange.Redis;

namespace FizzSeven.Api.Features.Statistics;

public sealed class RedisFizzBuzzStatisticsService(
    IConnectionMultiplexer connectionMultiplexer,
    ILogger<RedisFizzBuzzStatisticsService> logger) : IFizzBuzzStatisticsService
{
    private const string RankingKey = "fizzseven:statistics:ranking";

    public async Task RecordRequestAsync(FizzBuzzRequest request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var database = connectionMultiplexer.GetDatabase();
        var fingerprint = FizzBuzzRequestFingerprint.Create(request);

        try
        {
            await database.SortedSetIncrementAsync(RankingKey, fingerprint, 1).ConfigureAwait(false);
        }
        catch (RedisException exception) when (!cancellationToken.IsCancellationRequested)
        {
            logger.LogWarning(exception, "Unable to record FizzBuzz statistics in Redis.");
        }
    }

    public async Task<MostUsedFizzBuzzRequestResponse?> GetMostFrequentRequestAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var database = connectionMultiplexer.GetDatabase();
        var ranking = await database
            .SortedSetRangeByRankWithScoresAsync(RankingKey, 0, 0, Order.Descending)
            .ConfigureAwait(false);

        if (ranking.Length == 0)
        {
            return null;
        }

        var winner = ranking[0];
        var fingerprint = winner.Element.ToString();

        if (!FizzBuzzRequestFingerprint.TryParse(fingerprint, out var request) || request is null)
        {
            throw new InvalidOperationException($"Stored statistics fingerprint '{fingerprint}' could not be parsed.");
        }

        return new MostUsedFizzBuzzRequestResponse(
            request.Int1,
            request.Int2,
            request.Limit,
            request.Str1,
            request.Str2,
            Convert.ToInt64(winner.Score));
    }
}

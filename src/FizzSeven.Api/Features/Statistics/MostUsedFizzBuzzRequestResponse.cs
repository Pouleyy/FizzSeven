namespace FizzSeven.Api.Features.Statistics;

public sealed record MostUsedFizzBuzzRequestResponse(
    int Int1,
    int Int2,
    int Limit,
    string Str1,
    string Str2,
    long Hits);

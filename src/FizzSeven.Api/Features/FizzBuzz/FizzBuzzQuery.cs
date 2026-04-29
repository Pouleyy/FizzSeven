using FizzSeven.Core.FizzBuzz;

namespace FizzSeven.Api.Features.FizzBuzz;

public sealed class FizzBuzzQuery
{
    public int Int1 { get; init; }

    public int Int2 { get; init; }

    public int Limit { get; init; }

    public string Str1 { get; init; } = string.Empty;

    public string Str2 { get; init; } = string.Empty;

    public FizzBuzzRequest ToRequest() => new(Int1, Int2, Limit, Str1, Str2);
}

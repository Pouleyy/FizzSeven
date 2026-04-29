using System.ComponentModel.DataAnnotations;

namespace FizzSeven.Api.Configuration;

public sealed class FizzBuzzOptions
{
    public const string SectionName = "FizzBuzz";

    [Range(1, int.MaxValue)]
    public int MaxLimit { get; init; } = 100;
}

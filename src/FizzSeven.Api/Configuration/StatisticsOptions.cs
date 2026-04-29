namespace FizzSeven.Api.Configuration;

public sealed class StatisticsOptions
{
    public const string SectionName = "Statistics";

    public bool UseInMemoryStore { get; init; }
}

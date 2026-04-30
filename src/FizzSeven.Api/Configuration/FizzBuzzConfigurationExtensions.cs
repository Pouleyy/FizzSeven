using FizzSeven.Api.Features.FizzBuzz;
using FizzSeven.Core.FizzBuzz;

namespace FizzSeven.Api.Configuration;

public static class FizzBuzzConfigurationExtensions
{
    public static IServiceCollection AddFizzBuzzServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<FizzBuzzOptions>()
            .Bind(configuration.GetSection(FizzBuzzOptions.SectionName))
            .ValidateDataAnnotations()
            .Validate(
                options => options.MaxLimit > 0,
                $"{FizzBuzzOptions.SectionName}:{nameof(FizzBuzzOptions.MaxLimit)} must be greater than zero.")
            .ValidateOnStart();

        services.AddSingleton<IFizzBuzzSequenceService, FizzBuzzSequenceService>();
        services.AddScoped<FizzBuzzQueryValidator>();

        return services;
    }
}

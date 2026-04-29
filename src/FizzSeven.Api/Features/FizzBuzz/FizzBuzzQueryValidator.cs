using FizzSeven.Api.Configuration;
using Microsoft.Extensions.Options;

namespace FizzSeven.Api.Features.FizzBuzz;

public sealed class FizzBuzzQueryValidator(IOptions<FizzBuzzOptions> options)
{
    private readonly FizzBuzzOptions _options = options.Value;

    public IReadOnlyDictionary<string, string[]> Validate(FizzBuzzQuery query)
    {
        var errors = new Dictionary<string, List<string>>(StringComparer.Ordinal);

        if (query.Int1 <= 0)
        {
            AddError(errors, nameof(query.Int1), "int1 must be greater than zero.");
        }

        if (query.Int2 <= 0)
        {
            AddError(errors, nameof(query.Int2), "int2 must be greater than zero.");
        }

        if (query.Limit <= 0)
        {
            AddError(errors, nameof(query.Limit), "limit must be greater than zero.");
        }
        else if (query.Limit > _options.MaxLimit)
        {
            AddError(errors, nameof(query.Limit), $"limit must be less than or equal to {_options.MaxLimit}.");
        }

        if (string.IsNullOrWhiteSpace(query.Str1))
        {
            AddError(errors, nameof(query.Str1), "str1 must not be empty.");
        }

        if (string.IsNullOrWhiteSpace(query.Str2))
        {
            AddError(errors, nameof(query.Str2), "str2 must not be empty.");
        }

        return errors.ToDictionary(static pair => pair.Key, static pair => pair.Value.ToArray(), StringComparer.Ordinal);
    }

    private static void AddError(IDictionary<string, List<string>> errors, string key, string message)
    {
        if (!errors.TryGetValue(key, out var fieldErrors))
        {
            fieldErrors = [];
            errors[key] = fieldErrors;
        }

        fieldErrors.Add(message);
    }
}

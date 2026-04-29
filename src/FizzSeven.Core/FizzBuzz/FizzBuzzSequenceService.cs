namespace FizzSeven.Core.FizzBuzz;

public sealed class FizzBuzzSequenceService : IFizzBuzzSequenceService
{
    public IReadOnlyList<string> Generate(FizzBuzzRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        Validate(request);

        var results = new List<string>(request.Limit);

        for (var number = 1; number <= request.Limit; number++)
        {
            var isInt1Multiple = number % request.Int1 == 0;
            var isInt2Multiple = number % request.Int2 == 0;

            if (isInt1Multiple && isInt2Multiple)
            {
                results.Add(string.Concat(request.Str1, request.Str2));
            }
            else if (isInt1Multiple)
            {
                results.Add(request.Str1);
            }
            else if (isInt2Multiple)
            {
                results.Add(request.Str2);
            }
            else
            {
                results.Add(number.ToString());
            }
        }

        return results;
    }

    private static void Validate(FizzBuzzRequest request)
    {
        if (request.Int1 <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(request.Int1), "int1 must be greater than zero.");
        }

        if (request.Int2 <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(request.Int2), "int2 must be greater than zero.");
        }

        if (request.Limit <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(request.Limit), "limit must be greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(request.Str1))
        {
            throw new ArgumentException("str1 must not be empty.", nameof(request.Str1));
        }

        if (string.IsNullOrWhiteSpace(request.Str2))
        {
            throw new ArgumentException("str2 must not be empty.", nameof(request.Str2));
        }
    }
}

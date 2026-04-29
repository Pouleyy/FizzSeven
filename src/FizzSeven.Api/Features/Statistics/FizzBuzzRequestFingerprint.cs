using System.Text;
using FizzSeven.Core.FizzBuzz;
using Microsoft.AspNetCore.WebUtilities;

namespace FizzSeven.Api.Features.Statistics;

public static class FizzBuzzRequestFingerprint
{
    public static string Create(FizzBuzzRequest request)
    {
        return string.Join(
            '|',
            request.Int1,
            request.Int2,
            request.Limit,
            Encode(request.Str1),
            Encode(request.Str2));
    }

    public static bool TryParse(string fingerprint, out FizzBuzzRequest? request)
    {
        request = null;

        var segments = fingerprint.Split('|', StringSplitOptions.None);

        if (segments.Length != 5
            || !int.TryParse(segments[0], out var int1)
            || !int.TryParse(segments[1], out var int2)
            || !int.TryParse(segments[2], out var limit))
        {
            return false;
        }

        try
        {
            request = new FizzBuzzRequest(int1, int2, limit, Decode(segments[3]), Decode(segments[4]));
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }

    private static string Encode(string value) => WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(value));

    private static string Decode(string value) => Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(value));
}

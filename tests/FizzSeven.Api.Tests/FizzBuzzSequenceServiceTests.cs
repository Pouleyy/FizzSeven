using FizzSeven.Core.FizzBuzz;

namespace FizzSeven.Api.Tests;

public sealed class FizzBuzzSequenceServiceTests
{
    private readonly FizzBuzzSequenceService _service = new();

    [Fact]
    public void Generate_ReturnsExpectedSequence()
    {
        var request = new FizzBuzzRequest(3, 5, 15, "fizz", "buzz");

        var result = _service.Generate(request);

        Assert.Equal(
            ["1", "2", "fizz", "4", "buzz", "fizz", "7", "8", "fizz", "buzz", "11", "fizz", "13", "14", "fizzbuzz"],
            result);
    }

    [Fact]
    public void Generate_UsesCustomReplacementStrings()
    {
        var request = new FizzBuzzRequest(2, 4, 8, "foo", "bar");

        var result = _service.Generate(request);

        Assert.Equal(["1", "foo", "3", "foobar", "5", "foo", "7", "foobar"], result);
    }

    [Theory]
    [InlineData(0, 5, 10)]
    [InlineData(-1, 5, 10)]
    [InlineData(3, 0, 10)]
    [InlineData(3, -5, 10)]
    [InlineData(3, 5, 0)]
    [InlineData(3, 5, -10)]
    public void Generate_ThrowsForNonPositiveNumbers(int int1, int int2, int limit)
    {
        var request = new FizzBuzzRequest(int1, int2, limit, "fizz", "buzz");

        Assert.ThrowsAny<ArgumentOutOfRangeException>(() => _service.Generate(request));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Generate_ThrowsForBlankStr1(string str1)
    {
        var request = new FizzBuzzRequest(3, 5, 10, str1, "buzz");

        Assert.Throws<ArgumentException>(() => _service.Generate(request));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Generate_ThrowsForBlankStr2(string str2)
    {
        var request = new FizzBuzzRequest(3, 5, 10, "fizz", str2);

        Assert.Throws<ArgumentException>(() => _service.Generate(request));
    }
}

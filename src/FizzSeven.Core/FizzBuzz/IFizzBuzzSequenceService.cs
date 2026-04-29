namespace FizzSeven.Core.FizzBuzz;

public interface IFizzBuzzSequenceService
{
    IReadOnlyList<string> Generate(FizzBuzzRequest request);
}

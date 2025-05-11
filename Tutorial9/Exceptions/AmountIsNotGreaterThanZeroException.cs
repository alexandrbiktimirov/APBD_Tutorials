namespace Tutorial9.Exceptions;

public class AmountIsNotGreaterThanZeroException : Exception
{
    public AmountIsNotGreaterThanZeroException(string? message) : base(message)
    {
    }
}
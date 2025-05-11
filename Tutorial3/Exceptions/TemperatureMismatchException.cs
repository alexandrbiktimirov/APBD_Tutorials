namespace Tutorial3.Exceptions;

public class TemperatureMismatchException : Exception
{
    public TemperatureMismatchException(string? message) : base(message)
    {
    }
}
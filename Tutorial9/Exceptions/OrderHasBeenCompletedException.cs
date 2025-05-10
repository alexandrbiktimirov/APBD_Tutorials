namespace Tutorial9.Exceptions;

public class OrderHasBeenCompletedException : Exception
{
    public OrderHasBeenCompletedException(string? message) : base(message)
    {
    }
}
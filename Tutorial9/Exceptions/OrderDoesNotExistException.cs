namespace Tutorial9.Exceptions;

public class OrderDoesNotExistException : Exception
{
    public OrderDoesNotExistException(string? message) : base(message)
    {
        
    }
}
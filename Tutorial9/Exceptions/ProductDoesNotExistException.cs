namespace Tutorial9.Exceptions;

public class ProductDoesNotExistException : Exception
{
    public ProductDoesNotExistException(string? message) : base(message)
    {
    }
}
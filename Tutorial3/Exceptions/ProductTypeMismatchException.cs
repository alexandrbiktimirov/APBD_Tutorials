namespace Tutorial3.Exceptions;

public class ProductTypeMismatchException : Exception
{
    public ProductTypeMismatchException(string? message) : base(message)
    {
    }
}
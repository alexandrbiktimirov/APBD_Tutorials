namespace Tutorial9.Exceptions;

public class WarehouseDoesNotExistException : Exception
{
    public WarehouseDoesNotExistException(string? message) : base(message)
    {
    }
}
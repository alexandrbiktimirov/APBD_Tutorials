namespace Tutorial11.Exceptions;

public class MedicationDoesNotExist : Exception
{
    public MedicationDoesNotExist(string? message) : base(message)
    {
    }
}
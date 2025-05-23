namespace Tutorial11.Exceptions;

public class MedicamentDoesNotExistException(string? message) : Exception(message);
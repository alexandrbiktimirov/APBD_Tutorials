namespace Tutorial11.Exceptions;

public class PatientDoesNotExistException(string? message) : Exception(message);
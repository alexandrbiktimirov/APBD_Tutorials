namespace Tutorial11.Exceptions;

public class DoctorDoesNotExistException(string? message) : Exception(message);
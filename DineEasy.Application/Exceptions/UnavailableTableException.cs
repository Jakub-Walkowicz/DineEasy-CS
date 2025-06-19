namespace DineEasy.Application.Exceptions;

public class UnavailableTableException : Exception
{
    public UnavailableTableException(string message) : base(message) { }
}
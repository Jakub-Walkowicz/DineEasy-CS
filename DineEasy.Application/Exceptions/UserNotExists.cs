namespace DineEasy.Application.Exceptions;

public class UserNotExists : Exception
{
    public UserNotExists(string message) : base(message) { }
}
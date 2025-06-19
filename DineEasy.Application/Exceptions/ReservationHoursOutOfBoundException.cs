namespace DineEasy.Application.Exceptions
{
    public class ReservationHoursOutOfBoundException : Exception
    {
        public ReservationHoursOutOfBoundException(string message) : base(message) { }
    }
}
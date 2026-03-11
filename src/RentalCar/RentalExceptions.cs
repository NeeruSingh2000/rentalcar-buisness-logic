namespace RentalCar;

public class RentalException : Exception
{
    public RentalException(string message) : base(message)
    {
    }
}

public  class DuplicateBookingException : RentalException
{
    public DuplicateBookingException(string bookingNumber)
        : base($"Booking number '{bookingNumber}' is already registered.")
    {
    }
}

public  class RentalNotFoundException : RentalException
{
    public RentalNotFoundException(string bookingNumber)
        : base($"Booking number '{bookingNumber}' was not found.")
    {
    }
}

public  class RentalAlreadyReturnedException : RentalException
{
    public RentalAlreadyReturnedException(string bookingNumber)
        : base($"Booking number '{bookingNumber}' has already been returned.")
    {
    }
}

public  class InvalidReturnException : RentalException
{
    public InvalidReturnException(string message) : base(message)
    {
    }
}

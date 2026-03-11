namespace RentalCar;

public class Rental
{
    public string BookingNumber { get; }
    public string RegistrationNumber { get; }
    public string CustomerSsn { get; }
    public CarCategory Category { get; }
    public DateTime PickupTime { get; }
    public int PickupMeterKm { get; }
    public DateTime? ReturnTime { get; private set; }
    public int? ReturnMeterKm { get; private set; }
    public decimal? Price { get; private set; }

    public Rental(PickupRegistration registration)
    {
        if (registration is null)
        {
            throw new ArgumentNullException(nameof(registration));
        }

        if (string.IsNullOrWhiteSpace(registration.BookingNumber))
        {
            throw new ArgumentException("Booking number is required.", nameof(registration));
        }

        if (string.IsNullOrWhiteSpace(registration.RegistrationNumber))
        {
            throw new ArgumentException("Registration number is required.", nameof(registration));
        }

        if (string.IsNullOrWhiteSpace(registration.CustomerSsn))
        {
            throw new ArgumentException("Customer SSN is required.", nameof(registration));
        }

        if (registration.PickupMeterKm < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(registration.PickupMeterKm));
        }

        BookingNumber = registration.BookingNumber;
        RegistrationNumber = registration.RegistrationNumber;
        CustomerSsn = registration.CustomerSsn;
        Category = registration.Category;
        PickupTime = registration.PickupTime;
        PickupMeterKm = registration.PickupMeterKm;
    }

    public void RegisterReturn(DateTime returnTime, int returnMeterKm, RentalRates rates)
    {
        if (ReturnTime.HasValue)
        {
            throw new RentalAlreadyReturnedException(BookingNumber);
        }

        if (returnTime <= PickupTime)
        {
            throw new InvalidReturnException("Return time must be after pickup time.");
        }

        if (returnMeterKm < PickupMeterKm)
        {
            throw new InvalidReturnException("Return meter reading cannot be lower than pickup meter reading.");
        }

        var rentalDays = CalculateRentalDays(PickupTime, returnTime);
        var rentalKm = returnMeterKm - PickupMeterKm;

        ReturnTime = returnTime;
        ReturnMeterKm = returnMeterKm;
        Price = RentalPriceCalculator.Calculate(Category, rentalDays, rentalKm, rates);
    }

    private static int CalculateRentalDays(DateTime pickupTime, DateTime returnTime)
    {
        var totalDays = (returnTime - pickupTime).TotalDays;
        var roundedDays = (int)Math.Ceiling(totalDays);

        // Rental days are charged as full days, rounding up partial days.
        return Math.Max(1, roundedDays);
    }
}

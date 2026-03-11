using System.Collections.Concurrent;

namespace RentalCar;

public sealed class RentalService
{
    private readonly ConcurrentDictionary<string, Rental> _rentals = new(StringComparer.OrdinalIgnoreCase);

    public Rental RegisterPickup(PickupRegistration registration)
    {
        ArgumentNullException.ThrowIfNull(registration);

        if (string.IsNullOrWhiteSpace(registration.BookingNumber))
        {
            throw new ArgumentException("Booking number is required.", nameof(registration));
        }

        var rental = new Rental(registration);

        if (!_rentals.TryAdd(registration.BookingNumber, rental))
        {
            throw new DuplicateBookingException(registration.BookingNumber);
        }

        return rental;
    }

    public Rental RegisterReturn(ReturnRegistration registration, RentalRates rates)
    {
        ArgumentNullException.ThrowIfNull(registration);

        if (string.IsNullOrWhiteSpace(registration.BookingNumber))
        {
            throw new ArgumentException("Booking number is required.", nameof(registration));
        }

        if (!_rentals.TryGetValue(registration.BookingNumber, out var rental))
        {
            throw new RentalNotFoundException(registration.BookingNumber);
        }

        rental.RegisterReturn(registration.ReturnTime, registration.ReturnMeterKm, rates);
        return rental;
    }

    public Rental? GetRental(string bookingNumber)
    {
        if (string.IsNullOrWhiteSpace(bookingNumber))
        {
            throw new ArgumentException("Booking number is required.", nameof(bookingNumber));
        }

        _rentals.TryGetValue(bookingNumber, out var rental);
        return rental;
    }
}

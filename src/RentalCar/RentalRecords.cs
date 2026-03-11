namespace RentalCar;

public record PickupRegistration(
    string BookingNumber,
    string RegistrationNumber,
    string CustomerSsn,
    CarCategory Category,
    DateTime PickupTime,
    int PickupMeterKm);

public record ReturnRegistration(
    string BookingNumber,
    DateTime ReturnTime,
    int ReturnMeterKm);

public record RentalRates(decimal BaseDayRental, decimal BaseKmPrice);
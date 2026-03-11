namespace RentalCar.Tests;

public class RentalServiceTests
{
    [Test]
    public void RegisterPickup_StoresRentalDetails()
    {
        var service = new RentalService();
        var pickup = new PickupRegistration(
            "B-100",
            "REG-123",
            "790101-1234",
            CarCategory.Combi,
            new DateTime(2026, 3, 1, 9, 30, 0, DateTimeKind.Utc),
            25000);

        var rental = service.RegisterPickup(pickup);

        Assert.That(rental.BookingNumber, Is.EqualTo("B-100"));
        Assert.That(rental.RegistrationNumber, Is.EqualTo("REG-123"));
        Assert.That(rental.CustomerSsn, Is.EqualTo("790101-1234"));
        Assert.That(rental.Category, Is.EqualTo(CarCategory.Combi));
        Assert.That(rental.PickupMeterKm, Is.EqualTo(25000));
    }

    [Test]
    public void RegisterPickup_ThrowsOnDuplicateBooking()
    {
        var service = new RentalService();
        var pickup = new PickupRegistration(
            "B-101",
            "REG-456",
            "800202-4321",
            CarCategory.SmallCar,
            new DateTime(2026, 3, 2, 8, 0, 0, DateTimeKind.Utc),
            12000);

        service.RegisterPickup(pickup);

        Assert.Throws<DuplicateBookingException>(() => service.RegisterPickup(pickup));
    }

    [Test]
    public void RegisterReturn_CalculatesPriceAndStoresReturnDetails()
    {
        var service = new RentalService();
        var pickup = new PickupRegistration(
            "B-200",
            "REG-789",
            "810303-6789",
            CarCategory.SmallCar,
            new DateTime(2026, 3, 1, 10, 0, 0, DateTimeKind.Utc),
            1000);

        service.RegisterPickup(pickup);

        var rental = service.RegisterReturn(
            new ReturnRegistration("B-200", new DateTime(2026, 3, 3, 10, 0, 0, DateTimeKind.Utc), 1100),
            new RentalRates(50, 2));

        Assert.That(rental.ReturnMeterKm, Is.EqualTo(1100));
        Assert.That(rental.ReturnTime, Is.EqualTo(new DateTime(2026, 3, 3, 10, 0, 0, DateTimeKind.Utc)));
        Assert.That(rental.Price, Is.EqualTo(100m));
    }

    [Test]
    public void RegisterReturn_RoundsUpPartialDay()
    {
        var service = new RentalService();
        var pickup = new PickupRegistration(
            "B-300",
            "REG-555",
            "820404-5555",
            CarCategory.SmallCar,
            new DateTime(2026, 3, 5, 9, 0, 0, DateTimeKind.Utc),
            5000);

        service.RegisterPickup(pickup);

        var rental = service.RegisterReturn(
            new ReturnRegistration("B-300", new DateTime(2026, 3, 6, 8, 0, 0, DateTimeKind.Utc), 5000),
            new RentalRates(80m, 1m));

        Assert.That(rental.Price, Is.EqualTo(80m));
    }

    [Test]
    public void RegisterReturn_ThrowsWhenReturnBeforePickup()
    {
        var service = new RentalService();
        var pickup = new PickupRegistration(
            "B-400",
            "REG-777",
            "830505-7777",
            CarCategory.Combi,
            new DateTime(2026, 3, 7, 12, 0, 0, DateTimeKind.Utc),
            8000);

        service.RegisterPickup(pickup);

        Assert.Throws<InvalidReturnException>(() =>
            service.RegisterReturn(
                new ReturnRegistration("B-400", new DateTime(2026, 3, 7, 11, 0, 0, DateTimeKind.Utc), 8050),
                new RentalRates(100m, 1m)));
    }

    [Test]
    public void RegisterReturn_ThrowsWhenMeterDecreases()
    {
        var service = new RentalService();
        var pickup = new PickupRegistration(
            "B-500",
            "REG-888",
            "840606-8888",
            CarCategory.SmallCar,
            new DateTime(2026, 3, 8, 9, 0, 0, DateTimeKind.Utc),
            1500);

        service.RegisterPickup(pickup);

        Assert.Throws<InvalidReturnException>(() =>
            service.RegisterReturn(
                new ReturnRegistration("B-500", new DateTime(2026, 3, 9, 9, 0, 0, DateTimeKind.Utc), 1400),
                new RentalRates(100m, 1m)));
    }
}

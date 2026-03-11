namespace RentalCar;

public static class RentalPriceCalculator
{
    public static decimal Calculate(
        CarCategory category,
        int numberOfDays,
        int numberOfKm,
        RentalRates rates)
    {
        ArgumentNullException.ThrowIfNull(rates);

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(numberOfDays);

        ArgumentOutOfRangeException.ThrowIfNegative(numberOfKm);

        return category switch
        {
            CarCategory.SmallCar => rates.BaseDayRental * numberOfDays,
            CarCategory.Combi => rates.BaseDayRental * numberOfDays * 1.3m + rates.BaseKmPrice * numberOfKm,
            CarCategory.Truck => rates.BaseDayRental * numberOfDays * 1.5m + rates.BaseKmPrice * numberOfKm * 1.5m,
            _ => throw new NotSupportedException($"Category '{category}' is not supported.")
        };
    }
}

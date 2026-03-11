namespace RentalCar.Tests;

public class RentalPriceCalculatorTests
{
    [TestCase(CarCategory.SmallCar, 3, 200, 300)]
    [TestCase(CarCategory.Combi, 3, 200, 790)]
    [TestCase(CarCategory.Truck, 3, 200, 1050)]
    public void Calculate_UsesCategoryFormula(CarCategory category, int days, int km, decimal expected)
    {
        var rates = new RentalRates(100m, 2m);

        var price = RentalPriceCalculator.Calculate(category, days, km, rates);

        Assert.That(price, Is.EqualTo(expected));
    }

    [Test]
    public void Calculate_ThrowsOnInvalidDays()
    {
        var rates = new RentalRates(100m, 2.2m);

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            RentalPriceCalculator.Calculate(CarCategory.SmallCar, 0, 10, rates));
    }

    [Test]
    public void Calculate_ThrowsOnInvalidKm()
    {
        var rates = new RentalRates(100m, 2.2m);

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            RentalPriceCalculator.Calculate(CarCategory.SmallCar, 1, -1, rates));
    }
}

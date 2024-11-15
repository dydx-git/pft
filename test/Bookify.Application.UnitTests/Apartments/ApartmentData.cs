using Pft.Domain.Shared;

namespace Pft.Application.UnitTests.Apartments;
internal static class ApartmentData
{
    public static Apartment Create() => new(
        ApartmentId.New(),
        "Test Apartment",
        "Test Description",
        new Address("Country", "State", "ZipCode", "City", "Street"),
        new Money(100.0m, Currency.Usd),
        Money.Zero(),
        []);
}

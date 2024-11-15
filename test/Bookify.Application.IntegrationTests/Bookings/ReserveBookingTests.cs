using Pft.Application.IntegrationTests.Infrastructure;
using FluentAssertions;
using Pft.Application.Bookings.ReserveBooking;
using Pft.Domain.Entities.Users;

namespace Pft.Application.IntegrationTests.Bookings;
public class ReserveBookingTests(IntegrationTestWebAppFactory webAppFactory) : BaseIntegrationTest(webAppFactory)
{
    private static readonly Guid ApartmentId = Guid.NewGuid();
    private static readonly Guid UserId = Guid.NewGuid();
    private static readonly DateOnly StartDate = new DateOnly(2024, 1, 1);
    private static readonly DateOnly EndDate = new DateOnly(2024, 1, 10);


    [Fact]
    public async Task ReserveBooking_ShouldReturnFailure_WhenUserIsNotFound()
    {
        // Arrange
        var command = new ReserveBookingCommand(ApartmentId, UserId, StartDate, EndDate);

        // Act
        var result = await Sender.Send(command);

        // Assert
        result.Error.Should().Be(UserErrors.NotFound);
    }

}

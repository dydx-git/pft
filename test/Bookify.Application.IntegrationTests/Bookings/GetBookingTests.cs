using Pft.Application.IntegrationTests.Infrastructure;
using FluentAssertions;
using Pft.Application.Bookings.GetBooking;

namespace Pft.Application.IntegrationTests.Bookings;
public class GetBookingTests(IntegrationTestWebAppFactory webAppFactory) : BaseIntegrationTest(webAppFactory)
{
    private static readonly Guid BookingId = Guid.NewGuid();

    [Fact]
    public async Task GetBooking_ShouldReturnFailure_WhenBookingIsNotFound()
    {
        // Arrange
        var command = new GetBookingQuery(BookingId);

        // Act
        var result = await Sender.Send(command);

        // Assert
        result.Error.Should().Be(BookingErrors.NotFound);
    }
}

using Pft.Application.IntegrationTests.Infrastructure;
using FluentAssertions;
using Pft.Application.Apartments.SearchApartments;

namespace Pft.Application.IntegrationTests.Apartments;
public class SearchApartmentsTests(IntegrationTestWebAppFactory webAppFactory) : BaseIntegrationTest(webAppFactory)
{
    [Fact]
    public async Task SearchApartments_ShouldReturnEmptyList_WhenDateRangeIsInvalid()
    {
        // Arrange
        var query = new SearchApartmentsQuery(new DateOnly(2024, 1, 10), new DateOnly(2024, 1, 1));

        // Act
        var result = await Sender.Send(query);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
    }

    [Fact]
    public async Task SearchApartments_ShouldReturnApartments_WhenDateRangeIsValid()
    {
        // Arrange
        var query = new SearchApartmentsQuery(new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 10));

        // Act
        var result = await Sender.Send(query);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
    }
}

using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pft.Application.Bookings.GetBooking;
using Pft.Application.Bookings.ReserveBooking;

namespace Bookify.API.Controllers.Bookings;

[Authorize]
[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route("api/v{version:apiVersion}/bookings")]
public class BookingsController(ISender sender) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBooking(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetBookingQuery(id);

        var result = await sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> ReserveBooking(
        ReserveBookingRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ReserveBookingCommand(
            request.ApartmentId,
            request.UserId,
            request.StartDate,
            request.EndDate);

        var result = await sender.Send(command, cancellationToken);

        if (result.IsFailure) return BadRequest(result.Error);

        return CreatedAtAction(nameof(GetBooking), new { id = result.Value }, result.Value);
    }
}

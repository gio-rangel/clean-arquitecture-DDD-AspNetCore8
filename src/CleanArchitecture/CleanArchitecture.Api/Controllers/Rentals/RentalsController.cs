using CleanArchitecture.Application.Rentals.Book;
using CleanArchitecture.Application.Rentals.GetRental;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Api.Controllers.Cars; 

[ApiController]
[Route("api/car-rentals")]
public class RentalsController : ControllerBase
{
    private readonly ISender _sender;

    public RentalsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRental(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetRentalQuery(id); 

        var result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess ? Ok(result) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> BookRental(
        Guid id,
        RentalBookRequest request,
        CancellationToken cancellationToken
    )
    {
        var command = new BookRentalCommand(
            request.CarId,
            request.UserId,
            request.StartDate,
            request.EndDate
        );

        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return CreatedAtAction(
            nameof(GetRental), 
            new { id = result.Value }
        );
    }
}

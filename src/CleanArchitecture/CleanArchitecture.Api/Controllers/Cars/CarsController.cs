using CleanArchitecture.Application.Cars.SearchCars.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Api.Controllers.Cars; 

[ApiController]
[Route("api/cars")]
public class CarsController : ControllerBase
{
    private readonly ISender _sender;

    public CarsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> SearchCars
    (
        DateOnly startDate, 
        DateOnly endDate,
        CancellationToken cancellationToken
    )
    {
        var query = new SearchCarsQuery(startDate, endDate);

        var results = await _sender.Send(query, cancellationToken);

        return Ok(results);
    }
}
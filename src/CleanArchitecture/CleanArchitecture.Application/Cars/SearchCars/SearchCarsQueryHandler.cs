using CleanArchitecture.Application.Abstractions.Data;
using CleanArchitecture.Application.Abstractions.Messaging;
using CleanArchitecture.Application.Cars.SearchCars.Responses;
using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Cars.ValueObjects;
using CleanArchitecture.Domain.Rentals.Enums;
using Dapper;

namespace CleanArchitecture.Application.Cars.SearchCars;

internal sealed class SearchCarsQueryHandler
: IQueryHandler<SearchCarsQuery, IReadOnlyList<CarResponse>>
{
    private static readonly int[] ActiveRentalStatuses = { 
        (int)RentalStatus.Booked,
        (int)RentalStatus.Confirmed,
        (int)RentalStatus.Completed
    };

    // Atajos: Control + . to get menu list action -> select auto generate constructor 
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public SearchCarsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    async public Task<Result<IReadOnlyList<CarResponse>>> Handle(
        SearchCarsQuery request, 
        CancellationToken cancellationToken
    )
    {
        if(request.Start > request.End)
        {
            return new List<CarResponse>();  
        }

        using var connection = _sqlConnectionFactory.CreateConnection();

        // La condición 'WHERE NOT EXISTS' filtra solo aquellos registros que no existan en la tabla 'rentals' (alquileres)
        // De esta manera cuando el usuario busque los coches disponibles, no podrá rentar aquellos que no matchee con las condiciones impuestas
        // Condiciones: que no este alquilado en el periodo solicitado y que este activo
        const string sqlQuery = """
            SELECT
                c.id AS Id,
                c.model AS MODEL,
                c.vin AS Vin,
                c.price AS Price,
                c.priceCurrency AS PriceCurrencyType,
                c.country_address AS Country,
                c.depto_address AS Depto,
                c.province_address AS Province,
                c.city_address AS City,
                c.street_address AS Street,
                c.street_number_address AS StreetNumber,
            FROM cars AS c
            WHERE NOT EXISTS
            (
                SELECT 1 
                FROM rentals AS r
                WHERE 
                    r.car_id = b.id
                    r.start <= @End AND
                    r.end >= @Start AND
                    r.status = ANY(@ActiveRentalStatuses)

            )
        """;

        var cars = await connection.QueryAsync<CarResponse, AddressResponse, CarResponse>
        (
            sqlQuery,
            (car, address) => {
                car.Address = address;
                return car;
            },
            new 
            {
                Start = request.Start,
                End = request.End,
                ActiveRentalStatuses
            },
            splitOn: "Country"
        );

        return cars.ToList();
    }
}
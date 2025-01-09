using CleanArchitecture.Application.Abstractions.Data;
using CleanArchitecture.Application.Abstractions.Messaging;
using CleanArchitecture.Domain.Abstractions;
using Dapper;

namespace CleanArchitecture.Application.Rentals.GetRental;

internal sealed class GetRentalQueryHandler
: IQueryHandler<GetRentalQuery, RentalResponse>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetRentalQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    async public Task<Result<RentalResponse>> Handle(
        GetRentalQuery request, 
        CancellationToken cancellationToken
    )
    {
        using var connection = _sqlConnectionFactory.CreateConnection();

        var sql = """
            SELECT
                id as Id,
                car_id as CarId,
                status as Status,
                base_price as BasePrice,
                base_price_currency as BasePriceCurrency,
                maintenance_price as MaintenancePrice,
                maintenance_price_currency as MaintenancePriceCurrency,
                accesories_price as AccesoriesPrice,
                accesories_price_currency as AccesoriesPriceCurrency,
                final_price as FinalPrice,
                final_price_currency as FinalPriceCurrency,
                start_date as Start,
                end_date as End,
                created_at as CreatedAt
            FROM rentals WHERE id=@RentalId
        """;

        var rental = await connection.QueryFirstOrDefaultAsync<RentalResponse>(
            sql,
            new {
                request.RentalId
            }
        );

        return rental!; 
    }
}
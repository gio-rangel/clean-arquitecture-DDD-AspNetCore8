using Bogus;
using Bogus.DataSets;
using CleanArchitecture.Application.Abstractions.Data;
using CleanArchitecture.Domain.Cars.ValueObjects;
using Dapper;

namespace CleanArchitecture.Api.Extensions;

public static class SeedDataExtensions 
{
    public static void SeedData (this IApplicationBuilder app) 
    {
        using var scope = app.ApplicationServices.CreateScope();

        var sqlConnectionFactory = scope.ServiceProvider.GetRequiredService<ISqlConnectionFactory>();

        using var connection = sqlConnectionFactory.CreateConnection();

        var faker = new Faker(); 

        List<object> cars = new(); 

        for (var i = 0; i < 50; i++)
        {
            cars.Add(new {
                Id = Guid.NewGuid(),
                Vin = faker.Vehicle.Vin(),
                Model = faker.Vehicle.Model(),
                Country = faker.Address.Country(),
                Province = faker.Address.State(),
                Depto = faker.Address.County(),
                City = faker.Address.City(),
                Street = faker.Address.StreetAddress(),
                PriceAmount = faker.Random.Decimal(1000, 20000),
                PriceCurrencyType = "USD",
                MaintenancePrice = faker.Random.Decimal(100, 300),
                MaintenancePriceCurrencyType = "USD",
                LastRentalDate = DateTime.MinValue,
                Accesories = new List<int>{ (int)Accesory.Wifi, (int)Accesory.AppleCar }
            });
        } 

        const string sql = """
            INSERT INTO public.cars
                (id, vin, model, address_country, address_province, address_depto, address_city, address_street, price_amount, price_currency_type, maintenance_price_amount, maintenance_price_currency_type, last_rental_date, accesories)
                values(@Id, @Vin, @Model, @Country, @Province, @Depto, @City, @Street, @PriceAmount, @PriceCurrencyType, @MaintenancePrice, @MaintenancePriceCurrencyType, @LastRentalDate, @Accesories)
        """;

        connection.Execute(sql, cars); 
    }
}
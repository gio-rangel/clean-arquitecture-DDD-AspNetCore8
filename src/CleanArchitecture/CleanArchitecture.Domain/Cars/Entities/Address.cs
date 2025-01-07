namespace CleanArchitecture.Domain.Cars.Entities;

/**
*Usando record:
*los valores que toman el record deben ser únicos en conjunto
*Por eso no es necesario definir un id
*/

public record Address(
    string Country,
    string Depto,
    string Province,
    string City, 
    string Street,
    int StreetNumber
);
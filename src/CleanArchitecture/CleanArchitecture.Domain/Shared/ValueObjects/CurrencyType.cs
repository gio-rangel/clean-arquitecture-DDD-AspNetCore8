namespace CleanArchitecture.Domain.Shared.ValueObjects;

/**
* Using complex record (value objects in C#)
* We can define how we want to build the properties of value object
*/
public record CurrencyType 
{
    public static readonly CurrencyType None = new("");
    public static readonly CurrencyType Usd = new("USD");
    public static readonly CurrencyType Eur = new("EUR");

    public string? Code {get; init;}
    private CurrencyType(string code) => Code = code; 

    /**
    * Return a CurrencyType collection
    */
    public static readonly IReadOnlyCollection<CurrencyType> All = new[]
    {
        Usd, 
        Eur
    };

    public static CurrencyType FromCode(string code)
    {
        return All.FirstOrDefault(c => c.Code == code) ?? 
            throw new ApplicationException("Invalid currency type");
    }
}
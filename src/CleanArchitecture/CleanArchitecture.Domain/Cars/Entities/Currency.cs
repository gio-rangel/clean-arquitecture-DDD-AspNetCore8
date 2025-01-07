namespace CleanArchitecture.Domain.Cars.Entities;

/**
* complex record (object value)
* With this aproach we get to define the currency and apply the rental payments
* and validate the currency type of the paymants
*/
public record Currency (   
    decimal Amount, 
    CurrencyType CurrencyType
) 
{
    public static Currency operator +(Currency first, Currency second)
    {
        if(first.CurrencyType != second.CurrencyType){
            throw new InvalidOperationException("Invalid: currency type must be the same.");
        }

        return new Currency(first.Amount + second.Amount, first.CurrencyType);
    }

    public static Currency Zero() => new(0, CurrencyType.None); 

    public static Currency Zero(CurrencyType currencyType) => new(0, currencyType);

    public bool IsZero() => this == Zero(CurrencyType); 
}
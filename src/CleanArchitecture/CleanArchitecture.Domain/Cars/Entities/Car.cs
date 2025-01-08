using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Cars.ValueObjects;
using CleanArchitecture.Domain.Shared.ValueObjects;

namespace CleanArchitecture.Domain.Cars.Entities;

/**
* We replaced primitive values for object values (records) instances
* This way the entity is more structured 
*/
public sealed class Car : Entity {

    public Car(
        Guid id,
        Model model,
        Vin vin,
        Currency price,
        Currency maintenancePrice,
        DateTime lastRentalDate,
        List<Accesory> accesories,
        Address address
    ) : base(id)
    {
        Id = id; 
        Model = model; 
        Vin = vin; 
        Price = price; 
        MaintenancePrice = maintenancePrice; 
        LastRentalDate = lastRentalDate; 
        Accesories = accesories; 
        Address = address; 
    }

    public Model? Model {get; private set;}
    public Vin? Vin {get; private set;}
    public Address? Address {get; private set;}
    public Currency? Price {get; private set;}
    public CurrencyType? PriceCurrency {get; private set;}
    public Currency? MaintenancePrice {get; private set;}
    public CurrencyType? MaintenanceCurrency {get; private set;}
    public DateTime? LastRentalDate {get; internal set;}
    public List<Accesory> Accesories {get; private set;} = new();
}
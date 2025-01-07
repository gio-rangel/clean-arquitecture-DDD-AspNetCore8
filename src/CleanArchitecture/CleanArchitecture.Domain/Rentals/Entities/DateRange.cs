namespace CleanArchitecture.Domain.Rentals.Entities;

public sealed record DateRange 
{
    private DateRange(DateOnly start, DateOnly end)
    {
        Start = start;
        End = end;
    }

    public DateOnly Start { get; private set; }
    public DateOnly End { get; private set; }
    public int Days => End.DayNumber - Start.DayNumber;

    public DateRange Create (DateOnly start, DateOnly end)
    {
        if(start > end) 
        {
            throw new ApplicationException("The starting date cannot be higher than the ending date");
        }

        return new DateRange(start, end);
    }
}
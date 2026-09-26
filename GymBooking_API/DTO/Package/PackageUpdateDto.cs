using GymBooking_API.Entity;

public sealed record PackageUpdateDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Includes Includes { get; set; }
    public decimal DailyPrice { get; set; }
    public decimal MonthlyPrice { get; set; }
}
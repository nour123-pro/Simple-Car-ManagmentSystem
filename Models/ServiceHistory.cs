using CarManagmentSystem.Models;

public class ServiceHistory
{
    public int ServiceId { get; set; }
    public int CarId { get; set; }
    public string ServiceType { get; set; } // Enum values
    public DateTime ServiceDate { get; set; }
    public string ServiceDescription { get; set; }
    public decimal? Cost { get; set; }

    public Car Car { get; set; }
}



namespace CarManagmentSystem.Models{
public class Client
{
    public int UserId { get; set; } // Primary key, also acts as foreign key for other entities
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
   

    public ICollection<RentalHistory> RentalHistories { get; set; }
    public ICollection<BuyerHistory> BuyerHistories { get; set; }
    public ICollection<CarDamageHistory> CarDamageHistories { get; set; }
    public ICollection<AccidentHistory> AccidentHistories { get; set; }
}
}
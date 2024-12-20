namespace CarManagmentSystem.Models{
public class CarDto
{
    public int CarId { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public decimal Price { get; set; }
    public int Mileage { get; set; }
    public string FuelType { get; set; }
    public string Transmission { get; set; }
    public string Color { get; set; }
    public int BrandId { get; set; }

     public Brand Brand { get; set; }
        public ICollection<ServiceHistory> ServiceHistories { get; set; }
        public ICollection<RentalHistory> RentalHistories { get; set; }
        public ICollection<BuyerHistory> BuyerHistories { get; set; }
        public ICollection<CarDamageHistory> CarDamageHistories { get; set; }
        public ICollection<AccidentHistory> AccidentHistories { get; set; }
        public ICollection<CarImage> CarImages { get; set; }
}
}
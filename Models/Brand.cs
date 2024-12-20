namespace CarManagmentSystem.Models
{
    public class Brand
    {
        public int BrandId { get; set; }
        public string BrandName { get; set; }

        public ICollection<Car> Cars { get; set; }
    }
}
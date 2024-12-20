using CarManagmentSystem.Models;

public class CarImage
{
    public int ImageId { get; set; }
    public int CarId { get; set; }
    public string ImageUrl { get; set; }
    public string Description { get; set; }
    public DateTime UploadDate { get; set; }

    public Car Car { get; set; }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarManagmentSystem.Models
{
    public class BuyerHistory
    {
        [Key]
        public int HistoryId { get; set; }

        [ForeignKey("Car")]
        public int CarId { get; set; }

        [ForeignKey("Client")]
        public int UserId { get; set; }  // Foreign key to Client

        [DataType(DataType.DateTime)]
        public DateTime PurchaseDate { get; set; } = DateTime.Now;  // Default value is the current date and time

        [Column(TypeName = "decimal(18,2)")]  // Defining decimal precision for Price
        public decimal? Price { get; set; }

        // Navigation properties
        public Car Car { get; set; }
        public Client Client { get; set; }

        // Constructor to initialize the BuyerHistory
        public BuyerHistory(int carId, int userId, decimal? price)
        {
            CarId = carId;
            UserId = userId;
            Price = price;
            PurchaseDate = DateTime.Now;  // Automatically set the purchase date
        }

        public BuyerHistory() {}
    }
}

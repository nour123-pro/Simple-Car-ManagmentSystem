using System;

namespace CarManagmentSystem.Models
{
    public class RentalHistory
    {
        public int RentalId { get; set; }
        public int CarId { get; set; }
        public int UserId { get; set; }  // Foreign key to Client
        public DateTime RentalDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public decimal? RentalPrice { get; set; }

        public Car Car { get; set; }
        public Client Client { get; set; }  // Navigation property
    }
}

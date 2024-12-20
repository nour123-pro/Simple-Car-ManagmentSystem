using System;

namespace CarManagmentSystem.Models
{
    public class CarDamageHistory
    {
        public int DamageId { get; set; }
        public int CarId { get; set; }
        public int UserId { get; set; }  // Foreign key to Client
        public string DamageDescription { get; set; }
        public DateTime DamageDate { get; set; }
        public decimal? RepairCost { get; set; }

        public Car Car { get; set; }
        public Client Client { get; set; }  // Navigation property
    }
}

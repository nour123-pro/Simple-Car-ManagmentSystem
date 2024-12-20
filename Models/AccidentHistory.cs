using System;

namespace CarManagmentSystem.Models
{
    public class AccidentHistory
    {
        public int AccidentId { get; set; }
        public int CarId { get; set; }
        public int UserId { get; set; }  // Foreign key to Client
        public string Description { get; set; }
        public DateTime AccidentDate { get; set; }
        

        public Car Car { get; set; }
        public Client Client { get; set; }  // Navigation property
    }
}

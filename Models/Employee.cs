
using Microsoft.EntityFrameworkCore;

namespace CarManagmentSystem.Models
{
    
public class Employee
{
        public int UserId { get; set; }

     private readonly AppDbContext _context;   
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public string Position { get; set; }
    
    
    public int? BranchId { get; set; }

    public Branch Branch { get; set; }
    
    
}

}
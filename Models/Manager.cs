using CarManagmentSystem.Models;

public class Manager
{
    public int  UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public int? BranchId { get; set; }

    public Branch Branch { get; set; }
}

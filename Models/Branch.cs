namespace  CarManagmentSystem.Models
{
public class Branch
{
    public int BranchId { get; set; }
    public string BranchName { get; set; }
    public string Location { get; set; }

    public ICollection<Employee> Employees { get; set; }
    public ICollection<Manager> Managers { get; set; }
}
}
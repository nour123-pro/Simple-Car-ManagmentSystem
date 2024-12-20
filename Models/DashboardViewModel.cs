namespace CarManagmentSystem.Models
{
    public class DashboardViewModel
    {
        

        public int TotalEmployees { get; set; }
        public double AttendanceOverview { get; set; }
        public List<Employee> Employees { get; set; }
        public Dictionary<string, int> CarCountsByBrand { get; set; }
        public Dictionary<string, int> CarsRentedByBrand 
        { get;set; } 
        public Dictionary<string, int> CarsSoldByBrand { get; set; }   
        public List<User> Users { get; internal set; }
    }
}
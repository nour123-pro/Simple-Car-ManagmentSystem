using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Threading.Tasks;
using CarManagmentSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using System.Security.Principal;
using CarManagmentSystem;
public class UserController : Controller
{
    private readonly AppDbContext _context;

    public UserController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }
 
    public async Task<IActionResult> ManagerDashboard(){
         var totalEmployees = await _context.Employee.CountAsync();
        var attendanceOverview = 90; // Replace with actual calculation if available

        var employees = await _context.Employee
                                       .ToListAsync()
        ;
        var users=await _context.User
                                .Where(u=>u.UserRole==UserRole.Employee)
                                .ToListAsync();

        // Fetch car counts by brand
        // Fetch car counts by brand
var carCountsByBrand = await _context.Car
    .GroupBy(c => c.Brand)
    .Select(group => new { BrandName = group.Key.BrandName, Count = group.Count() }) // Assuming Brand has a Name property
    .ToDictionaryAsync(x => x.BrandName, x => x.Count);
 


var carsRentedByBrand = await _context.Car
    .Where(c => c.RentalHistories.Any()) // Ensure that the car has been rented
    .GroupBy(c => c.Brand.BrandName) // Group cars by brand name
    .Select(group => new 
    { 
        Brand = group.Key, // This is the brand name
        Count = group.Count() // Count of cars rented under this brand
    })
    .ToDictionaryAsync(x => x.Brand, x => x.Count);

    // Calculate cars sold by brand
    var carsSoldByBrand = await _context.Car
        .Where(c => c.BuyerHistories.Any()) 
        .GroupBy(c => c.Brand.BrandName)
        .Select(group => new { Brand = group.Key, Count = group.Count() })
        .ToDictionaryAsync(x => x.Brand, x => x.Count);




        var model = new DashboardViewModel
        {
            TotalEmployees = totalEmployees,
            AttendanceOverview = attendanceOverview,
            Employees =   employees ,
            Users=users,
            CarCountsByBrand = carCountsByBrand,
            CarsRentedByBrand = carsRentedByBrand,
        CarsSoldByBrand = carsSoldByBrand
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(User model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _context.User
            .FirstOrDefaultAsync(u => u.UserName == model.UserName);

        if (user != null && user.UserPassword == model.UserPassword)  
        {
           
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.UserRole.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            return RedirectToAction("Index", "Car");
        }

       
        if (user == null)
        {
            ModelState.AddModelError("", "Username not present in the system.");
        }
        else
        {
            ModelState.AddModelError("", "Password invalid for the given username.");
        }
        
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login", "User");
    }
   




    // POST: User/AddEmployee
    [HttpPost]
    [ValidateAntiForgeryToken]
   public async Task<IActionResult> AddEmployee(Model model)
{  
        User user=new CarManagmentSystem.Models.User();
        var firstname=Request.Form["FirstName"];
        var lastname=Request.Form["LastName"];
        var username=user.GenerateUserName(firstname,lastname);
        var password=user.GeneratePassword();
      
        user.UserName=username;
        user.UserPassword=password;
       user.UserRole = UserRole.Employee;

       
        _context.User.Add(user);
        await _context.SaveChangesAsync();

        
        var employee = new Employee
        {
            UserId = user.UserId,
            FirstName = Request.Form["FirstName"],
            LastName = Request.Form["LastName"],
            Email = Request.Form["Email"],
            Phone = Request.Form["Phone"],
            Address = Request.Form["Address"],
            Position = Request.Form["Position"],
            BranchId =1 
        };

        _context.Employee.Add(employee);
        await _context.SaveChangesAsync();

        return RedirectToAction("ManagerDashboard");
       
    }

     public async Task<IActionResult> BuyCar(int carid) {
    
    var car = await _context.Car.FirstOrDefaultAsync(c => c.CarId == carid);
    if (car == null) {
        return NotFound(); 
    }

    
    var userName = User.Identity.Name;
    var user=_context.User.FirstOrDefault(u=>u.UserName==userName);
    
    var client =_context.Client
                        .Include(c => c.BuyerHistories)
                        .FirstOrDefault(c=>c.UserId==user.UserId);
                               
    if (client == null)
{
    return NotFound("Client not found.");
}
        
    

    
    var buyer = new BuyerHistory {
        CarId = car.CarId,
        UserId = client.UserId,
        Price = car.Price,
        PurchaseDate = DateTime.Now 
    };

   
    client.BuyerHistories.Add(buyer);

    
    try {
        await _context.SaveChangesAsync();
    } catch (Exception ex) {
       
        return StatusCode(500, "An error occurred while saving the transaction.");
    }

    
    return RedirectToAction("Index", "Car");
}

//renting car
[HttpGet]
 public async Task<IActionResult> RentCar(int carid,DateTime date) {
   
    var car = await _context.Car.FirstOrDefaultAsync(c => c.CarId == carid);
    if (car == null) {
        return NotFound(); 
    }

   
    var userName = User.Identity.Name;
    var user=_context.User.FirstOrDefault(u=>u.UserName==userName);
   
    var client =_context.Client
                        .Include(c => c.RentalHistories)
                        .FirstOrDefault(c=>c.UserId==user.UserId);
                               
    if (client == null)
{
    return NotFound("Client not found.");
}
        
    

    
    var rentalHistory = new RentalHistory
{
    CarId = car.CarId, 
    UserId = client.UserId, 
    RentalDate = DateTime.Now,
    ReturnDate = date, 
    RentalPrice = 100.00m 
};


   
    client.RentalHistories.Add(rentalHistory);

    
    try {
        await _context.SaveChangesAsync();
    } catch (Exception ex) {
       
        return StatusCode(500, "An error occurred while saving the transaction.");
    }

    
    return RedirectToAction("Index", "Car");
}
   public IActionResult MainContainer()
    {
        return PartialView("MainContainer");
    }

    public IActionResult AddEmployee()
    {
        return View("AddEmployee");
    }

    public async Task<IActionResult> DeleteEmployee(int userid){
        var user=_context.Employee.FirstOrDefault(e=>e.UserId==userid);
        if(user==null){
            return NotFound();
        }
        _context.Employee.Remove(user);
        await _context.SaveChangesAsync();
        return RedirectToAction("ManagerDashboard"); 
    }



public IActionResult SignUp(){
    return View();
}
[HttpPost]
public async Task<IActionResult> SignUp(ClientSignUpViewModel model)
{
    var user = new User
    {
        UserName = model.User.UserName,
        UserPassword = model.User.UserPassword,
        UserRole = UserRole.Client 
    };

    _context.User.Add(user);
    await _context.SaveChangesAsync();

    var client = new Client
    {
        UserId = user.UserId,
        FirstName = model.Client.FirstName,
        LastName = model.Client.LastName,
        Email = model.Client.Email,
        PhoneNumber = model.Client.PhoneNumber
    };

    _context.Client.Add(client);
    await _context.SaveChangesAsync();

    return RedirectToAction("Index", "Car");
}



public IActionResult ClientProfile(string username)
{
    var current=_context.User.FirstOrDefault(u=>u.UserName==username);
    var client = _context.Client
        .Include(c => c.RentalHistories)
            .ThenInclude(r => r.Car) 
        .Include(c => c.BuyerHistories)
            .ThenInclude(b => b.Car) 
        .FirstOrDefault(c => c.UserId == current.UserId);

    if (client == null)
    {
        return NotFound();
    }

    return View(client);
}






}




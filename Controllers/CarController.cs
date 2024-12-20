using Microsoft.AspNetCore.Mvc;
using CarManagmentSystem.Models; // Adjust namespace as needed
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using Microsoft.Extensions.Hosting;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc.Rendering;
using AutoMapper;

public class CarController : Controller
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _env;
    public CarController(AppDbContext context, IWebHostEnvironment env,IMapper mapper)
    {
        _context = context;
        _mapper=mapper;
        _env=env;
    }

    // GET: /Car/Index
    public async Task<IActionResult> Index(int? brandId)
    {
        // Query to get all cars with related images and brands
        var carsQuery = _context.Car
            .Include(c => c.CarImages)
            .Include(c => c.Brand)
            .AsQueryable();

        // Filter cars by brand if a brandId is provided
        if (brandId.HasValue)
        {
            carsQuery = carsQuery.Where(c => c.BrandId == brandId.Value);
        }

        // Execute the query to get the list of cars
        var cars = await carsQuery.ToListAsync();
        var carDtos = _mapper.Map<IEnumerable<CarDto>>(cars);
        // Get the list of all brands for the filter bar
        var brands = await _context.Brand.ToListAsync();

        // Pass both cars and brands to the view
        ViewData["Brands"] = brands;
        
        return View(carDtos);
    }


    public IActionResult Details(int id)
        {
            var car = _context.Car
                .Include(c => c.Brand)
                .Include(c => c.CarImages)
                .FirstOrDefault(c => c.CarId == id);

            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

    public IActionResult History(int carId)
{
    var car = _context.Car
                     .Include(c => c.AccidentHistories)
                     .Include(c => c.BuyerHistories)
                     .Include(c => c.ServiceHistories)
                     .Include(c=>c.RentalHistories)
                    
                     .FirstOrDefault(c => c.CarId == carId);
    
    if (car == null)
    {
        return NotFound();
    }

    return View(car);
}
    
[HttpPost]
public IActionResult AddServices(int carId, string FuelType, string OilChangeType, string GeneralMaintenance, string BatteryReplacement, string TireChange, string BrakeCheck)
{
    var selectedServices = new List<ServiceHistory>();

    // Helper function to add a service if not "None"
    void AddServiceIfSelected(string type, string value, decimal cost)
    {
        if (value != "None")
        {
            selectedServices.Add(new ServiceHistory
            {
                CarId = carId,
                ServiceType = type,
                ServiceDate = DateTime.Now,
                ServiceDescription = $"{type} Description: {value}",
                Cost = cost
            });
        }
    }

    // Add services if selected
    AddServiceIfSelected("Fuel", FuelType, 50.00m);
    AddServiceIfSelected("Oil Change", OilChangeType, 75.00m);
    AddServiceIfSelected("General Maintenance", GeneralMaintenance, 500.00m);
    AddServiceIfSelected("Battery Replacement", BatteryReplacement, 500.00m);
    AddServiceIfSelected("Tire Change", TireChange, 500.00m);
    AddServiceIfSelected("Brake Check", BrakeCheck, 500.00m);

    // Only add selected services to the database if there are any
    if (selectedServices.Any())
    {
        _context.ServiceHistory.AddRange(selectedServices);
        _context.SaveChanges();
    }

    return RedirectToAction("Details", new { id = carId });
}
public IActionResult Add()
    {
        // Retrieve all brands from the database
        var brands = _context.Brand.ToList();
        
        // Pass the brands to the view using ViewBag
        ViewBag.Brands = new SelectList(brands, "BrandId", "BrandName");

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Add(Car car)
    {
       
            _context.Car.Add(car);  
            await _context.SaveChangesAsync(); 
            return RedirectToAction("Index");
      

       
        
        
    }



    [HttpGet]
public IActionResult Edit(int carid)
{
    // Get the car with the specified carid
    var car = _context.Car
        .Include(c => c.Brand) // Ensure you include the Brand for display
        .FirstOrDefault(c => c.CarId == carid);

    if (car == null)
    {
        return NotFound();
    }

    // Pass the brands to the view using ViewBag for dropdown
    ViewBag.Brands = new SelectList(_context.Brand.ToList(), "BrandId", "BrandName", car.BrandId);

    return View(car);
}

[HttpPost]
public async Task<IActionResult> Edit(int carid, Car updatedCar)
{
   

    var car = await _context.Car.FindAsync(carid);

    if (car == null)
    {
        return NotFound();
    }

    car.Model = updatedCar.Model;
    car.Year = updatedCar.Year;
    car.Price = updatedCar.Price;
    car.Mileage = updatedCar.Mileage;
    car.FuelType = updatedCar.FuelType;
    car.Transmission = updatedCar.Transmission;
    car.Color = updatedCar.Color;
    car.BrandId = updatedCar.BrandId;

    await _context.SaveChangesAsync();

    return RedirectToAction("Index");

}

public IActionResult Delete(int carid){
    var deletedcar=_context.Car.Find(carid);
    if(deletedcar==null){
       return NotFound();
    }
     _context.Car.Remove(deletedcar);
     _context.SaveChanges();
    
    
    return RedirectToAction("Index");
}

    }



    


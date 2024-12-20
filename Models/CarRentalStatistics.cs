using System.Data.SqlClient;

public class CarRentalStatistics
{
    public Dictionary<string, int> GetTotalRentedCarsByBrand()
    {
        // Replace with your actual database connection string
        string connectionString = "Data Source=<localhost>;Initial Catalog=<CarManagment>;User ID=<root>;Password=<nour>;";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            SqlCommand command = new SqlCommand(
                "SELECT b.BrandName, COUNT(c.CarID) AS TotalRentedCars " +
                "FROM Car c " +
                "INNER JOIN Brand b ON c.BrandID = b.BrandID " +
                "INNER JOIN RentalHistory rh ON c.CarID = rh.CarID " +
                "GROUP BY b.BrandName " +
                "ORDER BY TotalRentedCars DESC;",
                connection);

            SqlDataReader reader = command.ExecuteReader();

            Dictionary<string, int> carsRentedByBrand = new Dictionary<string, int>();

            while (reader.Read())
            {
                string brandName = (string)reader["BrandName"];
                int totalRentedCars = (int)reader["TotalRentedCars"];

                carsRentedByBrand.Add(brandName, totalRentedCars);
            }

            return carsRentedByBrand;
        }
    }
}
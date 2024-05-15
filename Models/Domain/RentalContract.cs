namespace CarRental.API.Models.Domain
{
    public class RentalContract
    {
        public int id { get; set; }
        public int CustomerId { get; set; }
        public int VehicleId { get; set; }
        public string Date { get; set; }
        public decimal? Mileage { get; set; }
        public string Status { get; set; }
    }
}

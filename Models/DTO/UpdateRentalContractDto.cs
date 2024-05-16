namespace CarRental.API.Models.DTO
{
    public class UpdateRentalContractDto
    {
        public int CustomerId { get; set; }
        public int VehicleId { get; set; }
        public string Date { get; set; }
        public decimal? Mileage { get; set; }
        public string Status { get; set; }
    }
}

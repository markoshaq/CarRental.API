namespace CarRental.API.Models.Domain
{
    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Salutation { get; set; }
        public string Street { get; set; }
        public int ZipCode { get; set; }
        public string City { get; set; }
        public string? Country { get; set; }
        public string? Phone { get; set; }
        public string EMail { get; set; }
    }
}

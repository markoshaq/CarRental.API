using CarRental.API.Data;
using CarRental.API.Models.Domain;
using CarRental.API.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;
using System.IO;
using System.Numerics;
using System.Reflection.Emit;

namespace CarRental.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly CarRentalDbContext dbContext;
        public CustomersController(CarRentalDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // GET:https://localhost:port/api/customers
        [HttpGet]
        public IActionResult GetCustomers()
        {
            // get domain model from db
            var customers = dbContext.Customers.ToList();

            // create and return dto
            var customersDto = new List<CustomerDto>();
            foreach (var customer in customers)
            {
                customersDto.Add(new CustomerDto()
                {
                    Id = customer.Id,
                    Street = customer.Street,
                    City = customer.City,
                    Country = customer.Country,
                    ZipCode = customer.ZipCode,
                    Salutation = customer.Salutation,
                    EMail = customer.EMail,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Phone = customer.Phone
                });
            }
            return Ok(customersDto);
        }

        // GET:https://localhost:port/api/customers/{id}
        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetCustomerById([FromRoute] int id)
        {
            var customerDomain = dbContext.Customers.Find(id);

            if (customerDomain == null)
            {
                return NotFound();
            }
            
            var customersDto = new CustomerDto()
            {
                Id = customerDomain.Id,
                Street = customerDomain.Street,
                City = customerDomain.City,
                Country = customerDomain.Country,
                ZipCode = customerDomain.ZipCode,
                Salutation = customerDomain.Salutation,
                EMail = customerDomain.EMail,
                FirstName = customerDomain.FirstName,
                LastName = customerDomain.LastName,
                Phone = customerDomain.Phone
            };

            return Ok(customersDto);
        }

        // create new customer
        // POST: https://localhost:port/api/customers
        [HttpPost]
        public IActionResult CreateCustomer([FromBody] AddCustomerDto addCustomerDto)
        {
            // map dto to domain
            var customerDomain = new Customer
            {
                Salutation = addCustomerDto.Salutation,
                FirstName = addCustomerDto.FirstName,
                LastName = addCustomerDto.LastName,
                Street = addCustomerDto.Street,
                ZipCode = addCustomerDto.ZipCode,
                City = addCustomerDto.City,
                Country = addCustomerDto.Country,
                Phone = addCustomerDto.Phone,
                EMail = addCustomerDto.EMail
            };

            // save to db
            dbContext.Customers.Add(customerDomain);
            dbContext.SaveChanges();

            // response body
            var customerDto = new CustomerDto
            {
                Id = customerDomain.Id,
                ZipCode = customerDomain.ZipCode,
                Street = customerDomain.Street,
                FirstName = customerDomain.FirstName,
                LastName = customerDomain.LastName,
                Phone = customerDomain.Phone,
                Salutation = customerDomain.Salutation,
                Country = customerDomain.Country,
                City = customerDomain.City,
                EMail = customerDomain.EMail
            };

            // return response
            return CreatedAtAction(nameof(GetCustomerById), new { id = customerDomain.Id }, customerDto);
        }
    }
}

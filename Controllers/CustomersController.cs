using CarRental.API.Data;
using CarRental.API.Models.Domain;
using CarRental.API.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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

        // get all customers
        // GET:https://localhost:port/api/customers
        [HttpGet]
        public IActionResult GetCustomers()
        {
            var customers = dbContext.Customers.ToList();

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

        // get customer by id
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

        // create customer
        // POST: https://localhost:port/api/customers
        [HttpPost]
        public IActionResult CreateCustomer([FromBody] AddCustomerDto addCustomerDto)
        {
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

            dbContext.Customers.Add(customerDomain);
            dbContext.SaveChanges();

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

            return CreatedAtAction(nameof(GetCustomerById), new { id = customerDomain.Id }, customerDto);
        }

        // delete customer by id
        // DELETE: https://localhost:port/api/customers/id
        [HttpDelete]
        [Route("{id:int}")]
        public IActionResult DeleteCustomer([FromRoute] int id)
        {
            var customerDomain = dbContext.Customers.Find(id);

            if (customerDomain == null)
            {
                return NotFound();
            }

            try
            {
                dbContext.Customers.Remove(customerDomain);
                dbContext.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlEx && sqlEx.Number == 547)
                {
                    return Conflict("Cannot delete the customer because it is referenced by other records.");
                }
            }
            return Ok(customerDomain);
        }

        // update customer by id
        // PUT: https://localhost:port/api/customers/id
        [HttpPut]
        [Route("{id:int}")]
        public IActionResult UpdateCustomer([FromRoute] int id, UpdateCustomerDto updateCustomerDto)
        {
            var customerDomain = dbContext.Customers.Find(id);

            if (customerDomain == null)
            {
                return NotFound();
            }

            customerDomain.FirstName = updateCustomerDto.FirstName;
            customerDomain.LastName = updateCustomerDto.LastName;
            customerDomain.Street = updateCustomerDto.Street;
            customerDomain.City = updateCustomerDto.City;
            customerDomain.Country = updateCustomerDto.Country;
            customerDomain.Salutation = updateCustomerDto.Salutation;
            customerDomain.EMail = updateCustomerDto.EMail;
            customerDomain.Phone = updateCustomerDto.Phone;
            customerDomain.ZipCode = updateCustomerDto.ZipCode;

            dbContext.SaveChanges();
            return Ok(customerDomain);
        }
    }
}

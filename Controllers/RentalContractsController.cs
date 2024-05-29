using CarRental.API.Data;
using CarRental.API.Models.Domain;
using CarRental.API.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CarRental.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalContractsController : ControllerBase
    {
        private readonly CarRentalDbContext dbContext;
        public RentalContractsController(CarRentalDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // get all rental contracts
        // GET:https://localhost:port/api/rentalcontracts
        [HttpGet]
        public IActionResult GetRentalContracts()
        {
            var rentalcontractsDomain = dbContext.RentalContracts.ToList();

            var rentalcontractsDto = new List<RentalContractDto>();
            foreach (var contract in rentalcontractsDomain)
            {
                rentalcontractsDto.Add(new RentalContractDto()
                {
                    id = contract.id,
                    CustomerId = contract.CustomerId,
                    Date = contract.Date,
                    Mileage = contract.Mileage,
                    Status = contract.Status,
                    VehicleId = contract.VehicleId,
                });
            }
            return Ok(rentalcontractsDto);
        }

        // get rental contract by id
        // GET:https://localhost:port/api/rentalcontracts/{id}
        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetRentalContractById([FromRoute] int id)
        {
            var rentalcontractDomain = dbContext.RentalContracts.Find(id);

            if (rentalcontractDomain == null)
            {
                return NotFound();
            }

            var rentalcontractsDto = new RentalContractDto()
            {
                id = rentalcontractDomain.id,
                CustomerId = rentalcontractDomain.CustomerId,
                Date = rentalcontractDomain.Date,
                Mileage = rentalcontractDomain.Mileage,
                Status = rentalcontractDomain.Status,
                VehicleId = rentalcontractDomain.VehicleId,
            };

            return Ok(rentalcontractsDto);
        }

        // create rental contract
        // POST: https://localhost:port/api/rentalcontracts
        [HttpPost]
        public IActionResult CreateRentalContract([FromBody] AddRentalContractDto addRentalContractDto)
        {
            var existingVehicle = dbContext.Vehicles.Find(addRentalContractDto.VehicleId);
            var existingCustomer = dbContext.Customers.Find(addRentalContractDto.CustomerId);
            
            if (existingCustomer == null || existingVehicle == null)
            {
                return NotFound("Vehicle or customer with the provided ID does not exist.");
            }

            var rentalcontractDomain = new RentalContract
            {
                CustomerId = addRentalContractDto.CustomerId,
                Date = addRentalContractDto.Date,
                Mileage = addRentalContractDto.Mileage,
                Status = addRentalContractDto.Status,
                VehicleId = addRentalContractDto.VehicleId,
            };

            dbContext.RentalContracts.Add(rentalcontractDomain);
            dbContext.SaveChanges();

            var rentalcontractDto = new RentalContractDto
            {
                id = rentalcontractDomain.id,
                CustomerId = rentalcontractDomain.CustomerId,
                Date = rentalcontractDomain.Date,
                Mileage = rentalcontractDomain.Mileage,
                Status = rentalcontractDomain.Status,
                VehicleId = rentalcontractDomain.VehicleId
            };

            return CreatedAtAction(nameof(GetRentalContractById), new { id = rentalcontractDomain.id }, rentalcontractDto);
        }

        // delete rental contract
        // DELETE: https://localhost:port/api/rentalcontracts
        [HttpDelete]
        [Route("{id:int}")]
        public IActionResult DeleteRentalContract([FromRoute] int id)
        {
            var contractDomain = dbContext.RentalContracts.Find(id);

            if (contractDomain == null)
            {
                return NotFound();
            }

            dbContext.RentalContracts.Remove(contractDomain);
            dbContext.SaveChanges();

            return Ok(contractDomain);
        }

        // update rental contract
        // PUT: https://localhost:port/api/rentalcontracts/id
        [HttpPut]
        [Route("{id:int}")]
        public IActionResult UpdateRentalContract([FromRoute] int id, UpdateRentalContractDto updateRentalContractDto)
        {
            var contractDomain = dbContext.RentalContracts.Find(id);

            if (contractDomain == null)
            {
                return NotFound();
            }

            try
            {
                contractDomain.CustomerId = updateRentalContractDto.CustomerId;
                contractDomain.Date = updateRentalContractDto.Date;
                contractDomain.Mileage = updateRentalContractDto.Mileage;
                contractDomain.Status = updateRentalContractDto.Status;
                contractDomain.VehicleId = updateRentalContractDto.VehicleId;
                dbContext.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                // check if the exception is due to a foreign key constraint violation
                if (ex.InnerException is SqlException sqlEx && sqlEx.Number == 547)
                {
                    return Conflict("Cannot update the customer because it is referenced by other records.");
                }

            }
            return Ok(contractDomain);
        }
    }
}

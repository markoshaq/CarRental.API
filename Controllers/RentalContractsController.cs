using CarRental.API.Data;
using CarRental.API.Models.Domain;
using CarRental.API.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        // GET:https://localhost:port/api/rentalcontracts
        [HttpGet]
        public IActionResult GetRentalContracts()
        {
            // get domain model from db
            var rentalcontractsDomain = dbContext.RentalContracts.ToList();

            // create and return dto
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

        // create new rental contract
        // POST: https://localhost:port/api/rentalcontracts
        [HttpPost]
        public IActionResult CreateRentalContract([FromBody] AddRentalContractDto addRentalContractDto)
        {
            // map dto to domain
            var rentalcontractDomain = new RentalContract
            {
                CustomerId = addRentalContractDto.CustomerId,
                Date = addRentalContractDto.Date,
                Mileage = addRentalContractDto.Mileage,
                Status = addRentalContractDto.Status,
                VehicleId = addRentalContractDto.VehicleId,
            };

            // save to db
            dbContext.RentalContracts.Add(rentalcontractDomain);
            dbContext.SaveChanges();

            // response body
            var rentalcontractDto = new RentalContractDto
            {
                id = rentalcontractDomain.id,
                CustomerId = rentalcontractDomain.CustomerId,
                Date = rentalcontractDomain.Date,
                Mileage = rentalcontractDomain.Mileage,
                Status = rentalcontractDomain.Status,
                VehicleId = rentalcontractDomain.VehicleId
            };

            // return response
            return CreatedAtAction(nameof(GetRentalContractById), new { id = rentalcontractDomain.id }, rentalcontractDto);
        }
    }
}

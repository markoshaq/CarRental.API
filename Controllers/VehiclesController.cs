using CarRental.API.Data;
using CarRental.API.Models.Domain;
using CarRental.API.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CarRental.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        private readonly CarRentalDbContext dbContext;
        public VehiclesController(CarRentalDbContext dbContext)
        {
                this.dbContext = dbContext;
        }

        // GET:https://localhost:port/api/vehicles
        [HttpGet]
        public IActionResult GetVehicles()
        {
            var vehiclesDomain = dbContext.Vehicles.ToList();

            // create and return dto
            var vehiclesDto = new List<VehicleDto>();
            foreach (var vehicle in vehiclesDomain)
            {
                vehiclesDto.Add(new VehicleDto()
                {
                    id = vehicle.id,
                    Brand = vehicle.Brand,
                    Class = vehicle.Class,
                    FirstRegistration = vehicle.FirstRegistration,
                    LicensePlate = vehicle.LicensePlate,
                    Mileage = vehicle.Mileage,
                    RentPrice = vehicle.RentPrice,
                    TirePressure = vehicle.TirePressure
                });
            }
            return Ok(vehiclesDto);
        }

        // GET:https://localhost:port/api/vehicles/{id}
        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetVehicleById([FromRoute] int id)
        {
            var vehicleDomain = dbContext.Vehicles.Find(id);

            if (vehicleDomain == null)
            {
                return NotFound();
            }

            var vehiclesDto = new VehicleDto()
            {
                id = vehicleDomain.id,
                Brand = vehicleDomain.Brand,
                Class = vehicleDomain.Class,
                FirstRegistration = vehicleDomain.FirstRegistration,
                LicensePlate = vehicleDomain.LicensePlate,
                Mileage = vehicleDomain.Mileage,
                RentPrice = vehicleDomain.RentPrice,
                TirePressure = vehicleDomain.TirePressure
            };

            return Ok(vehiclesDto);
        }

        // create new vehicle
        // POST: https://localhost:port/api/vehicles
        [HttpPost]
        public IActionResult CreateVehicle([FromBody] AddVehicleDto addVehicleDto)
        {
            // map dto to domain
            var vehicleDomain = new Vehicle
            {
                Brand = addVehicleDto.Brand,
                Class = addVehicleDto.Class,
                FirstRegistration = addVehicleDto.FirstRegistration,
                LicensePlate = addVehicleDto.LicensePlate,
                Mileage = addVehicleDto.Mileage,
                RentPrice = addVehicleDto.RentPrice,
                TirePressure = addVehicleDto.TirePressure
            };

            // save to db
            dbContext.Vehicles.Add(vehicleDomain);
            dbContext.SaveChanges();

            // response body
            var vehicleDto = new VehicleDto
            {
                id = vehicleDomain.id,
                Brand = vehicleDomain.Brand,
                Class = vehicleDomain.Class,
                FirstRegistration = vehicleDomain.FirstRegistration,
                LicensePlate = vehicleDomain.LicensePlate,
                Mileage = vehicleDomain.Mileage,
                RentPrice = vehicleDomain.RentPrice,
                TirePressure = vehicleDomain.TirePressure
            };

            // return response
            return CreatedAtAction(nameof(GetVehicleById), new { id = vehicleDomain.id }, vehicleDto);
        }

        // delete vehicle
        // DELETE: https://localhost:port/api/vehicles
        [HttpDelete]
        [Route("{id:int}")]
        public IActionResult DeleteVehicle([FromRoute] int id)
        {
            var vehicleDomain = dbContext.Vehicles.Find(id);

            if (vehicleDomain == null)
            {
                return NotFound();
            }

            try
            {
                dbContext.Vehicles.Remove(vehicleDomain);
                dbContext.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                // check if the exception is due to a foreign key constraint violation
                if (ex.InnerException is SqlException sqlEx && sqlEx.Number == 547)
                {
                    return Conflict("Cannot delete the customer because it is referenced by other records.");
                }
            }

            return Ok(vehicleDomain);
        }

        // update vehicle
        // PUT: https://localhost:port/api/vehicles/id
        [HttpPut]
        [Route("{id:int}")]
        public IActionResult UpdateVehicle([FromRoute] int id, UpdateVehicleDto updateVehicleDto)
        {
            var vehicleDomain = dbContext.Vehicles.Find(id);

            if (vehicleDomain == null)
            {
                return NotFound();
            }

            vehicleDomain.Brand = updateVehicleDto.Brand;
            vehicleDomain.Class = updateVehicleDto.Class;
            vehicleDomain.FirstRegistration = updateVehicleDto.FirstRegistration;
            vehicleDomain.LicensePlate = updateVehicleDto.LicensePlate;
            vehicleDomain.Mileage = updateVehicleDto.Mileage;
            vehicleDomain.RentPrice = updateVehicleDto.RentPrice;
            vehicleDomain.TirePressure = updateVehicleDto.TirePressure;

            dbContext.SaveChanges();
            return Ok(vehicleDomain);
        }
    }
}
using Jungle.Entities;
using Jungle.Exceptions;
using Jungle.Repos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JungleSafari.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private IVehicleRepository repository;

        public VehicleController(IVehicleRepository repository)
        {
            this.repository = repository;
        }

        // GET: api/<VehicleController>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var list = repository.GetAll();
                return Ok(list);
            }
            catch (JungleException)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        // GET api/<VehicleController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var vehicle = repository.Get(id);
                return Ok(vehicle);
            }
            catch (JungleException ex)
            {

                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // POST api/<VehicleController>
        [HttpPost]
        public IActionResult Post([FromBody] Vehicle vehicle)
        {
            //SSV
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //Repo Add(emp) -> T/F
            //T --> Created
            //F --> Internal Server Error
            try
            {
                bool isAdded = repository.Add(vehicle);
                if (isAdded)
                {
                    return Created("vehicle", vehicle);
                }
                else
                {

                    return StatusCode((int)HttpStatusCode.InternalServerError);
                }
            }
            catch (JungleException ex)
            {
                if (ex.Message == "Vehicle alraedy exists")
                {
                    return Conflict(ex.Message);
                    //return StatusCode((int)HttpStatusCode.Forbidden, ex.Message);
                }
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        // PUT api/<VehicleController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Vehicle vehicle)
        {
            //ssv
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //check id
            if (id != vehicle.Vid)
            {
                return BadRequest("No Vehicle Found ");
            }
            //repos.update(emp)
            //T -> Ok
            //F -> internal server error
            try
            {
                bool isUpdate = repository.Update(vehicle);
                if (isUpdate)
                {
                    return Ok("Vehicle Updated");
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, "Failed to Update Vehicle");
                }
            }
            catch (JungleException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // DELETE api/<VehicleController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var vehicle = repository.Get(id);
                if (vehicle == null)
                {
                    return NotFound();
                }
                bool isDelete = repository.Remove(vehicle);
                if (isDelete)
                {
                    return Ok("Vehicle Deleted");
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, "Failed to Delete Vehicle");
                }
            }
            catch (JungleException ex)
            {

                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet("parks/{parkId}")]
        public IActionResult GetByPark(int parkId)
        {
            try
            {
                var list = repository.GetByPark(parkId);
                return Ok(list);
            }
            catch (JungleException ex)
            {

                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}

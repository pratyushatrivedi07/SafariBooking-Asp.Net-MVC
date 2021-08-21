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
    public class ParksController : ControllerBase
    {
        private IParkRepository repository;

        public ParksController(IParkRepository repository)
        {
            this.repository = repository;
        }
        // GET: api/<ParksController>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var list = repository.GetAll();
                return Ok(list);
            }
            catch (JungleException ex)
            {

                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        // GET api/<ParksController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var park = repository.Get(id);
                return Ok(park);
            }
            catch (JungleException ex)
            {

                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // POST api/<ParksController>
        [HttpPost]
        public IActionResult Post([FromBody] Parks park)
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
                bool isAdded = repository.Add(park);
                if (isAdded)
                {
                    return Created("Park", park);
                }
                else
                {

                    return StatusCode((int)HttpStatusCode.InternalServerError);
                }
            }
            catch (JungleException ex)
            {
                if (ex.Message == "Park alraedy exists")
                {
                    return Conflict(ex.Message);
                }
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        // PUT api/<ParksController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Parks park)
        {
            //ssv
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //check id
            if (id != park.ParkId)
            {
                return BadRequest("No Park Found ");
            }
            //repos.update(emp)
            //T -> Ok
            //F -> internal server error
            try
            {
                bool isUpdate = repository.Update(park);
                if (isUpdate)
                {
                    return Ok("Park Updated");
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, "Failed to Update Park");
                }
            }
            catch (JungleException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // DELETE api/<ParksController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var park = repository.Get(id);
                if (park == null)
                {
                    return NotFound();
                }
                bool isDelete = repository.Remove(park);
                if (isDelete)
                {
                    return Ok("Park Deleted");
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, "Failed to Delete Park");
                }
            }
            catch (JungleException ex)
            {

                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet("/{location}")]
        public IActionResult GetByLocation(string location)
        {
            try
            {
                var list = repository.GetByLocation(location);
                return Ok(list);
            }
            catch (JungleException ex)
            {
        
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet("search/{criteria}")]
        public IActionResult GetSearch(string criteria)
        {
            try
            {
                var list = repository.Search(criteria);
                return Ok(list);
            }
            catch (JungleException ex)
            {

                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}

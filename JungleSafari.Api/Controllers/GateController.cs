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
    public class GateController : ControllerBase
    {
        private IGateRepository repository;

        public GateController(IGateRepository repository)
        {
            this.repository = repository;
        }

        // GET: api/<GateController>
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

        // GET api/<GateController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var gate = repository.Get(id);
                return Ok(gate);
            }
            catch (JungleException ex)
            {

                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // POST api/<GateController>
        [HttpPost]
        public IActionResult Post([FromBody] Gate gates)
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
                bool isAdded = repository.Add(gates);
                if (isAdded)
                {
                    return Created("Gate", gates);
                }
                else
                {

                    return StatusCode((int)HttpStatusCode.InternalServerError);
                }
            }
            catch (JungleException ex)
            {
                if (ex.Message == "Gate alraedy exists")
                {
                    return Conflict(ex.Message);
                    //return StatusCode((int)HttpStatusCode.Forbidden, ex.Message);
                }
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        // PUT api/<GateController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Gate gate)
        {
            //ssv
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //check id
            if (id != gate.GateId)
            {
                return BadRequest("No Gate Found ");
            }
            //repos.update(emp)
            //T -> Ok
            //F -> internal server error
            try
            {
                bool isUpdate = repository.Update(gate);
                if (isUpdate)
                {
                    return Ok("Gate Updated");
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

        // DELETE api/<GateController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var gate = repository.Get(id);
                if (gate == null)
                {
                    return NotFound();
                }
                bool isDelete = repository.Remove(gate);
                if (isDelete)
                {
                    return Ok("Gate Deleted");
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, "Failed to Delete Gate");
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

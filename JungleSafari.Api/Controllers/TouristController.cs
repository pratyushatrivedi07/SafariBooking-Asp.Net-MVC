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
    public class TouristController : ControllerBase
    {
        private ITouristRepository repository;

        public TouristController(ITouristRepository repository)
        {
            this.repository = repository;
        }
        // GET: api/<TouristController>
        [HttpGet]
        public ActionResult Get()
        {
            try
            {
                var list = repository.GetAll();
                return Ok(list);
            }
            catch (JungleException ex)
            {

                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // GET api/<TouristController>/5
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            try
            {
                var emp = repository.Get(id);
                return Ok(emp);
            }
            catch (JungleException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // POST api/<TouristController>
        [HttpPost]
        public ActionResult Post([FromBody] Tourist tourist)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!TryValidateModel(tourist))
            {
                return BadRequest();
            }

            try
            {
                bool isAdded = repository.Add(tourist);
                if (isAdded)
                {
                    return Created("Tourist", tourist);
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError);
                }
            }
            catch (JungleException ex)
            {
                if (ex.Message == "Tourist already exist")
                {
                    return Conflict(ex.Message);
                }
              
                
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // PUT api/<TouristController>/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Tourist tourist)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != tourist.Id)
            {
                return BadRequest("Employee ids do not match");
            }
            try
            {
                bool isUpdated = repository.Update(tourist);
                if (isUpdated)
                {
                    return Ok("Tourist updated successfully");
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, "Tourist updated failed");
                }
            }
            catch (JungleException ex)
            {
                if (ex.Message == "Tourist already exist")
                {
                    return Conflict(ex.Message);
                }
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // DELETE api/<TouristController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var tourist = repository.Get(id);
                if (tourist == null)
                {
                    return NotFound();
                }
                bool isDeleted = repository.Remove(tourist);
                if (isDeleted)
                {
                    return Ok("Tourist deleted Successfully");
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError);
                }

            }
            catch (JungleException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpGet("TouristByMail/{emailid}")]
        public ActionResult GetTouristByEmail(string emailid)
        {
            try
            {
                var emp = repository.GetTouristByEmail(emailid);
                return Ok(emp);
            }
            catch (JungleException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}

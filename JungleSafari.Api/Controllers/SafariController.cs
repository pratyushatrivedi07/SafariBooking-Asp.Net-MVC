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
    public class SafariController : ControllerBase
    {
        private ISafariDetailRepos repository;

        public SafariController(ISafariDetailRepos repository)
        {
            this.repository = repository;
        }

        // GET: api/<SafariController>
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

        // GET api/<SafariController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var safari = repository.Get(id);
                return Ok(safari);
            }
            catch (JungleException ex)
            {

                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // POST api/<SafariController>
        [HttpPost]
        public IActionResult Post([FromBody] SafariDetail safari)
        {
            //SSV
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                bool isAdded = repository.Add(safari);
                if (isAdded)
                {
                    return Created("Safari", safari);
                }
                else
                {

                    return StatusCode((int)HttpStatusCode.InternalServerError);
                }
            }
            catch (JungleException ex)
            {
                if (ex.Message == "Safari already exists")
                {
                    return Conflict(ex.Message);
                    //return StatusCode((int)HttpStatusCode.Forbidden, ex.Message);
                }
                if (ex.Message == "Park Does Not Exists!")
                {
                    return NotFound(ex.Message);
                    // return StatusCode((int)HttpStatusCode.NotFound, ex.Message);
                }
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        // PUT api/<SafariController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] SafariDetail safari)
        {
            //ssv
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //check id
            if (id != safari.SafariId)
            {
                return BadRequest("No Safari Found ");
            }
            //repos.update(emp)
            //T -> Ok
            //F -> internal server error
            try
            {
                bool isUpdate = repository.Update(safari);
                if (isUpdate)
                {
                    return Ok("Safari Updated");
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, "Failed to Update Employee");
                }
            }
            catch (JungleException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // DELETE api/<SafariController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var safari = repository.Get(id);
                if (safari == null)
                {
                    return NotFound();
                }
                bool isDelete = repository.Remove(safari);
                if (isDelete)
                {
                    return Ok("Safari Deleted");
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, "Failed to Delete Employee");
                }
            }
            catch (JungleException ex)
            {

                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet("safari/{parkId}")]
        public IActionResult SafariByPark(int parkId)
        {
            try
            {
                var list = repository.SafariByPark(parkId);
                return Ok(list);
            }
            catch (JungleException ex)
            {

                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        //[HttpGet("safari/{parkId}")]
        //public IActionResult SafariByPark(int parkId)
        //{
        //    try
        //    {
        //        var list = repository.SafariByPark(parkId);
        //        return Ok(list);
        //    }
        //    catch (JungleException ex)
        //    {

        //        return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

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

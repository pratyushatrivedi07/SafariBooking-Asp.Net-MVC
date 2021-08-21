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
    public class IdentityProofController : ControllerBase
    {
        private IRepository<IdentityProof> repository;

        public IdentityProofController(IRepository<IdentityProof> repository ) 
            {
            this.repository = repository;
            } 
        // GET: api/<IdentityProofController>
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

        // GET api/<IdentityProofController>/5
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

        // POST api/<IdentityProofController>
        [HttpPost]
        public void Post([FromBody] string value)
        {

        }

        // PUT api/<IdentityProofController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<IdentityProofController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

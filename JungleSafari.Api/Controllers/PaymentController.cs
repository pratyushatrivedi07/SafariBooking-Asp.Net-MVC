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
    public class PaymentController : ControllerBase
    {
        private IRepository<Payment> repository;

        public PaymentController(IRepository<Payment> repository)
        {
            this.repository = repository;
        }
        // GET: api/<PaymentController>
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

        // GET api/<PaymentController>/5
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

        // POST api/<PaymentController>
        [HttpPost]
        public ActionResult Post([FromBody] Payment pay)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!TryValidateModel(pay))
            {
                return BadRequest();
            }

            try
            {
                bool isAdded = repository.Add(pay);
                if (isAdded)
                {
                    return Created("Payment", pay);
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError);
                }
            }
            catch (JungleException ex)
            {
                if (ex.Message == "Booking already exist")
                {
                    return Conflict(ex.Message);
                }
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // PUT api/<PaymentController>/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Payment pay)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != pay.PayId)
            {
                return BadRequest("Payment ids do not match");
            }
            try
            {
                bool isUpdated = repository.Update(pay);
                if (isUpdated)
                {
                    return Ok("Payment updated successfully");
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, "Payment updated failed");
                }
            }
            catch (JungleException ex)
            {
                if (ex.Message == "Payment already exist")
                {
                    return Conflict(ex.Message);
                }
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // DELETE api/<PaymentController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var pay = repository.Get(id);
                if (pay == null)
                {
                    return NotFound();
                }
                bool isDeleted = repository.Remove(pay);
                if (isDeleted)
                {
                    return Ok("Payment deleted Successfully");
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
    }
}

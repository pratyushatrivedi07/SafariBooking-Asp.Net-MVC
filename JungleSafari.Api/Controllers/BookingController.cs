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
    public class BookingController : ControllerBase
    {
        private IBookingRepository repository;

        public BookingController(IBookingRepository repository)
        {
            this.repository = repository;
        }
        // GET: api/<BookingController>
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

        // GET api/<BookingController>/5
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            try
            {
                var emp = repository.Get(id);
                if (emp != null)
                {
                    return Ok(emp);
                }
                else
                {
                    return BadRequest("No Booking Found");
                }
                
            }
            catch (JungleException ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // POST api/<BookingController>
        [HttpPost]
        public ActionResult Post([FromBody] Booking booked)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!TryValidateModel(booked))
            {
                return BadRequest();
            }

            try
            {
                bool isAdded = repository.Add(booked);
                if (isAdded)
                {
                    return Created("Booking", booked);
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


        // PUT api/<BookingController>/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Booking booked)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != booked.Id)
            {
                return BadRequest("Booking ids do not match");
            }
            try
            {
                bool isUpdated = repository.Update(booked);
                if (isUpdated)
                {
                    return Ok("Booking updated successfully");
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, "Booking updated failed");
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

        // DELETE api/<BookingController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var employee = repository.Get(id);
                if (employee == null)
                {
                    return NotFound();
                }
                bool isDeleted = repository.Remove(employee);
                if (isDeleted)
                {
                    return Ok("Booking deleted Successfully");
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

        [HttpGet("search/{Id}")]
        public ActionResult GetBookingById(int Id)
        {
            try
            {
                var list = repository.GetByBookingId(Id);
                return Ok(list);
            }
            catch (JungleException ex)
            {

                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet("BookingById")]
        public ActionResult GetBookingId()
        {
            try
            {
                var list = repository.GetBookingByID();
                return Ok(list);
            }
            catch (JungleException ex)
            {

                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

    }
}
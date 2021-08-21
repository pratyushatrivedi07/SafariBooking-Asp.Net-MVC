using Jungle.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Jungle.MVC.Api.Controllers
{
    public class BookingController : Controller
    {
        private IConfiguration configuration;
        private string apiUrl = null;

        public BookingController(IConfiguration configuration)
        {
            this.configuration = configuration;
            apiUrl = configuration["ApiUrl"];
        }

        // GET: BookingController
        public ActionResult Index()
        {
            IEnumerable<Booking> list = null;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(int id)
        {
            IEnumerable<Booking> elist = null;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    //HTTP GET 
                    var responseTask = client.GetAsync($"booking/search/{id}");  // PersonDetails is the WebApi controller name
                                                                                               // wait for task to complete
                    responseTask.Wait();
                    var result = responseTask.Result;
                    // check the status code for success
                    if (result.IsSuccessStatusCode)
                    {
                        // Add nuget package: Microsoft.AspNet.WebApi.Client
                        var readTask = result.Content.ReadAsAsync<IEnumerable<Booking>>();
                        readTask.Wait();
                        // fill the list vairable created above with the returned result
                        elist = readTask.Result;
                    }
                    else //web api sent error response 
                    {
                        elist = Enumerable.Empty<Booking>();
                        ModelState.AddModelError(string.Empty, "Server error.");
                    }
                }
            }
            catch (Exception ex)
            {
                elist = Enumerable.Empty<Booking>();
                ModelState.AddModelError(string.Empty, "Server error.");
            }
            return View(elist);
        }

        // GET: BookingController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: BookingController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BookingController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: BookingController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: BookingController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: BookingController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: BookingController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}

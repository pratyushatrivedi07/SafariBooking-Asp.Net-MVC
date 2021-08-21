using Jungle.Entities;
using Jungle.MVC.Api.Models;
using Microsoft.AspNetCore.Authorization;
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
    public class ParksController : Controller
    {
        private IConfiguration configuration;
        private string apiUrl = null;

        public ParksController(IConfiguration configuration)
        {
            this.configuration = configuration;
            apiUrl = configuration["ApiUrl"];
        }
        // GET: ParksController
        public ActionResult Index(double pg = 1)
        {
            IEnumerable<Parks> data = null;
            const int pageSize = 8;
            IEnumerable<Parks> plist = null;

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    //HTTP GET https://localhost:44380/api/parks
                    var responseTask = client.GetAsync("parks");  // PersonDetails is the WebApi controller name
                                                                  // wait for task to complete
                    responseTask.Wait();
                    var result = responseTask.Result;
                    // check the status code for success
                    if (result.IsSuccessStatusCode)
                    {
                        // Add nuget package: Microsoft.AspNet.WebApi.Client
                        var readTask = result.Content.ReadAsAsync<IEnumerable<Parks>>();
                        readTask.Wait();
                        // fill the list vairable created above with the returned result
                        plist = readTask.Result;

                        if (pg < 1)
                        {
                            pg = 1;
                        }

                        int recsCount = plist.Count();
                        var pager = new Pager(recsCount, (int)pg, pageSize);
                        int recSkip = ((int)pg - 1) * pageSize;
                        data = plist.Skip(recSkip).Take(pageSize).ToList();
                        this.ViewBag.Pager = pager;
                    }
                    else //web api sent error response 
                    {
                        plist = Enumerable.Empty<Parks>();
                        ModelState.AddModelError(string.Empty, "Server error.");
                    }
                }
            }
            catch (Exception ex)
            {
                plist = Enumerable.Empty<Parks>();
                ModelState.AddModelError(string.Empty, "Server error.");
            }
            return View(data);
        }

        // POST: EmployeesController
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string criteria)
        {
            //api -> search
            IEnumerable<Parks> list = null;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    //HTTP GET //api/employees/search/{criteria}
                    var responseTask = client.GetAsync($"parks/search/{criteria}");  // PersonDetails is the WebApi controller name
                                                                                         // wait for task to complete
                    responseTask.Wait();
                    var result = responseTask.Result;
                    // check the status code for success
                    if (result.IsSuccessStatusCode)
                    {
                        // Add nuget package: Microsoft.AspNet.WebApi.Client
                        var readTask = result.Content.ReadAsAsync<IEnumerable<Parks>>();
                        readTask.Wait();
                        // fill the list vairable created above with the returned result
                        list = readTask.Result;
                    }
                    else //web api sent error response 
                    {
                        list = Enumerable.Empty<Parks>();
                        ModelState.AddModelError(string.Empty, "Server error.");
                    }
                }
            }
            catch (Exception ex)
            {
                list = Enumerable.Empty<Parks>();
                ModelState.AddModelError(string.Empty, "Server error.");
            }
            return View(list);

        }

        // GET: ParksController/Details/5
        public ActionResult Details(int id)
        {
            Parks park = null;

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    //HTTP GET //api/employees
                    var responseTask = client.GetAsync($"parks/{id}");  // PersonDetails is the WebApi controller name
                                                                              // wait for task to complete
                    responseTask.Wait();
                    var result = responseTask.Result;
                    // check the status code for success
                    if (result.IsSuccessStatusCode)
                    {
                        // Add nuget package: Microsoft.AspNet.WebApi.Client
                        var readTask = result.Content.ReadAsAsync<Parks>();
                        readTask.Wait();
                        // fill the list vairable created above with the returned result
                        park = readTask.Result;
                    }
                    else //web api sent error response 
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(park);
        }

        // GET: ParksController/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            Parks park = new Parks();
            return View(park);
        }

        // POST: ParksController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(Parks park)
        {
            //User.Identity.GetUserId();
            //SSV
            if (!ModelState.IsValid)
            {
                return View(park);
            }
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    //HTTP POST// http://localhost:53225/api/parks
                    var responseTask = client.PostAsJsonAsync("parks", park);  // PersonDetails is the WebApi controller name
                                                                                  // wait for task to complete
                    responseTask.Wait();
                    var result = responseTask.Result;
                    // check the status code for success
                    if (result.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "Park Added";
                        return RedirectToAction(nameof(Index));
                    }
                    else //web api sent error response 
                    {

                        ModelState.AddModelError(string.Empty, "Server error.");
                        return View(park);
                    }
                }
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Server error.");
                return View(park);
            }
        }

        // GET: ParksController/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            Parks park = null;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    //HTTP GET// http://localhost:53225/api/parks/id
                    var responseTask = client.GetAsync($"parks/{id}");  // PersonDetails is the WebApi controller name
                                                                              // wait for task to complete
                    responseTask.Wait();
                    var result = responseTask.Result;
                    // check the status code for success
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<Parks>();
                        readTask.Wait();
                        park = readTask.Result;
                    }
                    else //web api sent error response 
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }

            Parks p = new Parks
            {
                ParkId = park.ParkId,
                Name = park.Name,
                Location = park.Location,
                Fee = park.Fee
            };
            return View(p);
        }

        // POST: ParksController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id, Parks park)
        {
            park.ParkId = id;
            //ssv
            if (!ModelState.IsValid)
            {
                return View(park);
            }

            //id
            if (id != park.ParkId)
            {
                return RedirectToAction(nameof(Index));
            }

            Parks p = new Parks
            {
                ParkId = park.ParkId,
                Name = park.Name,
                Location = park.Location,
                Fee = park.Fee
            };

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    //HTTP Put// http://localhost:53225/api/parks
                    var responseTask = client.PutAsJsonAsync($"parks/{p.ParkId}", p);  // PersonDetails is the WebApi controller name
                                                                                         // wait for task to complete
                    responseTask.Wait();
                    var result = responseTask.Result;
                    // check the status code for success
                    if (result.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "Park Updated";
                        return RedirectToAction(nameof(Index));
                    }
                    else //web api sent error response 
                    {

                        ModelState.AddModelError(string.Empty, "Server error.");
                        return View(park);
                    }
                }
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Server error.");
                return View(park);
            }
        }

        // GET: ParksController/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            //api -> delete -> Index
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    //HTTP POST// http://localhost:53225/api/parks
                    var responseTask = client.DeleteAsync($"parks/{id}");  // PersonDetails is the WebApi controller name
                                                                               // wait for task to complete
                    responseTask.Wait();
                    var result = responseTask.Result;
                    // check the status code for success
                    if (result.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "Park Deleted";
                        return RedirectToAction(nameof(Index));
                    }
                    else //web api sent error response 
                    {

                        ModelState.AddModelError(string.Empty, "Server error.");
                        return View();
                    }
                }
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Server error.");
                return View();
            }
        }
    }
}

using Jungle.Entities;
using Jungle.MVC.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Jungle.MVC.Api.Controllers
{
    public class GateController : Controller
    {
        private IConfiguration configuration;
        private string apiUrl = null;

        public GateController(IConfiguration configuration)
        {
            this.configuration = configuration;
            apiUrl = configuration["ApiUrl"];
        }
        // GET: GateController
        [Authorize(Roles = "Admin")]
        public ActionResult Index(double pg = 1)
        {
            IEnumerable<Gate> data = null;
            IEnumerable<Parks> list = GetParks();
            ViewBag.ParkId = new SelectList(list, "ParkId", "Name");

            IEnumerable<Gate> glist = null;
            const int pageSize = 6;

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    //HTTP GET //api/employees
                    var responseTask = client.GetAsync("gate");  // PersonDetails is the WebApi controller name
                                                                 // wait for task to complete
                    responseTask.Wait();
                    var result = responseTask.Result;
                    // check the status code for success
                    if (result.IsSuccessStatusCode)
                    {
                        // Add nuget package: Microsoft.AspNet.WebApi.Client
                        var readTask = result.Content.ReadAsAsync<IEnumerable<Gate>>();
                        readTask.Wait();
                        // fill the list vairable created above with the returned result
                        glist = readTask.Result;
                        if (pg < 1)
                        {
                            pg = 1;
                        }
                        int recsCount = glist.Count();
                        var pager = new Pager(recsCount, (int)pg, pageSize);
                        int recSkip = ((int)pg - 1) * pageSize;
                        data = glist.Skip(recSkip).Take(pageSize).ToList();
                        this.ViewBag.Pager = pager;
                    }
                    else //web api sent error response 
                    {
                        glist = Enumerable.Empty<Gate>();
                        ModelState.AddModelError(string.Empty, "Server error.");
                    }
                }
            }
            catch (Exception ex)
            {
                glist = Enumerable.Empty<Gate>();
                ModelState.AddModelError(string.Empty, "Server error.");
            }
            return View(glist);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Index(int park)
        {
            IEnumerable<Parks> list = GetParks();
            ViewBag.ParkId = new SelectList(list, "ParkId", "Name");

            IEnumerable<Gate> vlist = null;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    //HTTP GET 
                    var responseTask = client.GetAsync($"gate/parks/{park}");  // PersonDetails is the WebApi controller name
                                                                               // wait for task to complete
                    responseTask.Wait();
                    var result = responseTask.Result;
                    // check the status code for success
                    if (result.IsSuccessStatusCode)
                    {
                        // Add nuget package: Microsoft.AspNet.WebApi.Client
                        var readTask = result.Content.ReadAsAsync<IEnumerable<Gate>>();
                        readTask.Wait();
                        // fill the list vairable created above with the returned result
                        vlist = readTask.Result;
                    }
                    else //web api sent error response 
                    {
                        vlist = Enumerable.Empty<Gate>();
                        ModelState.AddModelError(string.Empty, "Server error.");
                    }
                }
            }
            catch (Exception ex)
            {
                vlist = Enumerable.Empty<Gate>();
                ModelState.AddModelError(string.Empty, "Server error.");
            }
            return View(vlist);
        }


        // GET: GateController/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            IEnumerable<Parks> list = GetParks();
            ViewBag.ParkId = new SelectList(list, "ParkId", "Name");

            Gate gate = new Gate();
            return View(gate);
        }

        // POST: GateController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(Gate gate)
        {
            //User.Identity.GetUserId();
            //SSV
            if (!ModelState.IsValid)
            {
                return View(gate);
            }
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    //HTTP POST// http://localhost:53225/api/parks
                    var responseTask = client.PostAsJsonAsync("gate", gate);  // PersonDetails is the WebApi controller name
                                                                              // wait for task to complete
                    responseTask.Wait();
                    var result = responseTask.Result;
                    // check the status code for success
                    if (result.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "Gate Added";
                        return RedirectToAction(nameof(Index));
                    }
                    else //web api sent error response 
                    {

                        ModelState.AddModelError(string.Empty, "Server error.");
                        return View(gate);
                    }
                }
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Server error.");
                return View(gate);
            }
        }

        // GET: GateController/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            IEnumerable<Parks> list = GetParks();
            ViewBag.ParkId = new SelectList(list, "ParkId", "Name");

            Gate gate = null;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    //HTTP GET// http://localhost:53225/api/parks/id
                    var responseTask = client.GetAsync($"gate/{id}");  // PersonDetails is the WebApi controller name
                                                                       // wait for task to complete
                    responseTask.Wait();
                    var result = responseTask.Result;
                    // check the status code for success
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<Gate>();
                        readTask.Wait();
                        gate = readTask.Result;
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

            Gate g = new Gate
            {
                GateId = gate.GateId,
                Name = gate.Name,
                ParkId = gate.ParkId
            };
            return View(g);
        }

        // POST: GateController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id, Gate gate)
        {
            gate.GateId = id;
            IEnumerable<Parks> list = GetParks();
            ViewBag.ParkId = new SelectList(list, "ParkId", "Name");
            //ssv
            if (!ModelState.IsValid)
            {
                return View(gate);
            }
            //id
            if (id != gate.GateId)
            {
                return RedirectToAction(nameof(Index));
            }

            Gate g = new Gate
            {
                GateId = gate.GateId,
                Name = gate.Name,
                ParkId = gate.ParkId
            };

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    //HTTP Put// http://localhost:53225/api/employees
                    var responseTask = client.PutAsJsonAsync($"gate/{g.GateId}", g);  // PersonDetails is the WebApi controller name
                                                                                      // wait for task to complete
                    responseTask.Wait();
                    var result = responseTask.Result;
                    // check the status code for success
                    if (result.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "Gate Updated";
                        return RedirectToAction(nameof(Index));
                    }
                    else //web api sent error response 
                    {

                        ModelState.AddModelError(string.Empty, "Server error.");
                        return View(gate);
                    }
                }
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Server error.");
                return View(gate);
            }
        }

        // GET: GateController/Delete/5
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
                    var responseTask = client.DeleteAsync($"gate/{id}");  // PersonDetails is the WebApi controller name
                                                                          // wait for task to complete
                    responseTask.Wait();
                    var result = responseTask.Result;
                    // check the status code for success
                    if (result.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "Gate Deleted";
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

        private IEnumerable<Parks> GetParks()
        {
            //dept list
            IEnumerable<Parks> list = null;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    //HTTP GET// http://localhost:53225/api/departments
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
            ViewData["Park"] = new SelectList(list, "ParkId", "Name");
            return list;
        }
    }
}

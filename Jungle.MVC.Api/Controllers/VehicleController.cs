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
    public class VehicleController : Controller
    {
        private IConfiguration configuration;
        private string apiUrl = null;

        public VehicleController(IConfiguration configuration)
        {
            this.configuration = configuration;
            apiUrl = configuration["ApiUrl"];
        }
        // GET: VehicleController
        [Authorize(Roles = "Admin")]
        public ActionResult Index(double pg = 1)
        {
            IEnumerable<Vehicle> data = null;
            const int pageSize = 6;
            IEnumerable<Parks> list = GetParks();
            ViewBag.ParkId = new SelectList(list, "ParkId", "Name");

            IEnumerable<Vehicle> vlist = null;

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    //HTTP GET //api/employees
                    var responseTask = client.GetAsync("vehicle");  // PersonDetails is the WebApi controller name
                                                                    // wait for task to complete
                    responseTask.Wait();
                    var result = responseTask.Result;
                    // check the status code for success
                    if (result.IsSuccessStatusCode)
                    {
                        // Add nuget package: Microsoft.AspNet.WebApi.Client
                        var readTask = result.Content.ReadAsAsync<IEnumerable<Vehicle>>();
                        readTask.Wait();
                        // fill the list vairable created above with the returned result
                        vlist = readTask.Result;
                        if (pg < 1)
                        {
                            pg = 1;
                        }

                        int recsCount = vlist.Count();
                        var pager = new Pager(recsCount, (int)pg, pageSize);
                        int recSkip = ((int)pg - 1) * pageSize;
                        data = vlist.Skip(recSkip).Take(pageSize).ToList();
                        this.ViewBag.Pager = pager;
                    }
                    else //web api sent error response 
                    {
                        vlist = Enumerable.Empty<Vehicle>();
                        ModelState.AddModelError(string.Empty, "Server error.");
                    }
                }
            }
            catch (Exception ex)
            {
                vlist = Enumerable.Empty<Vehicle>();
                ModelState.AddModelError(string.Empty, "Server error.");
            }
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Index(int park)
        {
            IEnumerable<Parks> list = GetParks();
            ViewBag.ParkId = new SelectList(list, "ParkId", "Name");

            IEnumerable<Vehicle> vlist = null;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    //HTTP GET 
                    var responseTask = client.GetAsync($"vehicle/parks/{park}");  // PersonDetails is the WebApi controller name
                                                                                  // wait for task to complete
                    responseTask.Wait();
                    var result = responseTask.Result;
                    // check the status code for success
                    if (result.IsSuccessStatusCode)
                    {
                        // Add nuget package: Microsoft.AspNet.WebApi.Client
                        var readTask = result.Content.ReadAsAsync<IEnumerable<Vehicle>>();
                        readTask.Wait();
                        // fill the list vairable created above with the returned result
                        vlist = readTask.Result;
                    }
                    else //web api sent error response 
                    {
                        vlist = Enumerable.Empty<Vehicle>();
                        ModelState.AddModelError(string.Empty, "Server error.");
                    }
                }
            }
            catch (Exception ex)
            {
                vlist = Enumerable.Empty<Vehicle>();
                ModelState.AddModelError(string.Empty, "Server error.");
            }
            return View(vlist);
        }

        // GET: VehicleController/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int id)
        {
            Vehicle vehicle = null;

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    //HTTP GET //api/employees
                    var responseTask = client.GetAsync($"vehicle/{id}");  // PersonDetails is the WebApi controller name
                                                                        // wait for task to complete
                    responseTask.Wait();
                    var result = responseTask.Result;
                    // check the status code for success
                    if (result.IsSuccessStatusCode)
                    {
                        // Add nuget package: Microsoft.AspNet.WebApi.Client
                        var readTask = result.Content.ReadAsAsync<Vehicle>();
                        readTask.Wait();
                        // fill the list vairable created above with the returned result
                        vehicle = readTask.Result;
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
            return View(vehicle);
        }

        // GET: VehicleController/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            IEnumerable<Parks> list = GetParks();
            ViewBag.ParkId = new SelectList(list, "ParkId", "Name");

            VehicleViewModel vehicleVM = new VehicleViewModel();
            return View(vehicleVM);
        }

        // POST: VehicleController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(VehicleViewModel vehicleVM)
        {
            IEnumerable<Parks> list = GetParks();
            ViewBag.ParkId = new SelectList(list, "ParkId", "Name");
            //SSV
            if (!ModelState.IsValid)
            {
                return View(vehicleVM);
            }
            Vehicle v = new Vehicle
            {
                Name = vehicleVM.Name,
                Vtype = vehicleVM.Vtype,
                Capacity = vehicleVM.Capacity.ToString(),
                EntryCost = vehicleVM.EntryCost,
                ParkId = vehicleVM.ParkId
            };
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    //HTTP POST// http://localhost:53225/api/safari
                    var responseTask = client.PostAsJsonAsync("vehicle", v);  // PersonDetails is the WebApi controller name
                                                                              // wait for task to complete
                    responseTask.Wait();
                    var result = responseTask.Result;
                    // check the status code for success
                    if (result.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "Vehicle Added";
                        return RedirectToAction(nameof(Index));
                    }
                    else //web api sent error response 
                    {

                        ModelState.AddModelError(string.Empty, "Server error.");
                        return View(vehicleVM);
                    }
                }
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Server error.");
                return View(vehicleVM);
            }
        }

        // GET: VehicleController/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            IEnumerable<Parks> list = GetParks();
            ViewBag.ParkId = new SelectList(list, "ParkId", "Name");

            Vehicle vehicle = null;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    //HTTP POST// http://localhost:53225/api/employees/id
                    var responseTask = client.GetAsync($"vehicle/{id}");  // PersonDetails is the WebApi controller name
                                                                          // wait for task to complete
                    responseTask.Wait();
                    var result = responseTask.Result;
                    // check the status code for success
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<Vehicle>();
                        readTask.Wait();
                        vehicle = readTask.Result;
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
            //tranform Emp -> EmpVM

            VehicleViewModel vehicleVM = new VehicleViewModel
            {
                Vid = vehicle.Vid,
                Name = vehicle.Name,
                Capacity = (capacity)Enum.Parse(typeof(capacity), vehicle.Capacity),
                EntryCost = vehicle.EntryCost,
                Vtype = vehicle.Vtype,
                ParkId = vehicle.ParkId
            };

            return View(vehicleVM);
        }

        // POST: VehicleController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id, VehicleViewModel vehicleVM)
        {
            vehicleVM.Vid = id;
            IEnumerable<Parks> list = GetParks();
            ViewBag.ParkId = new SelectList(list, "ParkId", "Name");
            //ssv
            if (!ModelState.IsValid)
            {
                return View(vehicleVM);
            }
            //id
            if (id != vehicleVM.Vid)
            {
                return RedirectToAction(nameof(Index));
            }

            Vehicle vehicle = new Vehicle
            {
                Vid = vehicleVM.Vid,
                Name = vehicleVM.Name,
                Capacity = vehicleVM.Capacity.ToString(),
                EntryCost = vehicleVM.EntryCost,
                Vtype = vehicleVM.Vtype,
                ParkId = vehicleVM.ParkId
            };

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    //HTTP Put// http://localhost:53225/api/employees
                    var responseTask = client.PutAsJsonAsync($"vehicle/{vehicle.Vid}", vehicle);  // PersonDetails is the WebApi controller name
                                                                                                  // wait for task to complete
                    responseTask.Wait();
                    var result = responseTask.Result;
                    // check the status code for success
                    if (result.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "Vehicle Updated";
                        return RedirectToAction(nameof(Index));
                    }
                    else //web api sent error response 
                    {

                        ModelState.AddModelError(string.Empty, "Server error.");
                        return View(vehicleVM);
                    }
                }
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Server error.");
                return View(vehicleVM);
            }
        }

        // GET: VehicleController/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    //HTTP POST// http://localhost:53225/api/employees
                    var responseTask = client.DeleteAsync($"vehicle/{id}");  // PersonDetails is the WebApi controller name
                                                                             // wait for task to complete
                    responseTask.Wait();
                    var result = responseTask.Result;
                    // check the status code for success
                    if (result.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "Vehicle Deleted";
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

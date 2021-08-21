using Jungle.Entities;
using Jungle.MVC.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Jungle.MVC.Api.Controllers
{
    public class SafariController : Controller
    {
        private IConfiguration configuration;
        private string apiUrl = null;

        public SafariController(IConfiguration configuration)
        {
            this.configuration = configuration;
            apiUrl = configuration["ApiUrl"];
        }


        // GET: SafariController
        public ActionResult Index(double pg = 1)
        {
            IEnumerable<SafariDetail> data = null;
            const int pageSize = 6;
            IEnumerable<Parks> list = GetParks();
            ViewBag.ParkId = new SelectList(list, "ParkId", "Name");
            IEnumerable<SafariDetail> slist = null;

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    //HTTP GET //api/employees
                    var responseTask = client.GetAsync("safari");  // PersonDetails is the WebApi controller name
                                                                   // wait for task to complete
                    responseTask.Wait();
                    var result = responseTask.Result;
                    // check the status code for success
                    if (result.IsSuccessStatusCode)
                    {
                        // Add nuget package: Microsoft.AspNet.WebApi.Client
                        var readTask = result.Content.ReadAsAsync<IEnumerable<SafariDetail>>();
                        readTask.Wait();
                        // fill the list vairable created above with the returned result
                        slist = readTask.Result;
                        if (pg < 1)
                        {
                            pg = 1;
                        }

                        int recsCount = slist.Count();
                        var pager = new Pager(recsCount, (int)pg, pageSize);
                        int recSkip = ((int)pg - 1) * pageSize;
                        data = slist.Skip(recSkip).Take(pageSize).ToList();
                        this.ViewBag.Pager = pager;

                    }
                    else //web api sent error response 
                    {
                        slist = Enumerable.Empty<SafariDetail>();
                        ModelState.AddModelError(string.Empty, "Server error.");

                    }

                }
            }
            catch (Exception ex)
            {
                slist = Enumerable.Empty<SafariDetail>();
                ModelState.AddModelError(string.Empty, "Server error.");
            }
            return View(data);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(int park)
        {
            IEnumerable<Parks> list = GetParks();
            ViewBag.ParkId = new SelectList(list, "ParkId", "Name");

            IEnumerable<SafariDetail> vlist = null;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    //HTTP GET 
                    var responseTask = client.GetAsync($"safari/parks/{park}");  // PersonDetails is the WebApi controller name
                                                                                 // wait for task to complete
                    responseTask.Wait();
                    var result = responseTask.Result;
                    // check the status code for success
                    if (result.IsSuccessStatusCode)
                    {
                        // Add nuget package: Microsoft.AspNet.WebApi.Client
                        var readTask = result.Content.ReadAsAsync<IEnumerable<SafariDetail>>();
                        readTask.Wait();
                        // fill the list vairable created above with the returned result
                        vlist = readTask.Result;
                    }
                    else //web api sent error response 
                    {
                        vlist = Enumerable.Empty<SafariDetail>();
                        ModelState.AddModelError(string.Empty, "Server error.");
                    }
                }
            }
            catch (Exception ex)
            {
                vlist = Enumerable.Empty<SafariDetail>();
                ModelState.AddModelError(string.Empty, "Server error.");
            }
            return View(vlist);
        }


        // GET: SafariController/Details/5
        public ActionResult Details(int id)
        {
            SafariDetail safari = null;

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    //HTTP GET //api/employees
                    var responseTask = client.GetAsync($"safari/{id}");  // PersonDetails is the WebApi controller name
                                                                         // wait for task to complete
                    responseTask.Wait();
                    var result = responseTask.Result;
                    // check the status code for success
                    if (result.IsSuccessStatusCode)
                    {
                        // Add nuget package: Microsoft.AspNet.WebApi.Client
                        var readTask = result.Content.ReadAsAsync<SafariDetail>();
                        readTask.Wait();
                        // fill the list vairable created above with the returned result
                        safari = readTask.Result;
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
            return View(safari);
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

        // GET: SafariController/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            IEnumerable<Parks> list = null;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    //HTTP GET// http://localhost:53225/api/parks
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
            ViewBag.ParkId = new SelectList(list, "ParkId", "Name");
            SafariViewModel safariVM = new SafariViewModel();
            return View(safariVM);
        }

        // POST: SafariController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(SafariViewModel safariVM)
        {
            //User.Identity.GetUserId();
            IEnumerable<Parks> list = GetParks();
            ViewBag.ParkId = new SelectList(list, "ParkId", "Name");
            //SSV
            if (!ModelState.IsValid)
            {
                return View(safariVM);
            }

            if (safariVM.SafariDate < DateTime.Now)
            {
                ModelState.AddModelError(string.Empty, "Available Date should be in future");
                return View(safariVM);
            }

            SafariDetail safari = new SafariDetail
            {
                SafariName = safariVM.SafariName,
                SafariDate = safariVM.SafariDate,
                SafariTime = Convert.ToString(safariVM.SafariTime),
                SafariCost = safariVM.SafariCost,
                ParkId = safariVM.ParkId
            };
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    //HTTP POST// http://localhost:53225/api/safari
                    var responseTask = client.PostAsJsonAsync("safari", safari);  // PersonDetails is the WebApi controller name
                                                                                  // wait for task to complete
                    responseTask.Wait();
                    var result = responseTask.Result;
                    // check the status code for success
                    if (result.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "Safari Added";
                        return RedirectToAction(nameof(Index));
                    }
                    else //web api sent error response 
                    {

                        ModelState.AddModelError(string.Empty, "Server error.");
                        return View(safariVM);
                    }
                }
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Server error.");
                return View(safariVM);
            }
        }

        // GET: SafariController/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            IEnumerable<Parks> list = GetParks();
            ViewData["Park"] = new SelectList(list, "ParkId", "Name");

            SafariDetail safari = null;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    //HTTP POST// http://localhost:53225/api/employees/id
                    var responseTask = client.GetAsync($"safari/{id}");  // PersonDetails is the WebApi controller name
                                                                         // wait for task to complete
                    responseTask.Wait();
                    var result = responseTask.Result;
                    // check the status code for success
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<SafariDetail>();
                        readTask.Wait();
                        safari = readTask.Result;
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
            SafariViewModel safariVM = new SafariViewModel
            {
                SafariId = safari.SafariId,
                SafariName = safari.SafariName,
                SafariDate = safari.SafariDate,
                SafariTime = (Slot)Enum.Parse(typeof(Slot), safari.SafariTime),
                SafariCost = safari.SafariCost,
                ParkId = safari.ParkId
            };
            return View(safariVM);
        }

        // POST: SafariController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id, SafariViewModel safariVM)
        {
            safariVM.SafariId = id;
            IEnumerable<Parks> list = GetParks();
            ViewData["Park"] = new SelectList(list, "ParkId", "Name");
            //ssv
            if (!ModelState.IsValid)
            {
                return View(safariVM);
            }
            //id
            if (id != safariVM.SafariId)
            {
                return RedirectToAction(nameof(Index));
            }

            if (safariVM.SafariDate < DateTime.Now)
            {
                ModelState.AddModelError(string.Empty, "Available Date should be in future");
                return View(safariVM);
            }

            SafariDetail safari = new SafariDetail
            {
                SafariId = safariVM.SafariId,
                SafariName = safariVM.SafariName,
                SafariCost = safariVM.SafariCost,
                SafariDate = safariVM.SafariDate,
                SafariTime = Convert.ToString(safariVM.SafariTime),
                ParkId = safariVM.ParkId
            };

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    //HTTP Put// http://localhost:53225/api/employees
                    var responseTask = client.PutAsJsonAsync($"safari/{safari.SafariId}", safari);  // PersonDetails is the WebApi controller name
                                                                                                    // wait for task to complete
                    responseTask.Wait();
                    var result = responseTask.Result;
                    // check the status code for success
                    if (result.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "Safari Updated";
                        return RedirectToAction(nameof(Index));
                    }
                    else //web api sent error response 
                    {

                        ModelState.AddModelError(string.Empty, "Server error.");
                        return View(safariVM);
                    }
                }
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Server error.");
                return View(safariVM);
            }
        }

        // GET: SafariController/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            //api -> delete -> Index
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    //HTTP POST// http://localhost:53225/api/employees
                    var responseTask = client.DeleteAsync($"safari/{id}");  // PersonDetails is the WebApi controller name
                                                                            // wait for task to complete
                    responseTask.Wait();
                    var result = responseTask.Result;
                    // check the status code for success
                    if (result.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "Safari Deleted";
                        return RedirectToAction(nameof(Index));
                    }
                    else //web api sent error response 
                    {

                        ModelState.AddModelError(string.Empty, "Server error.");
                        TempData["Message"] = "Cannot Deleted as there as booking against this Safari";
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Server error.");
                return View();
            }
        }

        [Authorize(Roles = "Tourist")]
        public ActionResult Book(int Pid, int Sid)
        {
            //----------------Gate Drop Down List-----------------------------------------------//
            IEnumerable<Gate> list = null;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    //HTTP GET// http://localhost:53225/api/gate
                    var responseTask = client.GetAsync($"Gate/parks/{Pid}");  // PersonDetails is the WebApi controller name
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
                        list = readTask.Result;
                    }
                    else //web api sent error response 
                    {
                        list = Enumerable.Empty<Gate>();
                        ModelState.AddModelError(string.Empty, "Server error.");
                    }
                }
            }
            catch (Exception ex)
            {
                list = Enumerable.Empty<Gate>();
                ModelState.AddModelError(string.Empty, "Server error.");
            }
            ViewBag.GateId = new SelectList(list, "GateId", "Name");
            //----------------------------------------------------------------------------------//
            //---------------Getting ParkInfo based on selected parkid-------------------------//
            Parks parks = null;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    //HTTP GET// http://localhost:53225/api/parks
                    var responseTask = client.GetAsync($"Parks/{Pid}");  // PersonDetails is the WebApi controller name
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
                        parks = readTask.Result;
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
            //ViewBag.ParkId = (list,"ParkId");
            //----------------------------------------------------------------------------------//

            //-----------------------Getting SafariInfo based on selected Safariid-------------------//

            SafariDetail safari = null;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    //HTTP GET// http://localhost:53225/api/parks
                    var responseTask = client.GetAsync($"Safari/{Sid}");  // PersonDetails is the WebApi controller name
                                                                          // wait for task to complete
                    responseTask.Wait();
                    var result = responseTask.Result;
                    // check the status code for success
                    if (result.IsSuccessStatusCode)
                    {
                        // Add nuget package: Microsoft.AspNet.WebApi.Client
                        var readTask = result.Content.ReadAsAsync<SafariDetail>();
                        readTask.Wait();
                        // fill the list vairable created above with the returned result
                        safari = readTask.Result;
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
            //ViewBag.SafariId =(safari,"SafariId");
            //----------------------------------------------------------------------------------//
            //----------------------------Getting Vehicle List For based on Park id--------------//

            IEnumerable<IdentityProof> identity = null;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    //HTTP GET// http://localhost:53225/api/parks
                    var responseTask = client.GetAsync("IdentityProof");  // PersonDetails is the WebApi controller name
                                                                          // wait for task to complete
                    responseTask.Wait();
                    var result = responseTask.Result;
                    // check the status code for success
                    if (result.IsSuccessStatusCode)
                    {
                        // Add nuget package: Microsoft.AspNet.WebApi.Client
                        var readTask = result.Content.ReadAsAsync<IEnumerable<IdentityProof>>();
                        readTask.Wait();
                        // fill the list vairable created above with the returned result
                        identity = readTask.Result;
                    }
                    else //web api sent error response 
                    {
                        identity = Enumerable.Empty<IdentityProof>();
                        ModelState.AddModelError(string.Empty, "Server error.");
                    }
                }
            }
            catch (Exception ex)
            {
                identity = Enumerable.Empty<IdentityProof>();
                ModelState.AddModelError(string.Empty, "Server error.");
            }
            ViewBag.IdentityId = new SelectList(identity, "IdentityId", "IdentityName");
            //----------------------------------------------------------------------------------//
            IEnumerable<VehicleViewModel> vehicle = null;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    //HTTP GET// http://localhost:53225/api/gate
                    var responseTask = client.GetAsync($"vehicle/parks/{Pid}");  // PersonDetails is the WebApi controller name
                                                                                 // wait for task to complete
                    responseTask.Wait();
                    var result = responseTask.Result;
                    // check the status code for success
                    if (result.IsSuccessStatusCode)
                    {
                        // Add nuget package: Microsoft.AspNet.WebApi.Client
                        var readTask = result.Content.ReadAsAsync<IEnumerable<VehicleViewModel>>();
                        readTask.Wait();
                        // fill the list vairable created above with the returned result
                        vehicle = readTask.Result;
                    }
                    else //web api sent error response 
                    {
                        vehicle = Enumerable.Empty<VehicleViewModel>();
                        ModelState.AddModelError(string.Empty, "Server error.");
                    }
                }
            }
            catch (Exception ex)
            {
                vehicle = Enumerable.Empty<VehicleViewModel>();
                ModelState.AddModelError(string.Empty, "Server error.");
            }
            ViewBag.VId = new SelectList(vehicle, "Vid", "Name", "Capacity");

            //----------------------------------------------------------------------------------//

            //-----------------------Get identityproof dropdownlist----------------------------//
            BookingViewModel booking = new BookingViewModel
            {
                ParkId = parks.ParkId,
                ParkName = parks.Name,
                SafariId = safari.SafariId,
                SafariName = safari.SafariName
            };

            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Tourist")]
        public ActionResult Book(BookingViewModel bookingModel)
        {
            if (!ModelState.IsValid)
            {
                return View(bookingModel);
            }

            Tourist tourist = new Tourist()
            {
                Name = bookingModel.Name,
                Gender = bookingModel.Gender.ToString(),
                DateOfBirth = bookingModel.DateOfBirth,
                MobileNo = bookingModel.MobileNo.ToString(),
                City = bookingModel.City,
                Country = bookingModel.Country,
                EmailId = bookingModel.EmailId,
                IdentityName = bookingModel.Identityproof,
                IdentityNumber = bookingModel.Identitynumber
            };

            decimal totalcost = Gettotalcost(bookingModel.People, bookingModel.ParkId, bookingModel.SafariId, bookingModel.VId);
            Booking booked = new Booking()
            {
                Pid = bookingModel.ParkId,
                SafariId = bookingModel.SafariId,
                GateId = bookingModel.gateId,
                VehicleId = bookingModel.VId,

            };
            booked.TotalCost = totalcost;

            bool isAdded = AddTourist(tourist);
            if (isAdded)
            {
                bool isBooked = AddBooking(booked);
                bookingModel.BookingId = GetBookingId();
                if (isBooked)
                {
                    TempData["BookingObject"] = JsonConvert.SerializeObject(bookingModel);
                    return RedirectToAction("GetPayment", "Safari");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error.");
                    return View(bookingModel);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Server error.");
                return View(bookingModel);
            }

        }

        [Authorize(Roles = "Tourist")]
        public ActionResult GetSearchById()
        {
            BookingViewModel bookingVM = null;
            return View(bookingVM);
        }

        [HttpPost]
        [Authorize(Roles = "Tourist")]
        public ActionResult GetSearchById(int Id)
        {
            Booking booking = GetById(Id);
            if (booking == null)
            {
                TempData["Message"] = "No Booking Found";
                return RedirectToAction(nameof(GetSearchById));
            }
            else
            {
                Parks parks = GetParkById(booking.Pid);
                Gate gate = GetGateById(booking.GateId);
                SafariDetail safariDetail = GetSafariById(booking.SafariId);
                Vehicle vehicle = GetVehicleById(booking.VehicleId);

                BookingViewModel bookingViewModel = new BookingViewModel()
                {
                    ParkId = parks.ParkId,
                    ParkName = parks.Name,
                    SafariId = safariDetail.SafariId,
                    SafariName = safariDetail.SafariName,
                    gateId = gate.GateId,
                    GateName = gate.Name,
                    VId = vehicle.Vid,
                    VName = vehicle.Name,
                    TotalCost = booking.TotalCost,
                    BookingId = booking.Id

                };

                return View(bookingViewModel);
            }

        }

        [Authorize(Roles = "Tourist")]
        public ActionResult DeleteBooking(int id)
        {
            //api -> delete -> Index
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    //HTTP POST// http://localhost:53225/api/employees
                    var responseTask = client.DeleteAsync($"booking/{id}");  // PersonDetails is the WebApi controller name
                                                                             // wait for task to complete
                    responseTask.Wait();
                    var result = responseTask.Result;
                    // check the status code for success
                    if (result.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "Our Animals are sad to see you Go! :(";
                        return RedirectToAction(nameof(Index));
                    }
                    else //web api sent error response 
                    {

                        ModelState.AddModelError(string.Empty, "Server error.");
                        TempData["Message"] = "Could not be Deleted";
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Server error.");
                return View();
            }
        }

        //-----------------------------------------------Misc Methods------------------------------------------------------

        public ActionResult GetPayment()
        {
            BookingViewModel booked = JsonConvert.DeserializeObject<BookingViewModel>((string)TempData["BookingObject"]);
            Parks park = GetParkById(booked.ParkId);
            SafariDetail safari = GetSafariById(booked.SafariId);
            Vehicle vehicle = GetVehicleById(booked.VId);
            Tourist tourist = GetTouristByMail(booked.EmailId);
            Gate gate = GetGateById(booked.gateId);
            Payment p = new Payment()
            {
                BookingId = booked.BookingId,
                ParkId = park.ParkId,
                ParkName = park.Name,
                ParkCost = park.Fee,
                SafariId = safari.SafariId,
                SafariName = safari.SafariName,
                SafariCost = safari.SafariCost,
                VehicleId = vehicle.Vid,
                VehicleName = vehicle.Name,
                VehicleCost = vehicle.EntryCost,
                TouristName = tourist.Name,
                Email = tourist.EmailId,
                MobileNo = tourist.MobileNo,
                People = booked.People,
                GateId = booked.gateId,
                GateName = gate.Name
                
            };
            p.Total = Gettotalcost(booked.People, booked.ParkId, booked.SafariId, booked.VId);
            SendEmail(tourist.EmailId, p);
            return View(p);
        }

        private Tourist GetTouristByMail(string emailId)
        {
            Tourist tourist = null;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    //HTTP GET// http://localhost:53225/api/parks
                    var responseTask = client.GetAsync($"Tourist/TouristByMail/{emailId}");  // PersonDetails is the WebApi controller name
                                                                                             // wait for task to complete
                    responseTask.Wait();
                    var result = responseTask.Result;
                    // check the status code for success
                    if (result.IsSuccessStatusCode)
                    {
                        // Add nuget package: Microsoft.AspNet.WebApi.Client
                        var readTask = result.Content.ReadAsAsync<Tourist>();
                        readTask.Wait();
                        // fill the list vairable created above with the returned result
                        tourist = readTask.Result;
                    }
                    else //web api sent error response 
                    {
                        tourist = (Tourist)Enumerable.Empty<Tourist>();
                        ModelState.AddModelError(string.Empty, "Server error.");
                    }
                }
            }
            catch (Exception ex)
            {
                tourist = (Tourist)Enumerable.Empty<Tourist>();
                ModelState.AddModelError(string.Empty, "Server error.");
            }
            return tourist;
        }

        private decimal Gettotalcost(int people, int parkId, int safariId, int vId)
        {
            Parks park = GetParkById(parkId);
            SafariDetail safari = GetSafariById(safariId);
            Vehicle vehicle = GetVehicleById(vId);

            decimal cost = (decimal)park.Fee * people + (decimal)safari.SafariCost * people + (decimal)vehicle.EntryCost;
            return cost;

        }

        private Vehicle GetVehicleById(int vId)
        {
            Vehicle vehicle = null;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    //HTTP GET// http://localhost:53225/api/parks
                    var responseTask = client.GetAsync($"Vehicle/{vId}");  // PersonDetails is the WebApi controller name
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
                        vehicle = (Vehicle)Enumerable.Empty<Vehicle>();
                        ModelState.AddModelError(string.Empty, "Server error.");
                    }
                }
            }
            catch (Exception ex)
            {
                vehicle = (Vehicle)Enumerable.Empty<Vehicle>();
                ModelState.AddModelError(string.Empty, "Server error.");
            }
            return vehicle;
        }

        public ActionResult SafariByPark(int id, double pg = 1)
        {
            IEnumerable<SafariDetail> list = null;
            IEnumerable<SafariDetail> data = null;
            const int pageSize = 4;


            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    //HTTP GET //api/employees
                    var responseTask = client.GetAsync($"safari/safari/{id}");  // PersonDetails is the WebApi controller name
                                                                                // wait for task to complete
                    responseTask.Wait();
                    var result = responseTask.Result;
                    // check the status code for success
                    if (result.IsSuccessStatusCode)
                    {
                        // Add nuget package: Microsoft.AspNet.WebApi.Client
                        var readTask = result.Content.ReadAsAsync<IEnumerable<SafariDetail>>();
                        readTask.Wait();
                        // fill the list vairable created above with the returned result
                        list = readTask.Result;
                        if (pg < 1)
                        {
                            pg = 1;
                        }
                        int recsCount = list.Count();
                        var pager = new Pager(recsCount, (int)pg, pageSize);
                        int recSkip = ((int)pg - 1) * pageSize;
                        data = list.Skip(recSkip).Take(pageSize).ToList();
                        this.ViewBag.Pager = pager;
                    }
                    else //web api sent error response 
                    {
                        list = Enumerable.Empty<SafariDetail>();
                        ModelState.AddModelError(string.Empty, "Server error.");
                    }
                }
            }
            catch (Exception ex)
            {
                list = Enumerable.Empty<SafariDetail>();
                ModelState.AddModelError(string.Empty, "Server error.");
            }
            return View(data);
        }

        private SafariDetail GetSafariById(int safariId)
        {
            SafariDetail safari = null;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    //HTTP GET// http://localhost:53225/api/parks
                    var responseTask = client.GetAsync($"Safari/{safariId}");  // PersonDetails is the WebApi controller name
                                                                               // wait for task to complete
                    responseTask.Wait();
                    var result = responseTask.Result;
                    // check the status code for success
                    if (result.IsSuccessStatusCode)
                    {
                        // Add nuget package: Microsoft.AspNet.WebApi.Client
                        var readTask = result.Content.ReadAsAsync<SafariDetail>();
                        readTask.Wait();
                        // fill the list vairable created above with the returned result
                        safari = readTask.Result;
                    }
                    else //web api sent error response 
                    {
                        safari = (SafariDetail)Enumerable.Empty<SafariDetail>();
                        ModelState.AddModelError(string.Empty, "Server error.");
                    }
                }
            }
            catch (Exception ex)
            {
                safari = (SafariDetail)Enumerable.Empty<SafariDetail>();
                ModelState.AddModelError(string.Empty, "Server error.");
            }
            return safari;
        }

        private Gate GetGateById(int gateId)
        {
            Gate list = null;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    //HTTP GET// http://localhost:53225/api/parks
                    var responseTask = client.GetAsync($"gate/{gateId}");  // PersonDetails is the WebApi controller name
                                                                            // wait for task to complete
                    responseTask.Wait();
                    var result = responseTask.Result;
                    // check the status code for success
                    if (result.IsSuccessStatusCode)
                    {
                        // Add nuget package: Microsoft.AspNet.WebApi.Client
                        var readTask = result.Content.ReadAsAsync<Gate>();
                        readTask.Wait();
                        // fill the list vairable created above with the returned result
                        list = readTask.Result;
                    }
                    else //web api sent error response 
                    {
                        list = (Gate)Enumerable.Empty<Gate>();
                        ModelState.AddModelError(string.Empty, "Server error.");
                    }
                }
            }
            catch (Exception ex)
            {
                list = (Gate)Enumerable.Empty<Gate>();
                ModelState.AddModelError(string.Empty, "Server error.");
            }
            return list;
        }

        private Parks GetParkById(int parkId)
        {
            Parks list = null;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    //HTTP GET// http://localhost:53225/api/parks
                    var responseTask = client.GetAsync($"parks/{parkId}");  // PersonDetails is the WebApi controller name
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
                        list = readTask.Result;
                    }
                    else //web api sent error response 
                    {
                        list = (Parks)Enumerable.Empty<Parks>();
                        ModelState.AddModelError(string.Empty, "Server error.");
                    }
                }
            }
            catch (Exception ex)
            {
                list = (Parks)Enumerable.Empty<Parks>();
                ModelState.AddModelError(string.Empty, "Server error.");
            }
            return list;
        }

        private bool AddBooking(Booking booked)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    //HTTP POST// http://localhost:53225/api/safari
                    var responseTask = client.PostAsJsonAsync("Booking", booked);  // PersonDetails is the WebApi controller name
                                                                                   // wait for task to complete
                    responseTask.Wait();
                    var result = responseTask.Result;
                    // check the status code for success
                    if (result.IsSuccessStatusCode)
                    {
                        return true;
                    }
                    else //web api sent error response 
                    {
                        ModelState.AddModelError(string.Empty, "Server error.");
                        return false;
                    }
                }
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Server error.");
                return false;
            }
        }

        private bool AddTourist(Tourist tourist)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    //HTTP POST// http://localhost:53225/api/Tourist
                    var responseTask = client.PostAsJsonAsync("tourist", tourist);  // PersonDetails is the WebApi controller name
                                                                                    // wait for task to complete
                    responseTask.Wait();
                    var result = responseTask.Result;
                    // check the status code for success
                    if (result.IsSuccessStatusCode)
                    {
                        return true;
                    }
                    else //web api sent error response 
                    {
                        ModelState.AddModelError(string.Empty, "Server error.");
                        return false;
                    }
                }
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Server error.");
                return false;
            }
        }

        public Booking GetById(int Id)
        {
            Booking booking = null;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    //HTTP GET// http://localhost:53225/api/parks
                    var responseTask = client.GetAsync($"booking/{Id}");  // PersonDetails is the WebApi controller name
                                                                                 // wait for task to complete
                    responseTask.Wait();
                    var result = responseTask.Result;
                    // check the status code for success
                    if (result.IsSuccessStatusCode)
                    {
                        // Add nuget package: Microsoft.AspNet.WebApi.Client
                        var readTask = result.Content.ReadAsAsync<Booking>();
                        readTask.Wait();
                        // fill the list vairable created above with the returned result
                        booking = readTask.Result;
                    }
                    else //web api sent error response 
                    {
                        //booking = (Booking)Enumerable.Empty<Booking>();
                        ModelState.AddModelError(string.Empty, "No Booking Found");
                    }
                }
            }
            catch (Exception ex)
            {
                booking = (Booking)Enumerable.Empty<Booking>();
                ModelState.AddModelError(string.Empty, "Server error.");
            }
            return booking;
        }

        public int GetBookingId()
        {
            int b = 0;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    //HTTP GET// http://localhost:53225/api/parks
                    var responseTask = client.GetAsync($"Booking/BookingById");  // PersonDetails is the WebApi controller name
                                                                                 // wait for task to complete
                    responseTask.Wait();
                    var result = responseTask.Result;
                    // check the status code for success
                    if (result.IsSuccessStatusCode)
                    {
                        // Add nuget package: Microsoft.AspNet.WebApi.Client
                        var readTask = result.Content.ReadAsAsync<int>();
                        readTask.Wait();
                        // fill the list vairable created above with the returned result
                        b = readTask.Result;
                    }
                    else //web api sent error response 
                    {

                        ModelState.AddModelError(string.Empty, "Server error.");
                    }
                }
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, "Server error.");
            }
            return b;

        }
        public static void SendEmail(string mail,Payment p)
        {
            System.Net.Mail.MailMessage mailing = new System.Net.Mail.MailMessage();
            System.Net.Mail.SmtpClient SmtpServer = new System.Net.Mail.SmtpClient("smtp.gmail.com");

            mailing.From = new System.Net.Mail.MailAddress("junglesafarisprint2021@gmail.com");
            mailing.To.Add(mail);
            mailing.Subject = $"Safari Booking Confirmation";
            mailing.Body = $"\n**** Thank you for booking a Safari ****\n\nHere are your booking details\n\nPark: {p.ParkName}\n\nSafari: {p.SafariName}" +
                                $"\n\nVehicle: {p.VehicleName}\n\n Name: {p.TouristName}\n\nAmount to be Paid: Rs.{p.Total}/-\n\n\nYour Entry will be from {p.GateName} Gate\n\n\n*********** Please carry your Id proof and Show Booking Id to Pay Cash***********" +
                                $"\n\nWe are looking forward for your visit\n\n\n--Team Jungle Safari Booking";
            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("junglesafarisprint2021@gmail.com", "Asdfg@123");
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mailing);
        }
    }
}

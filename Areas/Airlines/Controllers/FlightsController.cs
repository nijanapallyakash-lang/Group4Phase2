using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Group4Flight.Models;

namespace Group4Flight.Areas.Airlines.Controllers
{
    [Area("Airlines")]
    public class FlightsController : Controller
    {
        private readonly FlightContext _db;

        public FlightsController(FlightContext db)
        {
            _db = db;
        }

        // GET: Airlines/Flights/Index — shows list + add form + inline edit form
        public IActionResult Index(int? editId)
        {
            var vm = BuildViewModel(editId);
            return View(vm);
        }

        // POST: Airlines/Flights/Create — PRG back to Index
        [HttpPost]
        [ValidateAntiForgeryToken]
        // FIXED: The [Bind(Prefix)] forces the model binder to attach the "NewFlight" prefix to the ModelState errors
        public IActionResult Create([Bind(Prefix = "NewFlight")] Flight flight)
        {
            if (ModelState.IsValid)
            {
                // REQUIREMENT: Avoid duplicate validation effort using TempData.
                if (TempData.Peek("RemoteValidationRan") == null)
                {
                    bool isDuplicate = _db.Flights.Any(f => 
                        f.FlightCode.ToLower() == flight.FlightCode.ToLower() && 
                        f.Date.Date == flight.Date.Date);

                    if (isDuplicate)
                    {
                        // The prefix here now perfectly matches the UI Tag Helpers
                        ModelState.AddModelError("NewFlight.FlightCode", $"Flight {flight.FlightCode.ToUpper()} is already scheduled for {flight.Date.ToShortDateString()}.");
                        
                        var vmError = BuildViewModel(null);
                        vmError.NewFlight = flight;
                        return View(nameof(Index), vmError);
                    }
                }

                _db.Flights.Add(flight);
                _db.SaveChanges();

                // Clear the TempData flag now that the flight is successfully saved
                TempData.Remove("RemoteValidationRan");

                TempData["Message"] = "Flight " + flight.FlightCode.ToUpper() + " added successfully.";
                return RedirectToAction(nameof(Index));
            }
            
            var vm = BuildViewModel(null);
            vm.EditFlight = null;
            vm.NewFlight = flight;
            return View(nameof(Index), vm);
        }

        // GET/POST: Airlines/Flights/CheckDuplicate — Target for Remote Validation
        [AcceptVerbs("GET", "POST")]
        public IActionResult CheckDuplicate()
        {
            // Set the TempData flag so the Create method knows the DB was already queried
            TempData["RemoteValidationRan"] = true;

            // FOOLPROOF DATA EXTRACTION: Grab the raw values directly from the URL query
            string flightCode = Request.Query["NewFlight.FlightCode"].ToString();
            string dateString = Request.Query["NewFlight.Date"].ToString();

            // If either is missing, approve it and let the server-side catch it later
            if (string.IsNullOrEmpty(flightCode) || string.IsNullOrEmpty(dateString))
            {
                return Json(true);
            }

            // Parse the date and check the database
            if (DateTime.TryParse(dateString, out DateTime date))
            {
                var existingFlight = _db.Flights.FirstOrDefault(f => 
                    f.FlightCode.ToLower() == flightCode.ToLower() && 
                    f.Date.Date == date.Date);

                if (existingFlight != null)
                {
                    return Json($"Flight {flightCode.ToUpper()} is already scheduled for {date.ToShortDateString()}.");
                }
            }

            return Json(true);
        }

        // POST: Airlines/Flights/Edit — PRG back to Index
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Flight flight)
        {
            if (ModelState.IsValid)
            {
                _db.Flights.Update(flight);
                _db.SaveChanges();
                TempData["Message"] = "Flight " + flight.FlightCode + " updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            var vm = BuildViewModel(flight.FlightId);
            return View(nameof(Index), vm);
        }

        // POST: Airlines/Flights/Delete — PRG back to Index
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int flightId)
        {
            var flight = _db.Flights.Find(flightId);
            if (flight != null)
            {
                var code = flight.FlightCode;
                _db.Flights.Remove(flight);
                _db.SaveChanges();
                TempData["Message"] = "Flight " + code + " deleted successfully.";
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Regulation()
        {
            return Content("Airline Regulation Content");
        }

        private FlightManageViewModel BuildViewModel(int? editId)
        {
            return new FlightManageViewModel
            {
                Flights       = _db.Flights.Include(f => f.Airline).OrderBy(f => f.Date).ToList(),
                Airlines      = _db.Airlines.ToList(),
                CabinTypes    = Flight.CabinTypes,
                AircraftTypes = Flight.AircraftTypes,
                NewFlight     = new Flight { Date = DateTime.Today.AddDays(1) },
                EditFlight    = editId.HasValue ? _db.Flights.Find(editId.Value) : null,
                EditId        = editId
            };
        }
    }
}
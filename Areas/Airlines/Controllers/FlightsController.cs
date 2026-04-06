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
        public IActionResult Create(Flight flight)
        {
            if (ModelState.IsValid)
            {
                _db.Flights.Add(flight);
                _db.SaveChanges();
                TempData["Message"] = "Flight " + flight.FlightCode + " added successfully.";
                return RedirectToAction(nameof(Index));
            }
            var vm = BuildViewModel(null);
            vm.EditFlight = null;
            vm.NewFlight = flight;
            return View(nameof(Index), vm);
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


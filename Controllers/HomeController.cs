using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Group4Flight.Models;

namespace Group4Flight.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly FlightContext _db;

    public HomeController(ILogger<HomeController> logger, FlightContext db)
    {
        _logger = logger;
        _db = db;
    }

    private FlightSession Session => new FlightSession(HttpContext.Session);
    private FlightCookies Cookie => new FlightCookies(HttpContext);

    public IActionResult Index()
    {
        var cookieIds = Cookie.GetSelections();

        if (!cookieIds.Any() && Session.GetSelections().Any())
        {
            Session.ClearSelections();
        }
        else
        {
            Session.SeedFromCookie(cookieIds);
        }

        var criteria = Session.GetFilter();
        var vm = BuildViewModel(criteria);
        return View(vm);
    }

    [HttpPost]
    public IActionResult Index(FlightViewModel form)
    {
        var criteria = new FilterCriteria
        {
            From  = form.FilterFrom,
            To    = form.FilterTo,
            Date  = form.FilterDate,
            Cabin = form.FilterCabin
        };
        Session.SetFilter(criteria);
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Details(int id)
    {
        var flight = _db.Flights.Include(f => f.Airline).FirstOrDefault(f => f.FlightId == id);
        if (flight == null) return NotFound();

        ViewBag.IsSelected = Session.GetSelections().Contains(id);
        ViewBag.SelectionCount = Session.SelectionCount;
        return View(flight);
    }

    [HttpPost]
    public IActionResult Select(int flightId)
    {
        bool added = Session.AddSelection(flightId);
        if (added)
        {
            Cookie.AddSelection(flightId); // keep cookie in sync
            TempData["Message"] = "Flight selected! You now have " + Session.SelectionCount + " flight(s) selected.";
        }
        else
        {
            TempData["Message"] = "This flight is already in your selections.";
        }
        return RedirectToAction(nameof(Details), new { id = flightId });
    }

    public IActionResult Selections()
    {
        var ids = Session.GetSelections();
        var flights = _db.Flights.Include(f => f.Airline)
                                 .Where(f => ids.Contains(f.FlightId))
                                 .ToList();
        ViewBag.SelectionCount = ids.Count;
        return View(flights);
    }

    [HttpPost]
    public IActionResult CancelSelection(int flightId)
    {
        Session.RemoveSelection(flightId);
        Cookie.RemoveSelection(flightId);
        TempData["Message"] = "Flight removed from your selections.";
        return RedirectToAction(nameof(Selections));
    }

    [HttpPost]
    public IActionResult ClearSelections()
    {
        Session.ClearSelections();
        Cookie.ClearSelections();
        TempData["Message"] = "All selections have been cleared.";
        return RedirectToAction(nameof(Selections));
    }

    public IActionResult Privacy()
    {
        return Content("Client Privacy Policy Content");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private FlightViewModel BuildViewModel(FilterCriteria criteria)
    {
        var query = _db.Flights.Include(f => f.Airline).AsQueryable();

        if (!string.IsNullOrWhiteSpace(criteria.From))
            query = query.Where(f => f.From == criteria.From);
        if (!string.IsNullOrWhiteSpace(criteria.To))
            query = query.Where(f => f.To == criteria.To);
        if (criteria.Date.HasValue)
            query = query.Where(f => f.Date.Date == criteria.Date.Value.Date);
        if (!string.IsNullOrWhiteSpace(criteria.Cabin))
            query = query.Where(f => f.CabinType == criteria.Cabin);

        return new FlightViewModel
        {
            FilterFrom     = criteria.From,
            FilterTo       = criteria.To,
            FilterDate     = criteria.Date,
            FilterCabin    = criteria.Cabin,
            Flights        = query.ToList(),
            Airlines       = _db.Airlines.ToList(),
            FromCities     = _db.Flights.Select(f => f.From).Distinct().OrderBy(c => c).ToList(),
            ToCities       = _db.Flights.Select(f => f.To).Distinct().OrderBy(c => c).ToList(),
            SelectionCount = Session.SelectionCount
        };
    }
}

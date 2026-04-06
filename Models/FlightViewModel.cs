namespace Group4Flight.Models
{
    // ViewModel used by HomeController for the client-facing pages
    public class FlightViewModel
    {
        // Filter inputs (bound from form)
        public string? FilterFrom  { get; set; }
        public string? FilterTo    { get; set; }
        public DateTime? FilterDate { get; set; }
        public string? FilterCabin { get; set; }

        // Dropdown source data
        public List<string> FromCities    { get; set; } = new();
        public List<string> ToCities      { get; set; } = new();
        public List<string> CabinTypes    { get; set; } = Flight.CabinTypes;
        public List<string> AircraftTypes { get; set; } = Flight.AircraftTypes;

        // Results
        public List<Flight>  Flights  { get; set; } = new();
        public List<Airline> Airlines { get; set; } = new();

        // For selection badge in nav
        public int SelectionCount { get; set; }
    }

    // ViewModel used by FlightsController (Airline area) for the all-in-one manage page
    public class FlightManageViewModel
    {
        public List<Flight>  Flights       { get; set; } = new();
        public List<Airline> Airlines      { get; set; } = new();
        public List<string>  CabinTypes    { get; set; } = new();
        public List<string>  AircraftTypes { get; set; } = new();

        // Blank flight for the Add form
        public Flight  NewFlight  { get; set; } = new();

        // Flight currently being edited (null if none)
        public Flight? EditFlight { get; set; }
        public int?    EditId     { get; set; }
    }
}

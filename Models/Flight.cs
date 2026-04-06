namespace Group4Flight.Models
{
    public class Flight
    {
        public int FlightId { get; set; }
        public string FlightCode { get; set; } = "";
        public string From { get; set; } = "";
        public string To { get; set; } = "";
        public DateTime Date { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public TimeSpan ArrivalTime { get; set; }
        public string CabinType { get; set; } = "";
        public double Emission { get; set; }
        public string AircraftType { get; set; } = "";
        public decimal Price { get; set; }

        // Foreign key to Airline
        public int AirlineId { get; set; }
        public Airline? Airline { get; set; }

        // Predefined static lists (requirement: static list in model class)
        public static readonly List<string> CabinTypes = new()
        {
            "Basic Economy", "Economy", "Economy Plus", "Business"
        };

        public static readonly List<string> AircraftTypes = new()
        {
            "Airbus A318", "Airbus A319", "Airbus A320", "Airbus A321",
            "Boeing 737-700", "Boeing 737-800", "Boeing 737-900", "Boeing 737 MAX"
        };
    }
}

namespace Group4Flight.Models
{
    public class Airline
    {
        public int AirlineId { get; set; }
        public string Name { get; set; } = "";
        public string ImageName { get; set; } = ""; // file in wwwroot/images

        public ICollection<Flight> Flights { get; set; } = new List<Flight>();
    }
}

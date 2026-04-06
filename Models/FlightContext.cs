using Microsoft.EntityFrameworkCore;

namespace Group4Flight.Models
{
    public class FlightContext : DbContext
    {
        public FlightContext(DbContextOptions<FlightContext> options) : base(options) { }

        public DbSet<Flight> Flights { get; set; }
        public DbSet<Airline> Airlines { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Airline>().HasData(
                new Airline { AirlineId = 1, Name = "Delta Air Lines",    ImageName = "delta.png"     },
                new Airline { AirlineId = 2, Name = "United Airlines",    ImageName = "united.png"    },
                new Airline { AirlineId = 3, Name = "American Airlines",  ImageName = "american.png"  },
                new Airline { AirlineId = 4, Name = "Southwest Airlines", ImageName = "southwest.png" }
            );

            modelBuilder.Entity<Flight>().HasData(
                new Flight { FlightId = 1, FlightCode = "DL201", From = "Chicago",     To = "Boston",
                    Date = new DateTime(2026,8,1), DepartureTime = new TimeSpan(10,0,0),
                    ArrivalTime = new TimeSpan(13,15,0), CabinType = "Economy",
                    AircraftType = "Airbus A321", Emission = 190, Price = 245m, AirlineId = 1 },
                new Flight { FlightId = 2, FlightCode = "UA310", From = "Houston",     To = "San Francisco",
                    Date = new DateTime(2026,8,1), DepartureTime = new TimeSpan(11,45,0),
                    ArrivalTime = new TimeSpan(15,20,0), CabinType = "Economy Plus",
                    AircraftType = "Boeing 787-9", Emission = 280, Price = 355m, AirlineId = 2 },
                new Flight { FlightId = 3, FlightCode = "AA478", From = "New York",    To = "Orlando",
                    Date = new DateTime(2026,8,2), DepartureTime = new TimeSpan(7,20,0),
                    ArrivalTime = new TimeSpan(10,40,0), CabinType = "Business",
                    AircraftType = "Airbus A330", Emission = 175, Price = 610m, AirlineId = 3 },
                new Flight { FlightId = 4, FlightCode = "WN508", From = "Los Angeles", To = "Seattle",
                    Date = new DateTime(2026,8,2), DepartureTime = new TimeSpan(13,5,0),
                    ArrivalTime = new TimeSpan(15,25,0), CabinType = "Basic Economy",
                    AircraftType = "Boeing 737-900", Emission = 205, Price = 175m, AirlineId = 4 },
                new Flight { FlightId = 5, FlightCode = "DL630", From = "Miami",       To = "Atlanta",
                    Date = new DateTime(2026,8,3), DepartureTime = new TimeSpan(8,15,0),
                    ArrivalTime = new TimeSpan(10,10,0), CabinType = "Economy",
                    AircraftType = "Airbus A220", Emission = 160, Price = 215m, AirlineId = 1 }
            );
        }
    }
}

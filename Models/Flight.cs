using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Group4Flight.Models
{
    public class Flight
    {
        public int FlightId { get; set; }

        [Required(ErrorMessage = "Flight Code is required.")]
        [RegularExpression(@"^[a-zA-Z]{2}\d{1,4}$", ErrorMessage = "Flight Code must start with 2 letters followed by 1 to 4 digits.")]
        [Remote("CheckDuplicate", "Flights", "Airlines", AdditionalFields = "Date", ErrorMessage = "This Flight already exists on this Date.")]
        public string FlightCode { get; set; } = "";

        [Required(ErrorMessage = "Departure city is required.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "City must contain letters and spaces only.")]
        [StringLength(50, ErrorMessage = "City cannot exceed 50 characters.")]
        public string From { get; set; } = "";

        [Required(ErrorMessage = "Arrival city is required.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "City must contain letters and spaces only.")]
        [StringLength(50, ErrorMessage = "City cannot exceed 50 characters.")]
        public string To { get; set; } = "";

        [Required(ErrorMessage = "Date is required.")]
        [ValidFlightDate(ErrorMessage = "Date must be in the future and cannot exceed 3 years from today.")]
        [Remote("CheckDuplicate", "Flights", "Airlines", AdditionalFields = "FlightCode", ErrorMessage = "This Flight already exists on this Date.")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Departure time is required.")]
        public TimeSpan DepartureTime { get; set; }

        [Required(ErrorMessage = "Arrival time is required.")]
        public TimeSpan ArrivalTime { get; set; }

        public string CabinType { get; set; } = "";

        [Range(0, 5000, ErrorMessage = "Emission cannot exceed 5000kg CO2e.")]
        public double Emission { get; set; }

        public string AircraftType { get; set; } = "";

        [Range(0, 50000, ErrorMessage = "Price must be between $0 and $50,000.")]
        public decimal Price { get; set; }

        // Foreign key to Airline
        public int AirlineId { get; set; }
        public Airline? Airline { get; set; }

        // Predefined static lists
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
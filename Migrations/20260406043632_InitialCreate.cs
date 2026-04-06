using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Group4Flight.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Airlines",
                columns: table => new
                {
                    AirlineId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ImageName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Airlines", x => x.AirlineId);
                });

            migrationBuilder.CreateTable(
                name: "Flights",
                columns: table => new
                {
                    FlightId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FlightCode = table.Column<string>(type: "TEXT", nullable: false),
                    From = table.Column<string>(type: "TEXT", nullable: false),
                    To = table.Column<string>(type: "TEXT", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DepartureTime = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    ArrivalTime = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    CabinType = table.Column<string>(type: "TEXT", nullable: false),
                    Emission = table.Column<double>(type: "REAL", nullable: false),
                    AircraftType = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false),
                    AirlineId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flights", x => x.FlightId);
                    table.ForeignKey(
                        name: "FK_Flights_Airlines_AirlineId",
                        column: x => x.AirlineId,
                        principalTable: "Airlines",
                        principalColumn: "AirlineId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Airlines",
                columns: new[] { "AirlineId", "ImageName", "Name" },
                values: new object[,]
                {
                    { 1, "delta.png", "Delta Air Lines" },
                    { 2, "united.png", "United Airlines" },
                    { 3, "american.png", "American Airlines" },
                    { 4, "southwest.png", "Southwest Airlines" }
                });

            migrationBuilder.InsertData(
                table: "Flights",
                columns: new[] { "FlightId", "AircraftType", "AirlineId", "ArrivalTime", "CabinType", "Date", "DepartureTime", "Emission", "FlightCode", "From", "Price", "To" },
                values: new object[,]
                {
                    { 1, "Airbus A321", 1, new TimeSpan(0, 13, 15, 0, 0), "Economy", new DateTime(2026, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 10, 0, 0, 0), 190.0, "DL201", "Chicago", 245m, "Boston" },
                    { 2, "Boeing 787-9", 2, new TimeSpan(0, 15, 20, 0, 0), "Economy Plus", new DateTime(2026, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 11, 45, 0, 0), 280.0, "UA310", "Houston", 355m, "San Francisco" },
                    { 3, "Airbus A330", 3, new TimeSpan(0, 10, 40, 0, 0), "Business", new DateTime(2026, 8, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 20, 0, 0), 175.0, "AA478", "New York", 610m, "Orlando" },
                    { 4, "Boeing 737-900", 4, new TimeSpan(0, 15, 25, 0, 0), "Basic Economy", new DateTime(2026, 8, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 13, 5, 0, 0), 205.0, "WN508", "Los Angeles", 175m, "Seattle" },
                    { 5, "Airbus A220", 1, new TimeSpan(0, 10, 10, 0, 0), "Economy", new DateTime(2026, 8, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 15, 0, 0), 160.0, "DL630", "Miami", 215m, "Atlanta" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Flights_AirlineId",
                table: "Flights",
                column: "AirlineId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Flights");

            migrationBuilder.DropTable(
                name: "Airlines");
        }
    }
}

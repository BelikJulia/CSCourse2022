using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Flight
    {
        public int Id { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string FlightNumber { get; set; }
        public DateTime DepartureTime { get; set; }
    }
}

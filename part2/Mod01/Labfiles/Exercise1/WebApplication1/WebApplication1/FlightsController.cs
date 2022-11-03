using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1
{
    [Route("api/[controller]")]
    public class FlightsController : Controller
    {
        private readonly FlightsContext _context;

        public FlightsController(FlightsContext context)
        {
            _context = context;
        }

        [HttpGet]
        // GET api/flights
        public IEnumerable<Flight> Get()
        {
            return _context.Flight.ToList();
        }

        // POST api/flights
        [HttpPost]
        public IActionResult Post([FromBody]Flight flight)
        {
            _context.Flight.Add(flight);
            _context.SaveChanges();

            return CreatedAtAction(nameof(Get), flight.Id);
        }


    }
}
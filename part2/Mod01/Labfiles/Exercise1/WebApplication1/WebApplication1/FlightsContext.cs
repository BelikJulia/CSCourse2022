using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1
{
    public class FlightsContext : DbContext
    {
        public FlightsContext(DbContextOptions<FlightsContext> options) : base(options){}

        public DbSet<Flight> Flight { get; set; }
    }
}
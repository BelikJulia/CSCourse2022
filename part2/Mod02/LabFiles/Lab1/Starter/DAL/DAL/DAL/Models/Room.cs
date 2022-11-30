using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
    class Room
    {
        public int RoomId { get; set; }
        public int Number { get; set; }
        public decimal Price { get; set; }
        public bool Available { get; set; }
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}

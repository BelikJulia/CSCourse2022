using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Models
{
    class Hotel
    {
        public int HotelId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public ICollection<Room> Rooms { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BlueYonder.Hotels.DAL.Models;

namespace BlueYonder.Hotels.DAL.Repository
{
    interface IHotelBookingRepository
    {
        IEnumerable<Room> GetAvaliabileByDate(DateTime date);
        IEnumerable<Reservation> GetAllReservation();
        Task DeleteReservation(int reservationId);
    }
}

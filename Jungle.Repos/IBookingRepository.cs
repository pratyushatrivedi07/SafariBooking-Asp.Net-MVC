using Jungle.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jungle.Repos
{
     public interface IBookingRepository : IRepository<Booking>
     {
        IEnumerable<Booking> GetByBookingId(int Id);
        public int GetBookingByID();
    }
}

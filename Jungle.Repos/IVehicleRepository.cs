using Jungle.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jungle.Repos
{
     public interface IVehicleRepository : IRepository<Vehicle>
    {
        IEnumerable<Vehicle> GetByPark(int pId);
    }
}

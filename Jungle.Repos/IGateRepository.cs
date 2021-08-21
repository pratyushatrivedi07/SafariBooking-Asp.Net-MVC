using Jungle.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jungle.Repos
{
    public interface IGateRepository :IRepository<Gate>
    {
        IEnumerable<Gate> GetByPark(int parkId);
    }
}

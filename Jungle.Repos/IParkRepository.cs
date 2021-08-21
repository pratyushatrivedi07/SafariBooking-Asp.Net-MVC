using Jungle.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jungle.Repos
{
    public interface IParkRepository : IRepository<Parks>
    {
        IEnumerable<Parks> GetByLocation(string location);
        IEnumerable<Parks> Search(string criteria);   
    }
}

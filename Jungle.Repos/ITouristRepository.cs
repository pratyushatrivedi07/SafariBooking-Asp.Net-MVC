using Jungle.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jungle.Repos
{
    public interface ITouristRepository : IRepository<Tourist>
    {
        public Tourist GetTouristByEmail(object emailid);
    }
}

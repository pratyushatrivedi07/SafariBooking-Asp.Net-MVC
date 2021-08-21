using Jungle.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jungle.Repos
{
    public interface ISafariDetailRepos : IRepository<SafariDetail>
    {
        IEnumerable<SafariDetail> SafariByPark(int parkId);
        IEnumerable<SafariDetail> GetByPark(int parkId);
    }
}

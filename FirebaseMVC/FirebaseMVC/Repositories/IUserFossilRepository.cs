using ACNHWorldMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACNHWorldMVC.Repositories
{
    public interface IUserFossilRepository
    {
        List<UserFossil> GetAllCurrentUserFossils(int userId);
    }
}

using ACNHWorldMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACNHWorldMVC.Repositories
{
    public interface IUserVillagerRepository
    {
        List<UserVillager> GetAllCurrentUserVillagers(int userId);
        public void Delete(int id);
        UserVillager GetUserVillagerById(int id);
    }
}

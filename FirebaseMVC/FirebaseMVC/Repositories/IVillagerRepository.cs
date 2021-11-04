using ACNHWorldMVC.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace ACNHWorldMVC.Repositories
{
    public interface IVillagerRepository
    {
        List<Villager> GetAllVillagers();
        Villager GetVillagerById(int id);
        List<Villager> GetVillagersbyUserId(int userId);
        void AddVillagerToUser(int villagerId, int userId);

    }

}
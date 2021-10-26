using ACNHWorldMVC.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using ACNHWorldMVC.Controllers;

namespace ACNHWorldMVC.Repositories
{
    public class VillagerRepository : IVillagerRepository
    {
        private readonly IConfiguration _config;

        // The constructor accepts an IConfiguration object as a parameter. This class comes from the ASP.NET framework and is useful for retrieving things out of the appsettings.json file like connection strings.
        public VillagerRepository(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        public List<Villager> GetAllVillagers()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT v.Id, v.[Name], v.ImageUrl
                        FROM Villager v
                        LEFT JOIN UserVillager uv ON uv.VillagerId = v.id
                        LEFT JOIN [User] u ON v.Id = u.id
                    ";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Villager> villagers = new List<Villager>();
                    while (reader.Read())
                    {
                        Villager villager = new Villager
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl"))
                        };

                        villagers.Add(villager);
                    }

                    reader.Close();

                    return villagers;
                }
            }
        }

        public Villager GetVillagerById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, [Name], ImageUrl
                        FROM Villager
                        WHERE Id = @id
                    ";

                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        Villager villager = new Villager
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl"))
                        };

                        reader.Close();
                        return villager;
                    }
                    else
                    {
                        reader.Close();
                        return null;
                    }
                }
            }
        }
    }
}
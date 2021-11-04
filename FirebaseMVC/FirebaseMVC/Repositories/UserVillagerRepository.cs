using ACNHWorldMVC.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACNHWorldMVC.Repositories
{
    public class UserVillagerRepository : IUserVillagerRepository
    {
        private readonly IConfiguration _config;

        // The constructor accepts an IConfiguration object as a parameter. This class comes from the ASP.NET framework and is useful for retrieving things out of the appsettings.json file like connection strings.
        public UserVillagerRepository(IConfiguration config)
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
        private UserVillager NewVillagerFromReader(SqlDataReader reader)
        {
            return new UserVillager()
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                VillagerId = reader.GetInt32(reader.GetOrdinal("VillagerId")),
            };
        }
        public List<UserVillager> GetAllCurrentUserVillagers(int userId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                      SELECT uv.id, uv.UserId, uv.VillagerId, u.id, u.FirebaseId, u.Email, v.id, v.[Name], v.ImageUrl
                                        FROM UserVillager AS uv
                                        LEFT JOIN Villager AS v ON uv.VillagerId = v.id
                                        LEFT JOIN [User] AS u ON uv.UserId = u.id
                       WHERE uv.UserId = @userId";

                    cmd.Parameters.AddWithValue("@userId", userId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<UserVillager> userVillagers = new List<UserVillager>();

                    while (reader.Read())
                    {
                        UserVillager userVillager = new UserVillager
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                            VillagerId = reader.GetInt32(reader.GetOrdinal("VillagerId")),
                            Villager = new Villager()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                            },
                        };
                        userVillagers.Add(userVillager);

                    }
                    reader.Close();
                    return userVillagers;



                }
            }
        }

        public UserVillager GetUserVillagerById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        SELECT v.[Name], v.ImageUrl, uv.UserId, uv.Id, uv.VillagerId 
                                        FROM UserVillager uv
                                        INNER JOIN Villager v ON uv.VillagerId = v.Id
                                        WHERE uv.VillagerId = @id;";

                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    UserVillager userVillager = null;

                    if (reader.Read())
                    {

                        userVillager = new UserVillager()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                            VillagerId = reader.GetInt32(reader.GetOrdinal("VillagerId")),
                            Villager = new Villager
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("VillagerId")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl"))
                            }
                        };
                    }
                    reader.Close();
                    return userVillager;
                }
            }
        }

        public void Delete(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            DELETE FROM UserVillager
                            WHERE Id = @id
                        ";

                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
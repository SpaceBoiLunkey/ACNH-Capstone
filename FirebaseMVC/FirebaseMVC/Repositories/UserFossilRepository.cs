using ACNHWorldMVC.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACNHWorldMVC.Repositories
{
    public class UserFossilRepository : IUserFossilRepository
    {
        private readonly IConfiguration _config;

        // The constructor accepts an IConfiguration object as a parameter. This class comes from the ASP.NET framework and is useful for retrieving things out of the appsettings.json file like connection strings.
        public UserFossilRepository(IConfiguration config)
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
        private UserFossil NewFossilFromReader(SqlDataReader reader)
        {
            return new UserFossil()
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                FossilId = reader.GetInt32(reader.GetOrdinal("FossilId")),
            };
        }
        public List<UserFossil> GetAllCurrentUserFossils(int userId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                      SELECT uf.id, uf.UserId, uf.FossilId, u.id, u.FirebaseId, u.Email, f.id, f.[Name], f.ImageUrl
                                        FROM UserFossil AS uf
                                        LEFT JOIN Fossil AS f ON uf.FossilId = f.id
                                        LEFT JOIN [User] AS u ON uf.UserId = u.id
                       WHERE uf.UserId = @userId";

                    cmd.Parameters.AddWithValue("@userId", userId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<UserFossil> userFossils = new List<UserFossil>();

                    while (reader.Read())
                    {
                        UserFossil userFossil = new UserFossil
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                            FossilId = reader.GetInt32(reader.GetOrdinal("FossilId")),
                            Fossil = new Fossil()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                            },
                        };
                        userFossils.Add(userFossil);

                    }
                        reader.Close();
                        return userFossils;
                    
                    

                }
            }
        }

    }
}

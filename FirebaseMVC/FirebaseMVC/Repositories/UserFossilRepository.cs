using ACNHWorldMVC.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACNHWorldMVC.Repositories
{
    public class UserFossilRepository : BaseRepository, IUserFossilRepository
    {
        private readonly IConfiguration _config;

        // The constructor accepts an IConfiguration object as a parameter. This class comes from the ASP.NET framework and is useful for retrieving things out of the appsettings.json file like connection strings.
        public UserFossilRepository(IConfiguration config) : base(config)
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
        public UserFossil GetUserFossilById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        SELECT f.Name, f.ImageUrl, uf.UserId, uf.Id, uf.FossilId 
                                        FROM UserFossil uf
                                        INNER JOIN Fossil f ON uf.FossilId = f.Id
                                        WHERE uf.FossilId = @id;";

                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    UserFossil userFossil = null;

                    if (reader.Read())
                    {

                        userFossil = new UserFossil()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                            FossilId = reader.GetInt32(reader.GetOrdinal("FossilId")),
                            Fossil = new Fossil
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("FossilId")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl"))
                            }
                        };
                    }
                    reader.Close();
                    return userFossil;
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
                            DELETE FROM UserFossil
                            WHERE Id = @id
                        ";

                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}

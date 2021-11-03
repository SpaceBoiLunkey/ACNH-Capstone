using ACNHWorldMVC.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using ACNHWorldMVC.Controllers;
using System;

namespace ACNHWorldMVC.Repositories
{
    public class FossilRepository : IFossilRepository
    {
        private readonly IConfiguration _config;

        // The constructor accepts an IConfiguration object as a parameter. This class comes from the ASP.NET framework and is useful for retrieving things out of the appsettings.json file like connection strings.
        public FossilRepository(IConfiguration config)
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

        public List<Fossil> GetAllFossils()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT f.Id, f.[Name], f.ImageUrl
                        FROM Fossil f
                    ";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Fossil> fossils = new List<Fossil>();
                    while (reader.Read())
                    {
                        Fossil fossil = new Fossil
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl"))
                        };

                        fossils.Add(fossil);
                    }

                    reader.Close();

                    return fossils;
                }
            }
        }

        public Fossil GetFossilById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT f.Id, f.[Name], f.ImageUrl, uf.Id, uf.UserId , uf.FossilId, u.Id, u.[Name], u.FirebaseId, u.Email
                        FROM Fossil f
                        LEFT JOIN UserFossils uf
                        ON f.Id = uf.UserId
                        LEFT JOIN User u
                        ON uf.UserId = u.Id
                        WHERE Id = @id
                    ";

                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        Fossil fossil = new Fossil
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl"))
                        };

                        reader.Close();
                        return fossil;
                    }
                    else
                    {
                        reader.Close();
                        return null;
                    }
                }
            }
        }
        public List<Fossil> GetFossilsbyUserId(int userId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        SELECT f.Id as FId, f.[Name], f.ImageUrl
                                        FROM UserFossil uf
                                        INNER JOIN Fossil f ON uf.FossilId = f.Id
                                        WHERE uf.UserId = @userId;";

                    cmd.Parameters.AddWithValue("@userId", userId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Fossil> fossils = new List<Fossil>();


                    while (reader.Read())
                    {

                        Fossil fossil = new Fossil()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("FId")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl"))
                        };
                        fossils.Add(fossil);
                    }
                    reader.Close();
                    return fossils;
                }
            }
        }

        public void AddFossilToUser(int fossilId, int userId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        INSERT INTO UserFossil (UserId, FossilId)
                                        OUTPUT INSERTED.ID
                                        VALUES (@userid, @fossilId)
                                        ";
                    cmd.Parameters.AddWithValue("@userid", userId);
                    cmd.Parameters.AddWithValue("@fossilid", fossilId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        

        
    }
}

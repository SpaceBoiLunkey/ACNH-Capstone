using ACNHWorldMVC.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using ACNHWorldMVC.Controllers;

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
                        LEFT JOIN UserFossil uf ON uf.FossilId = f.id
                        LEFT JOIN [User] u ON f.Id = u.id
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
                        SELECT Id, [Name], ImageUrl
                        FROM Fossil
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
    }
}

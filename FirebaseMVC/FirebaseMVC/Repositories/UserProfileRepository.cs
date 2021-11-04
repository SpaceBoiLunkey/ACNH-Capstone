using Microsoft.Data.SqlClient;
using ACNHWorldMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ACNHWorldMVC.Repositories
{
    public class UserProfileRepository : IUserProfileRepository
    {

        private readonly IConfiguration _config;

        public UserProfileRepository(IConfiguration config)
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

        public User GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                    SELECT Id, [Name], Email, FirebaseId
                                    FROM [User]
                                    WHERE Id = @Id";

                    cmd.Parameters.AddWithValue("@id", id);

                    User userProfile = null;

                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        userProfile = new User
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            FirebaseId = reader.GetString(reader.GetOrdinal("FirebaseId")),
                        };
                    }
                    reader.Close();

                    return userProfile;
                }
            }
        }

        public User GetByFirebaseUserId(string firebaseUserId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                    SELECT Id, [Name], Email, FirebaseId
                                    FROM [User]
                                    WHERE FirebaseId = @FirebaseUserId";

                    cmd.Parameters.AddWithValue("@FirebaseUserId", firebaseUserId);

                    User userProfile = null;

                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        userProfile = new User
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            FirebaseId = reader.GetString(reader.GetOrdinal("FirebaseId")),
                        };
                    }
                    reader.Close();

                    return userProfile;
                }
            }
        }

        public void Add(User userProfile)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        INSERT INTO
                                        [User] ([Name], Email, FirebaseId) 
                                        OUTPUT INSERTED.ID
                                        VALUES(@name, @email, @firebaseId)";

                    cmd.Parameters.AddWithValue("@name", userProfile.Name);
                    cmd.Parameters.AddWithValue("@email", userProfile.Email);
                    cmd.Parameters.AddWithValue("@firebaseId", userProfile.FirebaseId);

                    userProfile.Id = (int)cmd.ExecuteScalar();
                }
            }
        }
    }
}

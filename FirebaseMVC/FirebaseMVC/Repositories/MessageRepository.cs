using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection.PortableExecutable;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ACNHWorldMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Security.Claims;
using ACNHWorldMVC.Repositories;


namespace ACNHWorldMVC.Repositories
{
    public class MessageRepository : BaseRepository, IMessageRepository
    {
        public MessageRepository(IConfiguration config) : base(config) { }
        public List<Message> GetAllPublishedMessages()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT m.Id, m.UserId, m.Subject, m.Text, 
                              u.[Name], u.FirebaseId, 
                              u.Email
                         FROM Message m
                              LEFT JOIN [User] u ON m.UserId = u.id";
                    var reader = cmd.ExecuteReader();

                    var messages = new List<Message>();

                    while (reader.Read())
                    {
                        messages.Add(NewMessageFromReader(reader));
                    }

                    reader.Close();

                    return messages;
                }
            }
        }

        public Message GetPublishedMessageById(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT m.Id, m.UserId, m.Subject, m.Text, 
                              u.[Name], u.FirebaseId, 
                              u.Email
                         FROM Message m
                              LEFT JOIN [User] u ON m.UserId = u.id
                        WHERE m.id = @id";

                    cmd.Parameters.AddWithValue("@id", id);
                    var reader = cmd.ExecuteReader();

                    Message message = null;

                    if (reader.Read())
                    {
                        message = NewMessageFromReader(reader);
                    }

                    reader.Close();

                    return message;
                }
            }
        }

        public Message GetUserMessageById(int id, int userId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT m.Id, m.UserId, m.Subject, m.Text, 
                              u.[Name], u.FirebaseId, 
                              u.Email,
                         FROM Message m
                              LEFT JOIN User u ON p.UserId = u.id
                        WHERE m.id = @id AND m.UserId = @userId";

                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@userId", userId);
                    var reader = cmd.ExecuteReader();

                    Message message = null;

                    if (reader.Read())
                    {
                        message = NewMessageFromReader(reader);
                    }

                    reader.Close();

                    return message;
                }
            }
        }


        public void Add(Message message)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO Message (
                            UserId, Subject, Text )
                        OUTPUT INSERTED.ID
                        VALUES (
                            @userId, @subject, @text )";
                    cmd.Parameters.AddWithValue("@UserId", message.UserId);
                    cmd.Parameters.AddWithValue("@Subject", message.Subject);
                    cmd.Parameters.AddWithValue("@Text", message.Text);

                    message.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        private Message NewMessageFromReader(SqlDataReader reader)
        {
            return new Message()
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                User = new User()
                {
                    Id = reader.GetInt32(reader.GetOrdinal("UserId")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    Email = reader.GetString(reader.GetOrdinal("Email")),
                    FirebaseId = reader.GetString(reader.GetOrdinal("FirebaseId")),
                },
                Subject = reader.GetString(reader.GetOrdinal("Subject")),
                Text = reader.GetString(reader.GetOrdinal("Text")),
            };
        }
        public void UpdateMessage(Message message)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            UPDATE Message
                            SET  
                                UserId = @userId, 
                                Subject = @subject, 
                                Text = @text
                            WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@userId", message.UserId);
                    cmd.Parameters.AddWithValue("@subject", message.Subject);
                    cmd.Parameters.AddWithValue("@text", message.Text);
                    cmd.Parameters.AddWithValue("@id", message.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void DeleteMessage(int Id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            DELETE FROM Message
                            WHERE Id = @id
                        ";
                    cmd.Parameters.AddWithValue("@id", Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public List<Message> GetAllCurrentUserMessages(int userId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT m.Id, m.UserId, m.Subject, m.Text, 
                              u.[Name], u.FirebaseId, 
                              u.Email,
                         FROM Message m
                              LEFT JOIN User u ON p.UserId = u.id
                        WHERE m.UserId = @userId";

                    cmd.Parameters.AddWithValue("@userId", userId);
                    var reader = cmd.ExecuteReader();

                    var messages = new List<Message>();

                    while (reader.Read())
                    {
                        messages.Add(NewMessageFromReader(reader));
                    }

                    reader.Close();

                    return messages;
                }
            }
        }
    }
}
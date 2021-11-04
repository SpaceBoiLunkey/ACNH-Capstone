using ACNHWorldMVC.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACNHWorldMVC.Repositories
{
    public class CommentRepository : BaseRepository, ICommentRepository
    {
        public CommentRepository(IConfiguration config) : base(config) { }
        public List<Comment> GetAllPublishedComments()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT c.Id, c.MessageId, c.UserId, c.Text
                       FROM Comment c

                              LEFT JOIN [User] u ON c.UserId = u.id
                       WHERE c.MessageId = @id";
                    var reader = cmd.ExecuteReader();

                    List<Comment> comments = new List<Comment>();

                    while (reader.Read())
                    {
                        Comment comment = new Comment
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            MessageId = reader.GetInt32(reader.GetOrdinal("MessageId")),
                            UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                            Text = reader.GetString(reader.GetOrdinal("Text"))
                        };

                        comments.Add(comment);

                    }

                    reader.Close();

                    return comments;
                }
            }
        }
        public List<Comment> GetCommentByMessageId(int messageId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT c.Id, c.MessageId, c.UserId, c.Text, u.[Name]
                       FROM Comment c
                       INNER JOIN Message m ON m.Id = c.MessageId
                       INNER JOIN [User] u ON u.Id = m.UserId
                       WHERE c.MessageId = @id";

                    cmd.Parameters.AddWithValue("@id", messageId);
                    var reader = cmd.ExecuteReader();

                    List<Comment> comments = new List<Comment>();

                    while (reader.Read())
                    {
                        Comment comment = new Comment
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            MessageId = reader.GetInt32(reader.GetOrdinal("MessageId")),
                            UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                            Text = reader.GetString(reader.GetOrdinal("Text"))
                        };

                        comments.Add(comment);

                    }

                    reader.Close();

                    return comments;
                }
            }
        }

        public List<Comment> GetUserCommentById(int id, int userId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT c.Id, c.MessageId, c.UserId, c.Text
                       FROM Comment c
                       INNER JOIN Message m ON m.Id = c.MessageId
                       INNER JOIN [User] u ON u.Id = u.MessageId
                       WHERE c.MessageId = @id";

                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@userId", userId);
                    var reader = cmd.ExecuteReader();

                    List<Comment> comments = new List<Comment>();

                    while (reader.Read())
                    {
                        Comment comment = new Comment
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            MessageId = reader.GetInt32(reader.GetOrdinal("MessageId")),
                            UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                            Text = reader.GetString(reader.GetOrdinal("Text"))
                        };

                        comments.Add(comment);

                    }

                    reader.Close();

                    return comments;
                }
            }
        }


        public void Add(Comment comment)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO Comment (
                            MessageId, UserId, Text
                            )
                        OUTPUT INSERTED.ID
                        VALUES (
                            @messageId, @userId, @text )";
                    cmd.Parameters.AddWithValue("@messageId", comment.MessageId);
                    cmd.Parameters.AddWithValue("@userId", comment.UserId);
                    cmd.Parameters.AddWithValue("@text", comment.Text);

                    comment.Id = (int)cmd.ExecuteScalar();
                }
            }
        }


        public void UpdateComment(Comment comment)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            UPDATE Comment
                            SET  
                                MessageId = @messageId, 
                                UserId = @userId, 
                                Text = @text
                            WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@messageId", comment.MessageId);
                    cmd.Parameters.AddWithValue("@userId", comment.UserId);
                    cmd.Parameters.AddWithValue("@text", comment.Text);
                    cmd.Parameters.AddWithValue("@id", comment.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void DeleteComment(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            DELETE FROM Comment
                            WHERE Id = @id
                        ";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public List<Comment> GetAllCurrentUserComments(int userId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT m.Id, m.Subject, m.Text,
                              u.[Name]
                         FROM Comment c
                        WHERE m.UserId = @userId";

                    cmd.Parameters.AddWithValue("@userId", userId);
                    var reader = cmd.ExecuteReader();

                    List<Comment> comments = new List<Comment>();

                    while (reader.Read())
                    {
                        Comment comment = new Comment
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            MessageId = reader.GetInt32(reader.GetOrdinal("MessageId")),
                            UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                            Text = reader.GetString(reader.GetOrdinal("Text"))
                        };

                        comments.Add(comment);

                    }

                    reader.Close();

                    return comments;
                }
            }
        }
        public Comment GetCommentById(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT m.Id, 
                       INNER JOIN Message m ON c.MessageId = m.Id
                       WHERE c.Id = @id";

                    cmd.Parameters.AddWithValue("@id", id);
                    var reader = cmd.ExecuteReader();

                    List<Comment> comments = new List<Comment>();

                    if (reader.Read())
                    {
                        Comment comment = new Comment
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            MessageId = reader.GetInt32(reader.GetOrdinal("MessageId")),
                            UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                            Text = reader.GetString(reader.GetOrdinal("Text"))
                        };

                        reader.Close();
                        return comment;

                    }

                    reader.Close();
                    return null;
                }
            }
        }

    }
}

using System.Collections.Generic;
using ACNHWorldMVC.Controllers;
using ACNHWorldMVC.Models;

namespace ACNHWorldMVC.Repositories
{
    public interface ICommentRepository
    {
        void Add(Comment comment);
        public List<Comment> GetAllPublishedComments();
        public List<Comment> GetUserCommentById(int id, int userProfileId);
        void UpdateComment(Comment comment);
        void DeleteComment(int Id);

        List<Comment> GetAllCurrentUserComments(int userProfileId);
        public List<Comment> GetCommentByMessageId(int messageId);

        public Comment GetCommentById(int id);

    }
}
using System.Collections.Generic;
using ACNHWorldMVC.Controllers;
using ACNHWorldMVC.Models;

namespace ACNHWorldMVC.Repositories
{
    public interface IMessageRepository
    {
        void Add(Message message);
        List<Message> GetAllPublishedMessages();
        Message GetPublishedMessageById(int id);
        Message GetUserMessageById(int id, int userProfileId);
        void UpdateMessage(Message message);
        void DeleteMessage(int Id);
        public List<Message> GetAllCurrentUserMessages(int userId);
    }
}
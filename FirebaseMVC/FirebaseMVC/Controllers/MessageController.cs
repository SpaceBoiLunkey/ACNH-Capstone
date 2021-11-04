using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System.Security.Claims;
using ACNHWorldMVC.Models;
using ACNHWorldMVC.Repositories;
using System;
using System.Collections.Generic;
using ACNHWorldMVC.Models.ViewModels;

namespace ACNHWorldMVC.Controllers
{
    [Authorize]
    public class MessageController : Controller
    {
        private readonly IMessageRepository _messageRepository;

        public MessageController(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;

        }

        public IActionResult Index()
        {
            List<Message> messages = _messageRepository.GetAllPublishedMessages();

            return View(messages);
        }

        public IActionResult Details(int id)
        {
            Message message = _messageRepository.GetPublishedMessageById(id);

            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Message message)
        {
            try
            {
                int userId = GetCurrentUserId();
                message.UserId = userId;
                _messageRepository.Add(message);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(message);
            }
        }

        public IActionResult Delete(int id)
        {
            Message message = _messageRepository.GetPublishedMessageById(id);

            return View(message);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Message message)
        {
            try
            {
                _messageRepository.DeleteMessage(id);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(message);
            }
        }


        public IActionResult UserMessages()
        {
            int userId = GetCurrentUserId();
            var messages = _messageRepository.GetAllCurrentUserMessages(userId);

            return View(messages);
        }

        private int GetCurrentUserId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }

        public ActionResult Edit(int id)
        {
            Message message = _messageRepository.GetPublishedMessageById(id);

            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit (int id, Message message)
        {
            try
            {
                _messageRepository.UpdateMessage(message);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(message);
            }
        }

    }
}
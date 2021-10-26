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
            var messages = _messageRepository.GetAllPublishedMessages();
            return View(messages);
        }

        public IActionResult Details(int id)
        {
            var message = _messageRepository.GetPublishedMessageById(id);
            if (message == null)
            {
                int userId = GetCurrentUserId();
                message = _messageRepository.GetUserMessageById(id, userId);
                if (message == null)
                {
                    return NotFound();
                }
            }
            return View(message);
        }

        public IActionResult Create()
        {
            var vm = new MessageCreateViewModel();
            return View(vm);
        }

        [HttpPost]
        public IActionResult Create(MessageCreateViewModel vm)
        {
            vm.Message.UserId = GetCurrentUserId();

            _messageRepository.Add(vm.Message);

            return RedirectToAction("Details", new { id = vm.Message.Id });
        }
        public IActionResult Delete(int id)
        {
            Message message = _messageRepository.GetPublishedMessageById(id);

            return View(message);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, Message message)
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
        public IActionResult Edit(int id)
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
        public IActionResult Edit(int id, Message message)
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
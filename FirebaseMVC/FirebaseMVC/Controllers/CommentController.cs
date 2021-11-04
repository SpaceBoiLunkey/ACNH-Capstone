using ACNHWorldMVC.Models;
using ACNHWorldMVC.Models.ViewModels;
using ACNHWorldMVC.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ACNHWorldMVC.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IMessageRepository _messageRepository;

        public CommentController(ICommentRepository commentRepository, IMessageRepository messageRepository)
        {
            _commentRepository = commentRepository;
            _messageRepository = messageRepository;
        }

        public IActionResult Index(int id)
        {
            MessageDetailViewModel vm = new MessageDetailViewModel();
            vm.Comments = _commentRepository.GetCommentByMessageId(id);
            vm.Message = new Message { Id = id };
            return View(vm);
        }

        public IActionResult Details(int id)
        {
            Comment comment = _commentRepository.GetCommentById(id);

            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Comment comment)
        {
            try
            {
                _commentRepository.Add(comment);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(comment);
            }
        }


        public IActionResult Delete(int id)
        {
            Comment comment = _commentRepository.GetCommentById(id);

            return View(comment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, Comment comment)
        {


            Comment commente = _commentRepository.GetCommentById(id);

            try
            {
                _commentRepository.DeleteComment(id);

                return RedirectToAction("Index", new { id = comment.MessageId });
            }
            catch (Exception ex)
            {
                return View(comment);
            }
        }

        public IActionResult UserComments()
        {
            int userProfileId = GetCurrentUserProfileId();
            var comments = _commentRepository.GetAllCurrentUserComments(userProfileId);

            return View(comments);
        }

        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
        public IActionResult Edit(int id)
        {
            Comment comment = _commentRepository.GetCommentById(id);

            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Comment comment)
        {
            Comment commente = _commentRepository.GetCommentById(id);
            try
            {
                _commentRepository.UpdateComment(comment);
                return RedirectToAction("Index", new { id = commente.MessageId });
            }
            catch (Exception ex)
            {
                return View(comment);
            }
        }
    }
}
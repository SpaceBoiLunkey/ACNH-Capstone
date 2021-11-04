using ACNHWorldMVC.Models;
using ACNHWorldMVC.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ACNHWorldMVC.Controllers
{
    public class FossilsController : Controller
    {
        private readonly IFossilRepository _fossilRepo;
        private readonly IUserProfileRepository _userRepo;
        private readonly IUserFossilRepository _userFossilRepo;

        // ASP.NET will give us an instance of our Walker Repository. This is called "Dependency Injection"
        public FossilsController(IFossilRepository fossilRepository, IUserProfileRepository userProfileRepository, IUserFossilRepository userFossilRepository)
        {
            _fossilRepo = fossilRepository;
            _userRepo = userProfileRepository;
            _userFossilRepo = userFossilRepository;
        }

       

        // GET: FossilsController
        public ActionResult Index()
        {
            List<Fossil> fossils = _fossilRepo.GetAllFossils();

            return View(fossils);
        }

        // GET: FossilsController/Details/5
        public ActionResult Details(int id)
        {
            Fossil fossil = _fossilRepo.GetFossilById(id);

            if (fossil == null)
            {
                return NotFound();
            }

            return View(fossil);
        }


        

        // GET: FossilsController/Create
        public ActionResult AddFossil(int id)
        {
            try
            {
                int userId = GetCurrentUserId();
                _fossilRepo.AddFossilToUser(id, userId);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: FossilsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: FossilsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: FossilsController/Delete/5
        
        private int GetCurrentUserId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}

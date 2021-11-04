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
    public class VillagerController : Controller
    {
        private readonly IVillagerRepository _villagerRepo;

        // ASP.NET will give us an instance of our Villager Repository. This is called "Dependency Injection"
        public VillagerController(IVillagerRepository villagerRepository)
        {
            _villagerRepo = villagerRepository;
        }

        // GET: VillagerController
        public ActionResult Index()
        {
            List<Villager> villagers = _villagerRepo.GetAllVillagers();

            return View(villagers);
        }


        // GET: VillagerController/Details/5
        public ActionResult Details(int id)
        {
            Villager villager = _villagerRepo.GetVillagerById(id);

            if (villager == null)
            {
                return NotFound();
            }

            return View(villager);
        }


        public ActionResult AddVillager(int id)
        {
            try
            {
                int userId = GetCurrentUserId();
                _villagerRepo.AddVillagerToUser(id, userId);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: VillagerController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: VillagerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: VillagerController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: VillagerController/Edit/5
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

        // GET: VillagerController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: VillagerController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
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
        private int GetCurrentUserId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}
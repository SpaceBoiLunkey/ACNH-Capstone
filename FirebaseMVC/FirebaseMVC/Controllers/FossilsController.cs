using ACNHWorldMVC.Models;
using ACNHWorldMVC.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACNHWorldMVC.Controllers
{
    public class FossilsController : Controller
    {
        private readonly IFossilRepository _fossilRepo;

        // ASP.NET will give us an instance of our Walker Repository. This is called "Dependency Injection"
        public FossilsController(IFossilRepository fossilRepository)
        {
            _fossilRepo = fossilRepository;
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
        public ActionResult Create()
        {
            return View();
        }

        // POST: FossilsController/Create
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
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: FossilsController/Delete/5
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
    }
}

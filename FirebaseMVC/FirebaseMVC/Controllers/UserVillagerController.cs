using ACNHWorldMVC.Models;
using ACNHWorldMVC.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ACNHWorldMVC.Controllers
{
    public class UserVillagerController : Controller
    {
        private readonly IUserVillagerRepository _userVillagerRepo;

        // ASP.NET will give us an instance of our Walker Repository. This is called "Dependency Injection"
        public UserVillagerController(IVillagerRepository fossilRepository, IUserProfileRepository userProfileRepository, IUserVillagerRepository userVillagerRepository)
        {
            _userVillagerRepo = userVillagerRepository;
        }

        public IActionResult MyVillagers()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            List<UserVillager> userVillagers = _userVillagerRepo.GetAllCurrentUserVillagers(userId);

            return View(userVillagers);
        }
        public ActionResult Delete(int id)
        {
            UserVillager userVillager = _userVillagerRepo.GetUserVillagerById(id);

            return View(userVillager);
        }

        // POST: VillagerController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                _userVillagerRepo.Delete(id);
                return RedirectToAction(nameof(MyVillagers));
            }
            catch
            {
                return View();
            }
        }
    }
}
using ACNHWorldMVC.Models;
using ACNHWorldMVC.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ACNHWorldMVC.Controllers
{
    public class UserFossilController : Controller
    {
        private readonly IUserFossilRepository _userFossilRepo;

        // ASP.NET will give us an instance of our Walker Repository. This is called "Dependency Injection"
        public UserFossilController(IFossilRepository fossilRepository, IUserProfileRepository userProfileRepository, IUserFossilRepository userFossilRepository)
        {
            _userFossilRepo = userFossilRepository;
        }

        public IActionResult MyFossils()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            List<UserFossil> userFossils = _userFossilRepo.GetAllCurrentUserFossils(userId);

            return View(userFossils);
        }
    }
}

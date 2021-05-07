using MedCheck.DAL;
using MedCheck.Models;
using MedCheck.Models.ViewModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedCheck.Controllers
{
    [Authorize(Roles = "Patient")]
    public class PatientController : Controller
    {
        private readonly UserManager<MainUser> _userManager;
        private readonly MedCheckContext _context;

        public PatientController(UserManager<MainUser> userManager, MedCheckContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Profile()
        {
            MainUser user = await _userManager.GetUserAsync(User);
            return View(user);
        }

        
        public async Task<IActionResult> EditPartial()
        {
            MainUser user = await _userManager.GetUserAsync(User);

            return PartialView(user);

        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(MainUser user)
        {
            MainUser current = await _userManager.GetUserAsync(User);

            current.Name = user.Name;
            current.FamilyName = user.FamilyName;
            current.BirthDate = user.BirthDate;

            _context.Update(current);
            _context.SaveChanges();

            return View(nameof(Profile), current);
        }
    }
}

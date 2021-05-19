using MedCheck.DAL;
using MedCheck.Models;
using MedCheck.Models.ViewModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
            Patient user = (Patient)await _userManager.GetUserAsync(User);
            StatsEntryViewModel stats = new StatsEntryViewModel
            { 
                Temperature = 0,
                Pressure = 0,
                Pulse = 0,
                OxygenLevel = 0,
            };

            PatientViewModel patient = new PatientViewModel
            {
                patient = user,
                sevm = stats,
            };

            return View(patient);
        }

        
        //public async Task<IActionResult> EditPartial()
        //{
        //    MainUser user = await _userManager.GetUserAsync(User);

        //    return PartialView(user);

        //}

        public async Task<IActionResult> StatsEntry()
        {
            StatsEntryViewModel stvm = new StatsEntryViewModel
            {
                Pulse = 0,
                Temperature = 0,
                Pressure = 0,
                OxygenLevel = 0,
            };

            return PartialView(stvm);

        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(PatientViewModel user)
        {
            Patient current = (Patient)await _userManager.GetUserAsync(User);

            current.Name = user.patient.Name;
            current.FamilyName = user.patient.FamilyName;
            current.BirthDate = user.patient.BirthDate;

            _context.Update(current);
            _context.SaveChanges();

            user.patient = current;
            StatsEntryViewModel stats = new StatsEntryViewModel
            {
                Temperature = 0,
                Pressure = 0,
                Pulse = 0,
                OxygenLevel = 0,
            };

            user.sevm = stats;

            return View(nameof(Profile), user);
        }

        [HttpPost]
        public async Task<IActionResult> AddRecord(StatsEntryViewModel stvm)
        {
            Patient current = (Patient) await _userManager.GetUserAsync(User);

            Stats stats = new Stats
            {
                UserId = current.Id,
                Date = DateTime.Now,
                Pressure = stvm.Pressure,
                Temperature = stvm.Temperature,
                OxygenLevel = stvm.OxygenLevel,
                Pulse = stvm.Pulse,
            };

            _context.Stats.Add(stats);
            _context.SaveChanges();

            StatsEntryViewModel sevm = new StatsEntryViewModel
            {
                Temperature = 0,
                Pressure = 0,
                Pulse = 0,
                OxygenLevel = 0,
            };


            PatientViewModel user = new PatientViewModel
            {
                patient = current,
                sevm = sevm,
            };


            return View(nameof(Profile), user);

        }

        [HttpPost]
        public ActionResult GetGraph(string graphType)
        {
            return PartialView("~/Views/Patient/Graphs/PatientGraphs.cshtml", graphType);
        }

        [HttpGet]
        public JsonResult GetSpecifiedGraph(string graphType, int showEntries, string graphDate)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var stats = new GetUserStats(_context, graphType);
            var data = stats.GetGraphStats(userId, showEntries);

            return Json(data);
        }

        [HttpGet]
        public JsonResult GetSpecifiedMiniGraph(string graphType, int showEntries, string graphDate)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var stats = new GetUserStats(_context, graphType);

            var data = stats.GetMiniGraphStats(userId, showEntries, graphDate);
            return Json(data);
        }

    }
}

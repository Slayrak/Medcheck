using MedCheck.DAL;
using MedCheck.Models;
using MedCheck.Models.ViewModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MedCheck.Controllers
{
    [Authorize(Roles = "MedWorker")]
    public class MedWorkerController : Controller
    {
        private readonly UserManager<MainUser> _userManager;
        private readonly MedCheckContext _context;
        IWebHostEnvironment _appEnvironment;

        public MedWorkerController(UserManager<MainUser> userManager, MedCheckContext context, IWebHostEnvironment appEnvironment)
        {
            _userManager = userManager;
            _context = context;
            _appEnvironment = appEnvironment;
        }

        public async Task<IActionResult> MedWorkerProfile()
        {
            MedWorker user = (MedWorker)await _userManager.GetUserAsync(User);


            //if (user.UserFamilyID == null)
            //{
            //    user.UserFamilyID = user.Id;
            //    _context.Users.Update(user);
            //    _context.SaveChanges();
            //}

            //var test1 = new MedWorkerPatient
            //{
            //    MedWorkerId = user.Id,
            //    PatientId = "ce81a028-d783-48e2-a88f-9eca961afecb",
            //};

            //_context.MedWorkerPatients.Add(test1);

            //test1 = new MedWorkerPatient
            //{
            //    MedWorkerId = user.Id,
            //    PatientId = "ad7042bd-2b9e-4b0c-ac8d-af3a38278f0c",
            //};

            //_context.MedWorkerPatients.Add(test1);
            //_context.SaveChanges();


            var medPat = _context.MedWorkerPatients.Where(x => x.MedWorkerId == user.Id).ToList();
            var patient = new List<MainUser>();

            for (int i = 0; i < medPat.Count; i++)
            {
                patient.Add(_context.Users.Where(x => x.Id == medPat[i].PatientId).FirstOrDefault());
            }
            MedWorkerViewModel data = new MedWorkerViewModel
            {
                medWorker = user,
                patients = patient,
            };

            return View(data);
        }

        public async Task<IActionResult> AddFile(IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                string path = "/Files/" + uploadedFile.FileName;
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }

                MainUser current = await _userManager.GetUserAsync(User);

                FileInfo fileInfo = new FileInfo(_appEnvironment.WebRootPath + current.ProfilePicture);

                bool checkMe = fileInfo.Exists;

                if (checkMe)
                {
                    fileInfo.Delete();
                }

                current.ProfilePicture = path;

                _context.Users.Update(current);
                _context.SaveChanges();
            }

            return RedirectToAction("MedWorkerProfile", "MedWorker");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(MedWorkerViewModel user)
        {
            MedWorker current = (MedWorker)await _userManager.GetUserAsync(User);

            current.Name = user.medWorker.Name;
            current.FamilyName = user.medWorker.FamilyName;
            current.BirthDate = user.medWorker.BirthDate;
            current.Price = user.medWorker.Price;

            _context.Update(current);
            _context.SaveChanges();

            user.medWorker = current;

            return RedirectToAction("MedWorkerProfile", "MedWorker", user);
        }

        public async Task<IActionResult> GetPatient(string currentUser)
        {
            MedWorker user = (MedWorker)await _userManager.GetUserAsync(User);

            var patient = _context.Users
                .Where(x => x.Id == currentUser)
                .ToList();

            var patientFamily = _context.Users
                .Where(x => x.Id != currentUser && x.UserFamilyID == currentUser)
                .ToList();

            var recipe = "";

            MedWorkerViewModel data = new MedWorkerViewModel
            {
                medWorker = user,
                patients = patient,
                chosenFamily = patientFamily,
                Prescription = recipe
            };

            return View("ChosenPatient", data);
        }

        [HttpPost]
        public ActionResult GetGraph(string graphType, string patId)
        {
            ChosenPatientGraphViewModel cpgm = new ChosenPatientGraphViewModel
            {
                graphType = graphType,
                userID = patId,
            };

            return PartialView("~/Views/MedWorker/ChosenPatientGraphs.cshtml", cpgm);
        }

        [HttpGet]
        public JsonResult GetSpecifiedGraph(string graphType, int showEntries, string graphDate, string patId)
        {
            var stats = new GetUserStats(_context, graphType);
            var data = stats.GetGraphStats(patId, showEntries);

            return Json(data);
        }

        [HttpGet]
        public JsonResult GetSpecifiedMiniGraph(string graphType, int showEntries, string graphDate, string patId)
        {
            var stats = new GetUserStats(_context, graphType);

            var data = stats.GetMiniGraphStats(patId, showEntries, graphDate);
            return Json(data);
        }

        [HttpPost]
        public async Task<IActionResult> Send(string patID, string text)
        { 
            MedWorker user = (MedWorker)await _userManager.GetUserAsync(User);

            if(text != "")
            {
                var pat = (Patient)await _userManager.FindByIdAsync(patID);

                var record = new Prescription
                            {
                                MedWorkerId = user.Id,
                                PrescriptionText = text,
                                Date = DateTime.Now,
                                PatientId = pat.Id,
                             };

                _context.Prescriptions.Add(record);
                _context.SaveChanges();
            }

           

            var medPat = _context.MedWorkerPatients.Where(x => x.MedWorkerId == user.Id).ToList();
            var patient = new List<MainUser>();

            for (int i = 0; i < medPat.Count; i++)
            {
                patient.Add(_context.Users.Where(x => x.Id == medPat[i].PatientId).FirstOrDefault());
            }
            MedWorkerViewModel data = new MedWorkerViewModel
            {
                medWorker = user,
                patients = patient,
            };

            return RedirectToAction("MedWorkerProfile", "MedWorker", data);
        }

    }
}

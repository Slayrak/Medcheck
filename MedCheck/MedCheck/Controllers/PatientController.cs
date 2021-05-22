using MedCheck.DAL;
using MedCheck.Models;
using MedCheck.Models.ViewModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.IO;
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
        IWebHostEnvironment _appEnvironment;

        public PatientController(UserManager<MainUser> userManager, MedCheckContext context, IWebHostEnvironment appEnvironment)
        {
            _userManager = userManager;
            _context = context;
            _appEnvironment = appEnvironment;
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

            if(user.UserFamilyID == null)
            {
                user.UserFamilyID = user.Id;
                _context.Users.Update(user);
                _context.SaveChanges();
            }

            List<MainUser> patients = _context.Users
                .Where(x => x.Id != user.Id && x.UserFamilyID == user.UserFamilyID && x.UserFamilyID != null)
                .ToList();

            //List<Patient> patientToPass = (List<Patient>)patients.Cast<Patient>();

            PatientViewModel patient = new PatientViewModel
            {
                patient = user,
                sevm = stats,
                patients = patients
            };

            return View(patient);
        }

        public async Task<IActionResult> FamilyList()
        {
            Patient user = (Patient)await _userManager.GetUserAsync(User);

            if (user.UserFamilyID == null)
            {
                user.UserFamilyID = user.Id;
                _context.Users.Update(user);
                _context.SaveChanges();
            }

            List<MainUser> patients = _context.Users
                .Where(x => x.Id != user.Id && x.UserFamilyID == user.UserFamilyID && x.UserFamilyID != null)
                .ToList();

            //List<Patient> patientToPass = (List<Patient>)patients.Cast<Patient>();

            List<Requests> request = _context.Requests
                .Where(x => x.ReceiverId == user.Id && x.RequestStatus == true).ToList();

            List<MainUser> senders = new List<MainUser>();

            for (int i = 0; i < request.Count; i++)
            {
                var chosen = _context.Users.Where(u => u.Id == request[i].SenderID).SingleOrDefault();
                senders.Add(chosen);
            }

            FamilyViewModel patient = new FamilyViewModel
            {
                patient = user,
                patients = patients,
                requests = request,
                requestSender = senders,
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

            return RedirectToAction("Profile", "Patient", user);
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


            return RedirectToAction("Profile", "Patient", user);

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

            return RedirectToAction("Profile", "Patient");
        }

        //[HttpPost]
        public async Task<IActionResult> SearchForFamily(string email)
        {
            Patient user = (Patient)await _userManager.GetUserAsync(User);

            if (user.UserFamilyID == null)
            {
                user.UserFamilyID = user.Id;
                _context.Users.Update(user);
                _context.SaveChanges();
            }

            MainUser patient = _context.Users
                .Where(x => x.Id != user.Id && x.Email == email)
                .SingleOrDefault();

            SendViewModel svm = new SendViewModel
            {
                patientID = user.Id,
                patients = patient,
            };

            //List<Patient> patientToPass = (List<Patient>)patients.Cast<Patient>();

            //FamilyViewModel patient = new FamilyViewModel
            //{
            //    patient = user,
            //    patients = patients
            //};

            return PartialView("SearchForFamily",svm);
        }

        public async Task<IActionResult> RequestWasSent(string SenderID, string patient)
        {

            var check = _context.Requests.ToList();

            if (check.Count != 0)
            {
                Requests req = _context.Requests.Where(x => x.ReceiverId == patient && x.SenderID == SenderID).SingleOrDefault();

                if (req == null)
                {

                    req = new Requests
                    {
                        Patient = _context.Users.Where(x => x.Id == patient).FirstOrDefault(),
                        ReceiverId = patient,
                        SenderID = SenderID,
                        RequestStatus = true,
                    };

                    _context.Requests.Add(req);
                    _context.SaveChanges();
                }
            }
            else
            {
                Requests req = new Requests
                {
                    Patient = _context.Users.Where(x => x.Id == patient).FirstOrDefault(),
                    ReceiverId = patient,
                    SenderID = SenderID,
                    RequestStatus = true,
                };

                _context.Requests.Add(req);
                _context.SaveChanges();
            }

            Patient user = (Patient)await _userManager.GetUserAsync(User);

            if (user.UserFamilyID == null)
            {
                user.UserFamilyID = user.Id;
                _context.Users.Update(user);
                _context.SaveChanges();
            }

            List<MainUser> patients = _context.Users
                .Where(x => x.Id != user.Id && x.UserFamilyID == user.UserFamilyID && x.UserFamilyID != null)
                .ToList();

            List<Requests> request = _context.Requests
                .Where(x => x.ReceiverId == user.Id).ToList();

            List<MainUser> senders = new List<MainUser>();

            for (int i = 0; i < request.Count; i++)
            {
                var chosen = _context.Users.Where(u => u.Id == request[i].SenderID).SingleOrDefault();
                senders.Add(chosen);
            }

            FamilyViewModel patientData = new FamilyViewModel
            {
                patient = user,
                patients = patients,
                requests = request,
                requestSender = senders,
            };

            return RedirectToAction("FamilyList", "Patient", patientData);
        }

        public async Task<IActionResult> ChosenAnswer(bool choice, string currentUser, string chosenFamily)
        {
            if(choice == true)
            {
                MainUser mainUser = _context.Users.Where(x => x.Id == currentUser).FirstOrDefault();

                MainUser current = _context.Users.Where(x => x.Id == chosenFamily).FirstOrDefault();

                mainUser.UserFamilyID = current.UserFamilyID;

                _context.Users.Update(mainUser);
                _context.SaveChanges();

                Requests requestForChange = _context.Requests
                    .Where(r => r.SenderID == chosenFamily && r.ReceiverId == currentUser).FirstOrDefault();

                requestForChange.RequestStatus = false;

                _context.Requests.Update(requestForChange);
                _context.SaveChanges();
            }
            else
            {
                Requests requestForDelete = _context.Requests
                    .Where(r => r.SenderID == chosenFamily && r.ReceiverId == currentUser).FirstOrDefault();

                _context.Remove(requestForDelete);
                _context.SaveChanges();
            }

            Patient user = (Patient)await _userManager.GetUserAsync(User);

            if (user.UserFamilyID == null)
            {
                user.UserFamilyID = user.Id;
                _context.Users.Update(user);
                _context.SaveChanges();
            }

            List<MainUser> patients = _context.Users
                .Where(x => x.Id != user.Id && x.UserFamilyID == user.UserFamilyID && x.UserFamilyID != null)
                .ToList();

            List<Requests> request = _context.Requests
                .Where(x => x.ReceiverId == user.Id).ToList();

            List<MainUser> senders = new List<MainUser>();

            for (int i = 0; i < request.Count; i++)
            {
                var chosen = _context.Users.Where(u => u.Id == request[i].SenderID).SingleOrDefault();
                senders.Add(chosen);
            }

            FamilyViewModel patientData = new FamilyViewModel
            {
                patient = user,
                patients = patients,
                requests = request,
                requestSender = senders,
            };

            return RedirectToAction("FamilyList", "Patient", patientData);

        }

        public async Task<IActionResult> RemoveFromFamily(string currentUser, string chosenUser)
        {

            MainUser mainUser = _context.Users.Where(x => x.Id == chosenUser).FirstOrDefault();

            mainUser.UserFamilyID = mainUser.Id;

            Requests requests = _context.Requests.Where(x => x.SenderID == currentUser && x.ReceiverId == chosenUser).FirstOrDefault();

            _context.Users.Update(mainUser);
            _context.Requests.Remove(requests);
            _context.SaveChanges();

            Patient user = (Patient)await _userManager.GetUserAsync(User);

            if (user.UserFamilyID == null)
            {
                user.UserFamilyID = user.Id;
                _context.Users.Update(user);
                _context.SaveChanges();
            }

            List<MainUser> patients = _context.Users
                .Where(x => x.Id != user.Id && x.UserFamilyID == user.UserFamilyID && x.UserFamilyID != null)
                .ToList();

            List<Requests> request = _context.Requests
                .Where(x => x.ReceiverId == user.Id).ToList();

            List<MainUser> senders = new List<MainUser>();

            for (int i = 0; i < request.Count; i++)
            {
                var chosen = _context.Users.Where(u => u.Id == request[i].SenderID).SingleOrDefault();
                senders.Add(chosen);
            }

            FamilyViewModel patientData = new FamilyViewModel
            {
                patient = user,
                patients = patients,
                requests = request,
                requestSender = senders,
            };

            return RedirectToAction("FamilyList", "Patient", patientData);
        }

        [HttpGet]
        public JsonResult GetRecipe(string patientID)
        {
            var recipe = new GetUserRecipes(_context);
            var data = recipe.GetRecipe(patientID);

            return Json(data);
        }

    }
}

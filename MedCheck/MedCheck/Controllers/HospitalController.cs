using MedCheck.DAL;
using MedCheck.Models;
using MedCheck.Models.ViewModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedCheck.Controllers
{
    [Authorize(Roles = "admin")]
    public class HospitalController : Controller
    {
        private readonly MedCheckContext _context;

        public HospitalController(MedCheckContext context)
        {
            _context = context;
        }

        public IActionResult HospitalList()
        {
            return View(_context.Hospitals.ToList());
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Hospital hospital)
        {
            _context.Add(hospital);
            _context.SaveChanges();
            return RedirectToAction("HospitalList");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _context.Hospitals.Remove(_context.Hospitals.Single(x => x.HospitalId == id));
            _context.SaveChanges();

            return RedirectToAction("HospitalList");
        }

        [HttpGet]
        public IActionResult Edit(int HospitalId)
        {
            var hospital = _context.Hospitals.Where(x => x.HospitalId == HospitalId).FirstOrDefault();

            //var data = new HospitalViewModel
            //{
            //    SecretCode = hospital.SecretCode,
            //    Street = hospital.Street,
            //    City = hospital.City,
            //    District = hospital.District,
            //    Name = hospital.Name
            //};


            return View(hospital);
        }

        [HttpPost]
        public IActionResult Edit(Hospital hospital)
        {
            //var dataCheck = new Hospital
            //{
            //    Name = hospital.Name,
            //    Street = hospital.Street,
            //    District = hospital.District,
            //    SecretCode = hospital.SecretCode,
            //    City = hospital.City,
            //};

            _context.Entry(hospital).State = EntityState.Modified;
            _context.SaveChanges();

            return RedirectToAction("HospitalList");
        }
    }
}

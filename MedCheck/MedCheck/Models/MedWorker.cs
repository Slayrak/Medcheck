using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MedCheck.Models
{
    public class MedWorker : MainUser
    {
        public string HospitalCode { get; set; }
        public int Price { get; set; }
        public string Cabinet { get; set; }

        public ICollection<Speciality> Specialities { get; set; }
        public ICollection<Hospital> Hospitals { get; set; }
    }
}

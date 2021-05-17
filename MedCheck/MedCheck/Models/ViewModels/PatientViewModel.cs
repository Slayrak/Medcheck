using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedCheck.Models.ViewModels
{
    public class PatientViewModel
    {
        public Patient patient { get; set; }
        public StatsEntryViewModel sevm { get; set; }
    }
}

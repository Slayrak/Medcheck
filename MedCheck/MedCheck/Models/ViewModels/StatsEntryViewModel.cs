using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedCheck.Models.ViewModels
{
    public class StatsEntryViewModel
    {
        [Range(33.0, 43.0, ErrorMessage = "Temperature should be in range [33, 43]")]
        public double Temperature { get; set; }

        [Range(0, 300, ErrorMessage = "Blood pressure should be in range [0, 300]")]
        public int Pressure { get; set; }

        [Range(0, 100, ErrorMessage = "Oxygen level should be in range [0, 100]")]
        public int OxygenLevel { get; set; }

        [Range(0, 200, ErrorMessage = "Pulse should be in range [0, 200]")]
        public int Pulse { get; set; }

        public DateTime Date { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedCheck.Models.ViewModels
{
    public class SendViewModel
    {
        public string patientID { get; set; }

        public MainUser patients { get; set; }
    }
}

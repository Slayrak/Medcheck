using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedCheck.Models.ViewModels
{
    public class MedWorkerViewModel
    {
        public MedWorker medWorker { get; set; }

        public List<MainUser> patients { get; set; }

        public List<MainUser> chosenFamily { get; set; }

        public string Prescription { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedCheck.Models.ViewModels
{
    public class FamilyViewModel
    {
        public Patient patient { get; set; }

        public List<MainUser> patients { get; set; }

        public List<MainUser> requestSender { get; set; }

        public List<Requests> requests { get; set; }
    }
}

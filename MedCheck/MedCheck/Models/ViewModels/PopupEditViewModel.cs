using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedCheck.Models.ViewModels
{
    public class PopupEditViewModel
    {
        public string Name { get; set; }
        public string FamilyName { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
    }
}

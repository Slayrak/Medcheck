using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MedCheck.Models
{
    public class Prescription
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PrescriptionId { get; set; }

        public string PrescriptionText { get; set; }

        public Patient Patient { get; set; }

        public MedWorker MedWorker { get; set; }
    }
}

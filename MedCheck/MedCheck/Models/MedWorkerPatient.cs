using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MedCheck.Models
{
    public class MedWorkerPatient
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long StatsId { get; set; }

        [Required]
        public string PatientId { get; set; }

        [Required]
        public string MedWorkerId { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MedCheck.Models
{
    public class Stats
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long StatsId { get; set; }

        [Required]
        [ForeignKey(nameof(Patient))]
        public string UserId { get; set; }

        [Required]
        public double Temperature { get; set; }

        [Required]
        public double Pressure { get; set; }

        [Required]
        public double OxygenLevel { get; set; }

        [Required]
        public double Pulse { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public Patient Patient { get; set; }

    }
}

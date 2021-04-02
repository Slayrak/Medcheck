using Microsoft.AspNetCore.Identity;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MedCheck.Models
{
    public class Patient : MainUser
    {
    //    [Key]
    //    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    public long UserId { get; set; }

    //    [Required]
    //    public string FullName { get; set; }

    //    [Required]
    //    public string Email { get; set; }

    //    [Required]
    //    public string Password { get; set; }

    //    [Required]
    //    public string Salt { get; set; }

// public long FamilyID { get; set; }

        public ICollection<Hospital> Hospitals { get; set; }
        public ICollection<Stats> Stats { get; set; }
        public ICollection<MedWorker> MedWorkers { get; set; }
    }
}

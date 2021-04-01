﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MedCheck.Models
{
    public class MedWorker
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long MedId { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Salt { get; set; }

        public byte[] ProfilePicture { get; set; }

        public ICollection<Speciality> Specialities { get; set; }
        public ICollection<Hospital> Hospitals { get; set; }
        public ICollection<User> Users { get; set; }
    }
}

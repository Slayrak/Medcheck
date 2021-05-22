using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MedCheck.Models
{
    public class Requests
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long RequestId { get; set; }

        [Required]
        public bool RequestStatus { get; set; }

        [Required]
        public string SenderID { get; set; }

        [Required]
        [ForeignKey(nameof(Patient))]
        public string ReceiverId { get; set; }

        public MainUser Patient { get; set; }

    }
}

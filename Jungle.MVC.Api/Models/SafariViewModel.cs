using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Jungle.MVC.Api.Models
{
    public class SafariViewModel
    {
        [Required]
        [Display(Name = "Id")]
        public int SafariId { get; set; }

        [Required]
        [Display(Name = "Safari Name")]
        public string SafariName { get; set; }

        [Required]
        [Display(Name = "Available Date")]
        [DataType(DataType.Date)]
        public DateTime SafariDate { get; set; }

        [Required]
        [Display(Name = "Time Slot")]
        public Slot SafariTime { get; set; }

        [Required]
        public int ParkId { get; set; }

        [Required]
        [Display(Name = "Safari Cost")]
        [Range(100, double.MaxValue, ErrorMessage = "Safari Cost should be > 100")]
        public decimal SafariCost { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Jungle.MVC.Api.Models
{
    public class VehicleViewModel
    {
        [Required]
        [Display(Name = "Id")]
        public int Vid { get; set; }

        [Required]
        [Display(Name = "Vehicle Type")]
        public string Vtype { get; set; }

        [Required]
        [Display(Name = "Vehicle Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Vehicle Entry Cost")]
        [Range(100, double.MaxValue, ErrorMessage = "Vehicle Entry Cost should be > 100")]
        public decimal EntryCost { get; set; }

        [Required]
        [Display(Name = "Seating Capacity")]
        public capacity Capacity { get; set; }

        [Required]
        public int ParkId { get; set; }
    }
}

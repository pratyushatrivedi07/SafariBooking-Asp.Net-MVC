using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Jungle.Entities
{
    public partial class Vehicle
    {
        public Vehicle()
        {
            Booking = new HashSet<Booking>();
            Payment = new HashSet<Payment>();
        }

        [Required]
        [Display(Name="Id")]
        public int Vid { get; set; }

        [Required]
        [Display(Name ="Vehicle Type")]
        public string Vtype { get; set; }

        [Required]
        [Display(Name="Vehicle Name")]
        [RegularExpression("^[A-Z][a-zA-Z]*$", ErrorMessage = "Enter correct Vehicle Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name ="Vehicle Entry Cost")]
        [Range(100, double.MaxValue,ErrorMessage ="Vehicle Entry Cost should be > 100")]
        public decimal EntryCost { get; set; }

        [Required]
        [Display(Name="Seating Capacity")]
        public string Capacity { get; set; }

        [Required]
        public int ParkId { get; set; }

        public virtual Parks Park { get; set; }

        public virtual ICollection<Booking> Booking { get; set; }
        public virtual ICollection<Payment> Payment { get; set; }
    }
}

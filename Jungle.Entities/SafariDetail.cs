using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Jungle.Entities
{
    public partial class SafariDetail
    {
        public SafariDetail()
        {
            Booking = new HashSet<Booking>();
            Payment = new HashSet<Payment>();
        }

        [Required]
        [Display(Name = "Id")]
        public int SafariId { get; set; }

        [Required]
        [Display(Name = "Safari Name")]
        [RegularExpression("^[A-Z][a-zA-Z]*$", ErrorMessage = "Enter correct Safari Name")]
        public string SafariName { get; set; }

        [Required]
        [Display(Name = "Available Date")]
        [DataType(DataType.Date)]
        public DateTime SafariDate { get; set; }

        [Required]
        [Display(Name ="Time Slot")]
        public string SafariTime { get; set; }

        [Required]
        public int ParkId { get; set; }

        [Required]
        [Display(Name ="Safari Cost")]
        [Range(100, double.MaxValue, ErrorMessage ="Safari Cost should be > 100")]
        public decimal SafariCost { get; set; }

        public virtual Parks Park { get; set; }
        public virtual ICollection<Booking> Booking { get; set; }
        public virtual ICollection<Payment> Payment { get; set; }
    }
}

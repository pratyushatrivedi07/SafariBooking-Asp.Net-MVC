using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Jungle.Entities
{
    public partial class Parks
    {
        public Parks()
        {
            Booking = new HashSet<Booking>();
            Gate = new HashSet<Gate>();
            Payment = new HashSet<Payment>();
            SafariDetail = new HashSet<SafariDetail>();
        }

        [Display(Name ="Id")]
        public int ParkId { get; set; }

        [Required]
        [Display(Name = "Park Name")]
        [RegularExpression("^[A-Z][a-zA-Z]*$", ErrorMessage = "Enter correct Park Name")]
        public string Name { get; set; }

        [Required]
        [RegularExpression("^[A-Z][a-zA-Z]*$", ErrorMessage = "Enter correct Park Location")]
        public string Location { get; set; }

        [Required]
        [Range(10,double.MaxValue, ErrorMessage ="Entry Fee should be > 10")]
        [Display(Name = "Entry Fee")]
        public decimal Fee { get; set; }

        [JsonIgnore]
        public virtual ICollection<Booking> Booking { get; set; }
        [JsonIgnore]
        public virtual ICollection<Gate> Gate { get; set; }
        [JsonIgnore]
        public virtual ICollection<Payment> Payment { get; set; }
        [JsonIgnore]
        public virtual ICollection<SafariDetail> SafariDetail { get; set; }
    }
}

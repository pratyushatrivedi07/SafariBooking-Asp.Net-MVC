using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Jungle.Entities
{
    public partial class Gate
    {
        public Gate()
        {
            Booking = new HashSet<Booking>();
        }

        [Required]
        [Display(Name="Id")]
        public int GateId { get; set; }

        [Required]
        [Display(Name = "Gate Name")]
        [RegularExpression("^[A-Z][a-zA-Z]*$", ErrorMessage = "Enter correct Gate Name")]
        public string Name { get; set; }

        [Required]
        public int ParkId { get; set; }

        public virtual Parks Park { get; set; }

        [JsonIgnore]
        public virtual ICollection<Booking> Booking { get; set; }
    }
}

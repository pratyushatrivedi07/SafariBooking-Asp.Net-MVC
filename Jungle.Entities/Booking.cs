using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Jungle.Entities
{
    public partial class Booking
    {
        [Required]
        public int Id { get; set; }
        public string Status { get; set; }

        [Required]
        public int Pid { get; set; }

        [Required]
        public int SafariId { get; set; }

        [Required]
        public int GateId { get; set; }

        [Required]
        public int VehicleId { get; set; }

        public decimal TotalCost { get; set; }

        public virtual Gate Gate { get; set; }
        public virtual Parks P { get; set; }
        public virtual SafariDetail Safari { get; set; }
        public virtual Vehicle Vehicle { get; set; }
    }
}

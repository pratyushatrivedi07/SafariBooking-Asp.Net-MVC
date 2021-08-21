using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Jungle.Entities
{
    public partial class Payment
    {
        public int BookingId { get; set; }
        public int PayId { get; set; }

        [Display(Name ="Total Cost")]
        public decimal Total { get; set; }
        public int TouristId { get; set; }

        [Display(Name = "Tourist Name")]
        public string TouristName { get; set; }
        public string  Email { get; set; }
        public string MobileNo { get; set; }
        public int ParkId { get; set; }

        [Display(Name = "Park Name")]
        public string ParkName { get; set; }

        [Display(Name = "Park Fee")]
        public decimal ParkCost { get; set; }
        public int SafariId { get; set; }

        [Display(Name = "Safari Name")]
        public string SafariName { get; set; }

         [Display(Name = "Safari Fee")]
        public decimal SafariCost { get; set; }
        public int VehicleId { get; set; }

        [Display(Name = "Vehicle Name")]
        public string VehicleName { get; set; }

        [Display(Name = "Vehicle Fee")]
        public decimal VehicleCost { get; set; }

        public int GateId { get; set; }

        [Display(Name = "Gate Name")]
        public string GateName { get; set; }

        public int People { get; set; }

        public virtual Parks Park { get; set; }
        public virtual SafariDetail Safari { get; set; }
        public virtual Tourist Tourist { get; set; }
        public virtual Vehicle Vehicle { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace Jungle.MVC.Api.Models
{
    public class BookingViewModel
    {
        public int BookingId { get; set; }

        [Display(Name = "Total Cost")]
        public decimal TotalCost { get; set; }
        public int ParkId { get; set; }
        
        [Display(Name ="Park Name")]
        public string ParkName { get; set; }
        public int SafariId { get; set; }

        [Display(Name = "Safari Name")]
        public string SafariName { get; set; }
        [Required]
        public int gateId { get; set; }

        [Display(Name = "Gate Name")]
        public string GateName { get; set; }
        [Required]
        public int VId { get; set; }

        [Display(Name = "Vehicle Name")]
        public string VName { get; set; }

        [Required]
        [Display(Name = "Number of people")]
        [Range(0,maximum:6)]
        public int People { get; set; }

        [Required]
        [Display(Name = "Your Name")]
        public string Name { get; set; }
        [Required]
        public Gender Gender { get; set; }

        [Required]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [Display(Name = "Mobile Number")]
        [RegularExpression(@"^[6-9]\d{9}$",ErrorMessage ="Please enter correct Mobile Number") ]
        public double MobileNo { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        [EmailAddress(ErrorMessage ="Invalid Email")]
        [Display(Name="Email")]
        public string EmailId { get; set; }

        [Required]
        [Display(Name = "Identity Proof")]
        public string Identityproof { get; set; }

        [Required]
        [Display(Name = "Identity Proof Number")]
        public string Identitynumber { get; set; }

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{ 
        //    Regex a = new Regex("^[2-9]{1}[0-9]{3}\\s[0-9]{4}\\s[0-9]{4}$");
        //    Regex b = new Regex("[A-Z]{5}[0-9]{4}[A-Z]{1}");
        //    Regex c = new Regex("[A-PR-WYa-pr-wy][1-9]\\d\\s?\\d{4}[1-9]$");
        //    if (!a.IsMatch(Identitynumber) || !b.IsMatch(Identitynumber) || !c.IsMatch(Identitynumber))
        //    {
        //        yield return new ValidationResult("Incorrect Identity number", new string[] { nameof(Identitynumber) });

        //    }

        //}

    }
}

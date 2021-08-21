using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Jungle.Entities
{
    public partial class Tourist
    {
        public Tourist()
        {
            Payment = new HashSet<Payment>();
        }

      
        public int Id { get; set; }

        [Required]
        [Display(Name="Your Name")]
        [RegularExpression("^[A-Z][a-zA-Z]*$", ErrorMessage = "Enter correct Name")]
        public string Name { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        [Display(Name ="Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }


        [Required]
        [Display(Name = "Mobile Number")]
        public string MobileNo { get; set; }

        [Required]
        [RegularExpression("^[A-Z][a-zA-Z]*$", ErrorMessage = "Enter correct City")]
        public string City { get; set; }

        [Required]
        [RegularExpression("^[A-Z][a-zA-Z]*$", ErrorMessage = "Enter correct Country")]
        public string Country { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string EmailId { get; set; }

        [Required]
        [Display(Name = "Identity Proof")]
        public string IdentityName { get; set; }

        [Required]
        [Display(Name = "Identity Proof Number")]
        public string IdentityNumber { get; set; }

        public virtual ICollection<Payment> Payment { get; set; }

       
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TrashCollection.Models
{

    public class Employee
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("ApplicationUser")]

        [Display(Name = "Please Verify your Email Address")]
        public string ApplicationId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Display(Name = "Your Designated Area Code")]
        public int ZipCode { get; set; } 
    }
}
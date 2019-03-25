using Bank.Models;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.ViewModels
{
    [NotMapped]
    public class Account_Register_VM : User
    {
        [Required]
        [RegularExpression("^[0-9-]{12}$", ErrorMessage = "Telephone may only contain numeric values in the current format 555-555-5555.")]
        public new string Telephone { get; set; }

        //[StringLength(32, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)] //JDR: Ser range for length
        //JDR: Set format for password
        //[RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$",
        //                    ErrorMessage = "Passwords must be at least 8 characters and contain at 3 of 4 of the following: upper case (A-Z), lower case (a-z), number (0-9) and special character (e.g. !@#$%^&*)")]
        public new string Password { get; set; }

        [Required]
        [StringLength(50)]
        public string Address { get; set; }

        [Required]
        [StringLength(25)]
        public string City { get; set; }

        [Required]
        [StringLength(25)]
        public string State { get; set; }

        [Required]
        [DisplayName("Zip Code")]
        [StringLength(5, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 5)]
        public string Zip { get; set; }
    }
}

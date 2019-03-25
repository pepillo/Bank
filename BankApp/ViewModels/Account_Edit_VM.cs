using Bank.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.ViewModels
{
    [NotMapped]
    public class Account_Edit_VM
    {
        [Required]
        [RegularExpression("^[0-9-]{12}$", ErrorMessage = "Telephone may only contain numeric values in the current format 555-555-5555.")]
        public string Telephone { get; set; }

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

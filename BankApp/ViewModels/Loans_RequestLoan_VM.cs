using Bank.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.ViewModels
{
    [NotMapped]
    public class Loans_RequestLoan_VM : LoanRequest
    {
        [Required]
        [RegularExpression("^[0-9-]{11}$", ErrorMessage = "Social Security format: ###-##-####.")]
        public new string SocialSecurity { get; set; }

        [Required]
        public new double? Income { get; set; }

        [Required]
        public new string Employer { get; set; }

        [Required]
        public new string JobTitle { get; set; }

        [Required]
        public new string EmploymentType { get; set; }

        [Required]
        public new double? Amount { get; set; }

        [Required]
        public new int? Terms { get; set; }

        [Required]
        public new double? DownPayment { get; set; }

        [Required]
        public new string LoanType { get; set; }
    }
}

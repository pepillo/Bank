namespace Bank.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    [Table("LoanRequest")]
    public partial class LoanRequest : DatabaseStruct.BaseDB
    {
        public LoanRequest() : base("Bank.Models.BankDB", "Bank.Models.LoanRequest")
        {

        }

        [Key]
        public override int ID { get; set; }

        public int UserID { get; set; }
        public int? Terms { get; set; }

        [StringLength(11)]
        [DisplayName("Social Security")]
        public string SocialSecurity { get; set; }

        [DataType(DataType.Currency)]
        [Range(0.0, 1000000)]
        public double? Income { get; set; }

        [StringLength(25)]
        public string Employer { get; set; }

        [StringLength(25)]
        [DisplayName("Job Title")]
        public string JobTitle { get; set; }

        [StringLength(10)]
        [DisplayName("Employment Type")]
        public string EmploymentType { get; set; }

        [StringLength(10)]
        public string Status { get; set; }

        [DataType(DataType.Currency)]
        [Range(0.0, 1000000)]
        public double? Amount { get; set; }

        [DisplayName("Down Payment")]
        public double? DownPayment { get; set; }

        [StringLength(10)]
        [DisplayName("Loan Type")]
        public string LoanType { get; set; }

        //JDR: Get all loan request for spesific user
        public static ICollection<LoanRequest> GetLoanRequestsByUserID(int UserID)
        {
            using (BankDB db = new BankDB())
            {
                return db.LoanRequest.OrderByDescending(Row => Row.ID).Where(Row => Row.UserID == UserID).ToList();
            }
        }

        //JDR: Get LoanResults For spesific LoanRequest
        public LoanResult GetLoanResults()
        {
            using (BankDB db = new BankDB())
            {
                LoanResult Result = db.LoanResult.Where(Row => Row.RequestID == this.ID).FirstOrDefault();

                return Result;
            }
        }
    }
}

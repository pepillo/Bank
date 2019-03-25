namespace Bank.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class LoanApprovalRule : DatabaseStruct.BaseDB
    {
        public LoanApprovalRule() : base("Bank.Models.BankDB", "Bank.Models.LoanApprovalRule")
        {

        }

        [Key]
        public override int ID { get; set; }

        public int MinimumScore { get; set; }

        public int MaximumScore { get; set; }

        public double? DownPaymentReq { get; set; }

        public double? DownPaymentPct { get; set; }

        public double APR { get; set; }

        [Required]
        [StringLength(10)]
        public string LoanType { get; set; }
    }
}

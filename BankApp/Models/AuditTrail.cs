namespace Bank.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AuditTrail")]
    public partial class AuditTrail
    {
        public int ID { get; set; }

        public int ActionID { get; set; }

        public int RequestID { get; set; }

        public int UserID { get; set; }

        public DateTime? TimeStamp { get; set; }

        public virtual LoanRequest LoanRequest { get; set; }
    }
}

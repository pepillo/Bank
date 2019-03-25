namespace Bank.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Address")]
    public partial class Address : DatabaseStruct.BaseDB
    {
        public Address() : base("Bank.Models.BankDB", "Bank.Models.Address")
        {

        }

        [Key]
        public override int ID { get; set; }

        [Required]
        [StringLength(50)]
        [Column("Address")]
        public string Address1 { get; set; }

        [Required]
        [StringLength(25)]
        public string City { get; set; }

        [Required]
        [StringLength(25)]
        public string State { get; set; }

        [Required]
        [StringLength(25)]
        public string ZipCode { get; set; }
    }
}

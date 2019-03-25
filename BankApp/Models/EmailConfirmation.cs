namespace Bank.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EmailConfirmation")]
    public partial class EmailConfirmation :DatabaseStruct.BaseDB
    {
        public static readonly string Salt = "SomeEmailSalt";

        public EmailConfirmation() : base("Bank.Models.BankDB", "Bank.Models.EmailConfirmation")
        {

        }

        [Key]
        public override int ID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        [StringLength(50)]
        public string Token { get; set; }

        public static dynamic SetEmailConfirmation(int ID)
        {
            EmailConfirmation Confirmation = new EmailConfirmation().New();
            Confirmation.UserID = ID;
            Confirmation.Token = GLOBAL.Hash(ID + DateTime.Now.ToString(), EmailConfirmation.Salt);

            Confirmation.Add();

            return Confirmation;
        }
    }
}

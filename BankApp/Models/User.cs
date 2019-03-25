namespace Bank.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Linq;
    using System.Web;

    [Table("User")]
    public partial class User : DatabaseStruct.BaseDB
    {
        public User() : base("Bank.Models.BankDB", "Bank.Models.User")
        {

        }

        //-------------------- Define Model Elements --------------------
        [Key]
        public override int ID { get; set; }

        public int? AddressID { get; set; }
        public int? UserStatus { get; set; }
        public DateTime? CreatedDate { get; set; }

        [Required]
        [StringLength(50)]
        [EmailAddress(ErrorMessage = "Please enter a vaild Email.")]
        public string Email { get; set; }

        [Required]
        [StringLength(25)]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(25)]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [StringLength(25)]
        [DisplayName("Initial")]
        public string MiddleName { get; set; }

        [StringLength(12)]
        [DisplayName("Phone Number")]
        public string Telephone { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public new bool Save()
        {
            this.Email = this.Email.Trim();

            return base.Save();
        }

        public static User GetByEmail(string Email)
        {
            using (BankDB db = new BankDB())
            {
                User User = db.User.Where(Row => Row.Email == Email).FirstOrDefault();
                return User;
            }
        }

        //JDR: Get all loan request for spesific user
        public ICollection<LoanRequest> GetLoanRequests()
        {
            return LoanRequest.GetLoanRequestsByUserID(this.ID);
        }

        //JDR: Get user credit score
        public CreditScore GetCreditScore()
        {
            return CreditScore.GetByUserID(this.ID);
        }

        //JDR: Overload 1: Get user Address
        public dynamic GetAddress(bool ReturnFormated = false)
        {
            if (this.AddressID == null && ReturnFormated == false) return null;
            if (this.AddressID == null && ReturnFormated == true)  return new string[] { "", "" };

            Address Address = new Address().GetByID((int)this.AddressID);

            if (ReturnFormated == true)
            {
                return new string[] { Address.Address1, Address.City + " " + Address.State + ", " + Address.ZipCode };
            }

            return Address;
        }

        //------------------------ Define Model Methods ------------------------
        //JDR: Get user full name
        public string FullName()
        {
            string name = this.FirstName.Trim()  + " " +
                          (this.MiddleName ?? "").Trim() + " " +
                          this.LastName.Trim();

            //JDR: Format Name
            //name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name.ToLower());

            return name;
        }
    }
}

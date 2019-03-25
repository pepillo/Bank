namespace Bank.Models
{
    using Newtonsoft.Json.Linq;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Net;

    [Table("CreditScore")]
    public partial class CreditScore : DatabaseStruct.BaseDB
    {
        public CreditScore() : base("Bank.Models.BankDB", "Bank.Models.CreditScore")
        {

        }

        //JDR: PUBLIC PROPERTIES ###########################################################################
        [Key]
        public override int  ID { get; set; }

        public int  UserID      { get; set; }

        public int? TransUnion  { get; set; }

        public int? Equifax     { get; set; }

        [Column(TypeName = "date")]
        public DateTime? LastUpdatedDate { get; set; }

        //JDR: Get user credit score
        public static CreditScore GetByUserID(int UserID)
        {
            using (BankDB db = new BankDB())
            {
                var CreditScore = db.CreditScore.
                                  OrderByDescending(Row => Row.ID).
                                  Where(Row => Row.UserID == UserID).
                                  FirstOrDefault();

                if (CreditScore == null) return CreditScore.UpdateCreditScore(UserID);

                return CreditScore;
            }
        }

        //JDR: Insert new creditScore in DB
        public static CreditScore UpdateCreditScore(int UserID)
        {
            //JDR: Call Api
            using (WebClient wc = new WebClient())
            {
                var json = wc.DownloadString("https://dxc-banking-api.herokuapp.com/getCReditScore");
                JObject jsonObj = JObject.Parse(json);

                //JDR: More Validation Login on response from API
                //...LOGIC...

                var Score = (int)jsonObj["creditScore"];

                var CreditScore = new CreditScore().New();

                CreditScore.UserID          = UserID;
                CreditScore.Equifax         = Score;
                CreditScore.TransUnion      = Score;
                CreditScore.LastUpdatedDate = DateTime.Now;

                CreditScore.Save();

                return CreditScore;
            }
        }
    }
}

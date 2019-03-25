namespace Bank.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class LoanResult : DatabaseStruct.BaseDB
    {
        public LoanResult() : base("Bank.Models.BankDB", "Bank.Models.LoanResult")
        {

        }

        [Key]
        public override int ID { get; set; }

        public int RequestID { get; set; }

        public double? Amount { get; set; }

        public double APR { get; set; }

        public double? MonthlyPayment { get; set; }

        public int? CreditScore { get; set; }

        public int? Term { get; set; }

        public double? SuggestedDownPayment { get; set; }

        [StringLength(10)]
        public string Decision { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ResolutionDate { get; set; }

        [StringLength(250)]
        public string Comments { get; set; }

        //JDR: Calculate if Loan is approced or denied depending Debt To Income vs LoanType
        public static bool IsLoanApproved(double DebtToIncome, string LoanType)
        {
            double DebtToIncomePercent = DebtToIncome;
            double AvailableIncomePercentRequierd = 100;

            switch (LoanType)
            {
                case "HL":
                    AvailableIncomePercentRequierd = 43;
                    break;
                case "AL":
                    AvailableIncomePercentRequierd = 36;
                    break;
                case "PL":
                    AvailableIncomePercentRequierd = 45;
                    break;
            }

            AvailableIncomePercentRequierd = AvailableIncomePercentRequierd / 100;
            if (DebtToIncomePercent > 1) DebtToIncome = DebtToIncomePercent / 100;

            double Available = 1 - DebtToIncomePercent;

            return (Available > AvailableIncomePercentRequierd) ? true : false;
        }

        //JDR: Calculate Debt To Income
        public static double CalculateDebtToIncomePercent(double Income, double Debt, string IncomeType = "year", string DebtType = "month")
        {
            double IncomeAmount = Income;
            double DebtAmount = Debt;

            switch (IncomeType)
            {
                case "month":
                    break;
                case "year":
                    IncomeAmount = IncomeAmount / 12;
                    break;

            }

            switch (DebtType)
            {
                case "month":
                    break;
                case "year":
                    DebtAmount = DebtAmount / 12;
                    break;

            }

            double DebtToIncomePercent = (DebtAmount) / (IncomeAmount);

            return DebtToIncomePercent;
        }

        //JDR: Calculate onthly Payment
        public static double CalculateMonthlyPayment(dynamic Object)
        {
            int Terms = (int) Object.Terms;

            if (Object.APR > 1.0) Object.APR = Object.APR / 100;

            double Amount      = (double) Object.Amount;
            double DownPayment = (double) Object.DownPayment;
            double MonthlyAPR  = (double) (Object.APR / 12.0);

            var MP = (Amount - DownPayment) *
                      (
                        (
                            MonthlyAPR * Math.Pow((1.0 + MonthlyAPR), Terms)
                        ) / (
                            Math.Pow((1.0 + MonthlyAPR), Terms) - 1.0
                        )
                      );

            ///*//JDR: Formula Monthly Payment, same result another equation
            var Rate        = (double)Object.APR / 12 ;
            var Denominator = Math.Pow((1 + Rate), (double)Object.Terms) - 1;
            MP          = (Rate + (Rate / Denominator)) * (double)Object.Amount;
            //*/

            return Math.Round(MP, 2);
        }

        //JDR: Calculate APR
        public static double CalculateAPR(int CreditScore, int Term)
        {
            var APR = 0;

            //JDR: Set APR depending on term and score
            if (Term <= 12)
            {
                if      (CreditScore <= 600) { APR = 20; }  //Term: 1 yr, Score: (0   - 600)
                else if (CreditScore <= 650) { APR = 16; }  //Term: 1 yr, Score: (601 - 650)
                else if (CreditScore <= 700) { APR = 12; }  //Term: 1 yr, Score: (651 - 700)
                else                         { APR = 4;  }  //Term: 1 yr, Score: (701 - 750)
            }
            else if (Term <= 24)
            {
                if      (CreditScore <= 600) { APR = 18; }  //Term: 2 yr, Score: (0   - 600)
                else if (CreditScore <= 650) { APR = 14; }  //Term: 2 yr, Score: (601 - 650)
                else if (CreditScore <= 700) { APR = 10; }  //Term: 2 yr, Score: (651 - 700)
                else                         { APR = 4;  }  //Term: 2 yr, Score: (701 - 750)
            }
            else if (Term <= 36)
            {
                if      (CreditScore <= 600) { APR = 16; }  //Term: 3 yr, Score: (0   - 600)
                else if (CreditScore <= 650) { APR = 12; }  //Term: 3 yr, Score: (601 - 650)
                else if (CreditScore <= 700) { APR = 8;  }  //Term: 3 yr, Score: (651 - 700)
                else                         { APR = 4;  }  //Term: 3 yr, Score: (701 - 750)
            }
            else if (Term <= 60)
            {
                if      (CreditScore <= 600) { APR = 14; }  //Term: 5 yr, Score: (0   - 600)
                else if (CreditScore <= 650) { APR = 10; }  //Term: 5 yr, Score: (601 - 650)
                else if (CreditScore <= 700) { APR = 6;  }  //Term: 5 yr, Score: (651 - 700)
                else                         { APR = 4;  }  //Term: 5 yr, Score: (701 - 750)
            }
            else
            {
                APR = 20;                                   //Term: 5 < yr, Score: (0 - X)
            }

            return ((double)APR / (double)100); ;
        }
    }
}

//JDR: Calculations Specs
/*
    APR = APR %
    DP  = Down Payment(Auto, House)
    I   = Income
    CS  = Credit Score
    A   = Amount
    T   = Term
    MP  = Monthly Payment

    o	Calculate APR %
            Data will be provided that will included the APR % depending
            on the payment terms and the credit score

    o	Loan Calculation formula
            The following is the formula that will be used to calculate
            a loan when the user submits a loan request

            Formula: MP =  (A - DP) * { (APR/12) * [1 + (APR/12)]^T } / { [1 + (APR/12)]^T - 1 }

    o	Calculate debt-to-income
            Formula to calculate the amount a user has available to get a loan based on its income.
                �	Home Loan = 43% available
                �	Car Loan = 36%
                �	Personal Loan = 45%

                Formula: DebtToIncome = MP / (I/12) * loan %

*/

using Bank.Models;
using Bank.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bank.Controllers
{
    public class LoansController : Controller
    {
        // GET: Loan
        public ActionResult Index()
        {
            //return RedirectToAction("Index", "Home");    //JDR: Redirect to HomePage
            return View("~/Views/Home/Index.cshtml");
        }

        public ActionResult RequestLoan()
        {
            if (!UserAccount.IsLoggedIn(this)) return RedirectToAction("SendToLogin", "Account", new { Something = "" });

            return View();
        }

        [HttpPost]
        public ActionResult RequestLoan(Loans_RequestLoan_VM Model)
        {
            if (!UserAccount.IsLoggedIn(this)) return RedirectToAction("SendToLogin", "Account", new { Something = "" });

            if (!ModelState.IsValid) return View(Model);

            int UserID = (UserAccount.ActiveUser != null) ? UserAccount.ActiveUser.ID : 1027/*For Debug*/;

            var Loan = new  LoanRequest().New();

            Loan.UserID         = UserID;
            Loan.SocialSecurity = Model.SocialSecurity;
            Loan.Income         = Model.Income;
            Loan.Employer       = Model.Employer;
            Loan.JobTitle       = Model.JobTitle;
            Loan.EmploymentType = Model.EmploymentType;
            Loan.Amount         = Model.Amount;
            Loan.Terms          = Model.Terms;
            Loan.DownPayment    = Model.DownPayment;
            Loan.LoanType       = Model.LoanType;
            Loan.Status         = "Pending";

            if (!Loan.Save())
            {
                TempData["AlertTag"]     = "danger";
                TempData["AlertLabel"]   = "Error: ";
                TempData["AlertMessage"] = "Loan request could not be saved.";

                return View(Model);
            }

            var Score = CreditScore.GetByUserID(UserID).TransUnion;

            //JDR: Call Api to calclate Result
            // ... Code ...

            var LoanProperties = new
            {
                Terms       = Model.Terms,
                Amount      = Model.Amount,
                Income      = Model.Income,
                DownPayment = Model.DownPayment,
                CreditScore = Score,
                APR         = LoanResult.CalculateAPR((int)Score, (int)Model.Terms),
            };

            var Result = new LoanResult().New();

            Result.RequestID        = Loan.ID;
            Result.Amount           = Model.Amount;
            Result.Term             = Model.Terms;
            Result.ResolutionDate   = DateTime.Now;
            Result.CreditScore      = LoanProperties.CreditScore;
            Result.APR              = LoanProperties.APR;
            Result.MonthlyPayment   = LoanResult.CalculateMonthlyPayment(LoanProperties);
            Result.Decision         = "Pending";
            Result.Comments         = "Loan has been recived and is pending review.";

            double IncomeYrTotal  = (double) Model.Income;
            double DebtMonthTotal = (double) Result.MonthlyPayment;

            var UserLoans = LoanRequest.GetLoanRequestsByUserID(UserID);

            //JDR: Factor in other loans to debt equation
            foreach (LoanRequest LoanElement in UserLoans)
            {
                //if (!LoanElement.Active()) continue;
                if (LoanElement.Status.Trim() != "Approved") continue;

                LoanResult ResultElement = LoanElement.GetLoanResults();

                if (ResultElement == null) continue;

                DebtMonthTotal += (double)ResultElement.MonthlyPayment;
            }

            var DebtToIncome = LoanResult.CalculateDebtToIncomePercent(IncomeYrTotal, DebtMonthTotal);

            //JDR: Save Loan approved/denied result
            if (LoanResult.IsLoanApproved(DebtToIncome, Model.LoanType))
            {
                Loan.Status     = "Approved";
                Result.Decision = "Approved";
                Result.Comments = "Loan has been approved for a total of " + Result.Amount.ToString("$###,###.##") + ".";
            }
            else
            {
                Loan.Status     = "Denied";
                Result.Decision = "Denied";
                Result.Comments = "Loan has beed denied do to debt to income ratio.";

                Result.APR = 0.0;
                Result.MonthlyPayment = 0.0;
            }

            Loan.Save();

            if (!Result.Save())
            {
                TempData["AlertTag"] = "danger";
                TempData["AlertLabel"] = "Error: ";
                TempData["AlertMessage"] = "Loan request cound not save/compute result of request.";

                return RedirectToAction("Dashboard", "Account");
            }

            /*//DEBUG
            TempData["AlertMessage"] = "";
            TempData["AlertMessage"] += DEBUG.ObjectToString(Loan)           + "<hr>";
            TempData["AlertMessage"] += DEBUG.ObjectToString(Result)         + "<hr>";
            TempData["AlertMessage"] += DEBUG.ObjectToString(LoanProperties) + "<hr>";
            TempData["AlertMessage"] += "Result.MonthlyPayment   = " + Result.MonthlyPayment + "<hr>";
            TempData["AlertMessage"] += "IncomeYrTotal  = "+ IncomeYrTotal  + "<hr>";
            TempData["AlertMessage"] += "DebtMonthTotal = " + DebtMonthTotal + "<hr>";
            TempData["AlertMessage"] += "DebtToIncome   = " + DebtToIncome   + "<hr>";
            return View();
            //*/

            TempData["AlertTag"] = "success";
            TempData["AlertLabel"] = "Success!: ";
            TempData["AlertMessage"] = "Loan request has been submited.";


            return RedirectToAction("Dashboard", "Account");
        }
    }
}

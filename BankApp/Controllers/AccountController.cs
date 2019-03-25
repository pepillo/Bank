using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Bank.Models;
using Bank.ViewModels;
using Newtonsoft.Json;

namespace Bank.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Index()
        {
            using (BankDB db = new BankDB())
            {
                //return View(db.User.ToList());
            }

            return RedirectToRoute(new
            {
                controller = "Account",
                action = "Login"
            });
        }

        //JDR: This will be called when a action requiers loggin to be used
        public ActionResult SendToLogin()
        {
            TempData["AlertTag"]     = "warning";
            TempData["AlertLabel"]   = "Login:";
            TempData["AlertMessage"] = "Please Login to your account to access that page.";

            //JDR: Redirect to login if user not loggedIn
            return RedirectToRoute(new
            {
                controller = "Account",
                action = "Login"
            });
        }

        public ActionResult Dashboard()
        {
            if (!UserAccount.IsLoggedIn()) return SendToLogin();

            using (BankDB db = new BankDB())
            {
                User User = db.User.Find(UserAccount.ActiveUser.ID);

                var CreditScore = User.GetCreditScore();

                //JDR: Update CreditScore if over 3 days
                if (GLOBAL.DaysPassed(CreditScore.LastUpdatedDate) > 3) CreditScore.UpdateCreditScore(User.ID);

                return View(User);
            }
        }

        public ActionResult Register()
        {
            return View(new Account_Register_VM());
        }

        [HttpPost]
        public ActionResult Register(Account_Register_VM Model)
        {
            if (!ModelState.IsValid) return View(Model);

            if (UserAccount.EmailExist(Model.Email))
            {
                ModelState.AddModelError("", "A account with that email exist.");
                return View(Model);
            }

            //JDR: Register user, New user row in DB
            UserAccount.Register(Model);

            ModelState.Clear();

            TempData["AlertTag"]     = "info";
            TempData["AlertLabel"]   = "";
            TempData["AlertMessage"] = "Account has been created, Please Verify your account before login.";

            return RedirectToAction("Login", "Account");
        }

        //JDR: Resend newley generated ConfirmationCode to user "Email"
        public ActionResult ResentConfirmationCode(string Email)
        {
            using (BankDB db = new BankDB())
            {
                var User = db.User.Where(userRow => userRow.Email == Email).FirstOrDefault();

                if (User == null)
                {
                    TempData["AlertTag"]     = "danger";
                    TempData["AlertLabel"]   = "Error:";
                    TempData["AlertMessage"] = "Incorrect email for code validation.";

                    return RedirectToAction("Login", "Account");
                }

                var Confirmation = Models.EmailConfirmation.SetEmailConfirmation(User.ID);

                /**************************************** REMOVE ****************************************/
                //JDR: TODO: Remove this: This is here just to fake user email confirmation link
                Session["ConfirmEmail"] = User.Email;
                Session["ConfirmCode"]  = Confirmation.Token;
                /**************************************** REMOVE ****************************************/

                TempData["AlertTag"]     = "info";
                TempData["AlertLabel"]   = "";
                TempData["AlertMessage"] = "Email confirmation code was sent to <b>"+User.Email+"</b>";

                return RedirectToAction("Login", "Account");
            }
        }

        public ActionResult Login()
        {
            if (UserAccount.IsLoggedIn())
            {
                //JDR: Redirect useser to correct path depending where he tried to login
                ActionResult Action = RedirectToAction(UserAccount.LoggedInAction(), UserAccount.LoggedInController(), new { Example = "" });

                //JDR: Reset path that redirects user after login to default values
                UserAccount.ResetLoggedInPath();

                return Action;
            }

            /**************************************** REMOVE ****************************************/
            //JDR: TODO: Remove this: This is here just to fake user email confirmation link
            if (Session["ConfirmEmail"] != null)
            {
                string LinkConfirmation = Url.Action("EmailConfirmation", "Account", new { Email = Session["ConfirmEmail"].ToString().Trim(), Token = Session["ConfirmCode"].ToString().Trim() });
                LinkConfirmation = " <a class=\"pull-right\" target=\"_blank\" href=\"" + LinkConfirmation + "\">Activate Account</a>";

                Session["ConfirmEmail"] = null;
                Session["ConfirmCode"] = null;

                TempData["AlertMessage"] += LinkConfirmation;
            }
            /**************************************** REMOVE ****************************************/

            return View();
        }

        [HttpPost]
        public ActionResult Login(User Account)
        {
            if (UserAccount.Login(ref Account))
            {
                if (Account.UserStatus != 1)
                {
                    UserAccount.Clear();    //JDR: CLear Session for all

                    //JDR: Show Email confirmation Error
                    string LinkResendConfirmation = Url.Action("ResentConfirmationCode", "Account", new { Email = Account.Email });
                    LinkResendConfirmation = "<a href=\"" + LinkResendConfirmation + "\">Resend Confirmation Code</a>";

                    TempData["AlertTag"]     = "warning";
                    TempData["AlertLabel"]   = "Email Confirmation:";
                    TempData["AlertMessage"] = "Email confirmation is requierd, please verify you email. " + LinkResendConfirmation;

                    return View();
                }

                //JDR: Redirect useser to correct path depending where he tried to login
                ActionResult Action = RedirectToAction(UserAccount.LoggedInAction(), UserAccount.LoggedInController(), new { Example = "" });

                UserAccount.ResetLoggedInPath();

                return Action;
            }

            ModelState.AddModelError("", "Email/Password is Incorrect");

            return View();
        }

        //JDR: LoggedIn Action - Verify if user is logged in (session exist)
        public ActionResult LoggedIn()
        {
            if (!UserAccount.IsLoggedIn())
            {
                return RedirectToAction("Login");
            }

            return View();
        }

        //JDR: LoggedOut Action - Log Out user (session destroy)
        public ActionResult LoggedOut()
        {
            UserAccount.Clear();

            return RedirectToAction("LoggedIn");

        }

        public ActionResult EmailConfirmation(string Email, string Token)
        {
            using (BankDB db = new BankDB()) //JDR: Initiate context object
            {
                var User = db.User.Where(Row => Row.Email == Email).FirstOrDefault();

                if (User == null)
                {
                    TempData["AlertTag"]     = "danger";
                    TempData["AlertLabel"]   = "Error:";
                    TempData["AlertMessage"] = "Invalid Confirmation Code.";

                    return RedirectToAction("Login", "Account");
                }

                var Confirmation = db.EmailConfirmation.
                                   OrderByDescending(Row => Row.ID).
                                   Where(Row => Row.UserID == User.ID).
                                   FirstOrDefault();

                if (Confirmation == null || Confirmation.Token.Trim() != Token)
                {
                    TempData["AlertTag"]     = "danger";
                    TempData["AlertLabel"]   = "Error:";
                    TempData["AlertMessage"] = "Invalid Confirmation Code.";

                    return RedirectToAction("Login", "Account");
                }

                User.UserStatus = 1;
                User.Save();

                TempData["AlertTag"]     = "success";
                TempData["AlertLabel"]   = "Success!: ";
                TempData["AlertMessage"] = "Your Account Has Been Successfully Confirmed.";

                return RedirectToAction("Login", "Account");
            }
        }

        public ActionResult Edit()
        {
            if (!UserAccount.IsLoggedIn(this)) return SendToLogin();

            User User = new User().GetByID(UserAccount.ActiveUser.ID);
            Address Address = User.GetAddress();

            var Model = new Account_Edit_VM();

            Model.Telephone = User.Telephone;
            Model.Address   = Address.Address1;
            Model.City      = Address.City;
            Model.State     = Address.State;
            Model.Zip       = Address.ZipCode;

            return View(Model);
        }

        [HttpPost]
        public ActionResult Edit(Account_Edit_VM Model)
        {
            if (!UserAccount.IsLoggedIn(this)) return SendToLogin();

            if (!ModelState.IsValid) return View(Model);

            User User = new User().GetByID(UserAccount.ActiveUser.ID);
            User.Telephone = Model.Telephone;

            User.Save();

            Address Address = User.GetAddress();

            Address.Address1 = Model.Address;
            Address.City     = Model.City;
            Address.State    = Model.State;
            Address.ZipCode  = Model.Zip;

            Address.Save();

            ModelState.Clear();

            TempData["AlertTag"]     = "success";
            TempData["AlertLabel"]   = "Success!: ";
            TempData["AlertMessage"] = "Your account information was updated.";

            return RedirectToAction("Dashboard", "Account");
        }

        public ActionResult ForgotPassword()
        {
            if (UserAccount.IsLoggedIn()) return RedirectToAction("Dashboard", "Account");

            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(string Email)
        {
            if(!UserAccount.EmailExist(Email))
            {
                TempData["AlertTag"]     = "danger";
                TempData["AlertLabel"]   = "Error: ";
                TempData["AlertMessage"] = "Invalid Email Address";

                return View();
            }

            string Password = System.Web.Security.Membership.GeneratePassword(10, 1);

            using (BankDB db = new BankDB())
            {
                var User = db.User.Where(userRow => userRow.Email == Email).FirstOrDefault();

                User.Password = GLOBAL.Hash(Password, UserAccount.PasswordSalt);
                User.Email = User.Email.Trim();

                User.Save();
            }

            TempData["AlertTag"]     = "info";
            TempData["AlertLabel"]   = "";
            TempData["AlertMessage"] = "A new password has been sent to <b>" + Email + "</b> <div class=\"pull-right\">" + Password + "</div>";

            return View();
        }
    }
}

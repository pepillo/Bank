using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bank.Models
{
    public static class UserAccount
    {
        //-------------------- Define Model Static Elements --------------------
        public static dynamic ActiveUser = null;
        public static readonly string PasswordSalt = "KingdomHearts3WillBeSoAwsome!!!!Booyyaaa!!!";

        private static string LoggedInPathController = "Account";   //JDR: Var for controller path when user loggedIn
        private static string LoggedInPathAction = "Dashboard";     //JDR: Var for ACTION path when user loggedIn

        //-------------------- PRIVATE Define Model Methods --------------------
        //JDR: All overloads to setActiveUser call this function
        private static void Set(dynamic Object)
        {
            UserAccount.ActiveUser = Object;
        }

        //JDR: OVERLOAD 1: Private method to set ActiveUser using User model object or session
        private static bool SetActiveUser()
        {
            if (!UserAccount.IsLoggedIn()) return false;

            //JDR: Set static variable for active user
            var activeUser = new
            {
                ID = HttpContext.Current.Session["UserID"],
                Email = HttpContext.Current.Session["UserEmail"],
                Name = HttpContext.Current.Session["UserName"]
            };

            //JDR: Set static variable for active user
            UserAccount.Set(activeUser);

            return true;
        }

        //JDR: OVERLOAD 3: Private method to set static user if ID found
        private static bool SetActiveUser(int UserID) //JDR: Use UserID for populating ActiveUser
        {
            using (BankDB db = new BankDB())
            {
                var activeUser = db.User.Find(UserID);

                if (activeUser == null) return false;

                UserAccount.SetActiveUser(activeUser);

                return true;
            }
        }

        //JDR: OVERLOAD 4: Private method to set static user if ID found
        private static bool SetActiveUser(string UserID) //JDR: Use UserID for populating ActiveUser, convert str to int
        {
            return UserAccount.SetActiveUser(Int32.Parse(UserID));
        }

        //JDR: OVERLOAD 5: Private method to set ActiveUser using User model object or session
        private static bool SetActiveUser(User user) //JDR: Use user for populating ActiveUser
        {
            //JDR: Set static variable for active user
            var activeUser = new
            {
                ID = user.ID,
                Email = user.Email,
                Name = user.FullName()
            };

            //JDR: Set static variable for active user
            UserAccount.Set(activeUser);

            return true;
        }

        //JDR: Only method that can set session for active user
        private static void SetSession(User user)
        {
            //JDR: Set session for activeUser
            HttpContext.Current.Session["UserID"] = user.ID.ToString();
            HttpContext.Current.Session["UserEmail"] = user.Email.ToString();
            HttpContext.Current.Session["UserName"] = user.FullName();
        }

        //JDR: Set conteoller loggedIn
        private static void SetLoggedInPathController(string Controller = "Account")
        {
            UserAccount.LoggedInPathController = Controller;
        }

        //JDR: Set Action LoggedIn
        private static void SetLoggedInPathAction(string Action = "Dashboard")
        {
            UserAccount.LoggedInPathAction = Action;
        }

        //------------------------PUBLIC Define Model Methods ------------------------
        //JDR: Reset logginPath
        public static void ResetLoggedInPath()
        {
            UserAccount.SetLoggedInPathController();
            UserAccount.SetLoggedInPathAction();
        }

        //JDR: Return Controller path str
        public static string LoggedInController()
        {
            return UserAccount.LoggedInPathController;
        }

        //JDR:Return Action path str
        public static string LoggedInAction()
        {
            return UserAccount.LoggedInPathAction;
        }

        //JDR: Set path for user to be redirected after loggin, this is also used to send user to a page they atempted to acces but was not loggedIn
        public static void SetLoggedInPathLocation(string Controller, string Action)
        {
            UserAccount.SetLoggedInPathController(Controller);
            UserAccount.SetLoggedInPathAction(Action);
        }

        //JDR: Set path for user to be redirected after loggin, this is also used to send user to a page they atempted to acces but was not loggedIn
        public static void SetLoggedInPathLocation(System.Web.Mvc.Controller CurrentController)
        {
            string Controller = CurrentController.ControllerContext.RouteData.Values["controller"].ToString();
            string Action = CurrentController.ControllerContext.RouteData.Values["action"].ToString();

            UserAccount.SetLoggedInPathLocation(Controller, Action);
        }

        //JDR: Get ActiveUser variable
        public static dynamic Get()
        {
            if (!UserAccount.IsLoggedIn()) return false;
            if (UserAccount.ActiveUser == null) return false;

            return UserAccount.ActiveUser;
        }

        //JDR: Get specific element from ActiveUser object
        public static string Get(string element)
        {
            if (!UserAccount.IsLoggedIn()) return "";

            if (UserAccount.ActiveUser == null) return "";

            //JDR:Probably should have a logic here to verify if element exist
            //.......

            //JDR: Return element from object as string
            return UserAccount.Get().GetType().GetProperty(element).GetValue(UserAccount.Get(), null).ToString();
        }

        public static bool EmailExist(string Email)
        {
             using (BankDB db = new BankDB())
             {
                var User = db.User.Where(userRow => userRow.Email == Email).FirstOrDefault();

                if (User == null) return false;

                return true;
             }
        }

        //JDR: Overload 1: Verify if active user in seccion
        public static bool IsLoggedIn()
        {
            var UserID = HttpContext.Current.Session["UserID"] ?? null;

            if (UserID != null)
            {
                var result = UserAccount.SetActiveUser(UserID.ToString());

                if (result == false) return false;
            }

            return UserID != null ? true : false;
        }

        //JDR: Overload 2: Verify if active user in seccion, also set loggedInPathLocation
        public static bool IsLoggedIn(System.Web.Mvc.Controller CurrentController)
        {
            if (UserAccount.IsLoggedIn()) return true;

            //JDR: user is not loggedIn so Set LoggedInLocation for when he logges in
            UserAccount.SetLoggedInPathLocation(CurrentController);

            return false;
        }

        public static bool Login(ref User user)
        {
            using (BankDB db = new BankDB())
            {
                string Email = user.Email;
                string HashPassword = GLOBAL.Hash(user.Password, UserAccount.PasswordSalt);

                var activeUser = db.User.Where(userRow => userRow.Email == Email && userRow.Password == HashPassword).FirstOrDefault();

                if (activeUser == null) return false;

                user = activeUser;

                UserAccount.SetSession(activeUser);     //JDR: Store user login credentials in global Session
                UserAccount.SetActiveUser(activeUser);  //JDR: Set static variable for active user for all to call

                return true;
            }
        }

        public static int Register(dynamic Account)
        {
            //JDR: Setup User
            User User = new User().New();

            User.FirstName = Account.FirstName.Trim();
            User.MiddleName = (Account.MiddleName ?? "").Trim();
            User.LastName = Account.LastName.Trim();
            User.Email = Account.Email.Trim();
            User.Telephone = Account.Telephone.Trim();
            User.Password = GLOBAL.Hash(Account.Password, UserAccount.PasswordSalt);
            User.UserStatus = 0;
            User.CreatedDate = DateTime.Now;

            //JDR: Setup Address
            Address Address = new Address().New();

            Address.Address1 = Account.Address;
            Address.City = Account.City;
            Address.State = Account.State;
            Address.ZipCode = Account.Zip;

            Address.Add();  //JDR: Add Address to DB

            User.AddressID = Address.ID;
            User.Add();     //JDR: Add User to DB

            //JDR: Get User CreditScore
            CreditScore.UpdateCreditScore(User.ID);

            //JDR: Create User confirmation code in DB
            var Confirmation = EmailConfirmation.SetEmailConfirmation(User.ID);

            /*#################################### REMOE /*####################################*/
            //JDR: TODO: REMOVE: This is here just to fake user email confirmation link
            HttpContext.Current.Session["ConfirmEmail"] = User.Email;
            HttpContext.Current.Session["ConfirmCode"] = Confirmation.Token;
            /*#################################### REMOE /*####################################*/

            return User.ID;
        }

        //JDR: Clear session and ActiveUser Static
        public static void Clear()
        {
            HttpContext.Current.Session.Clear();    //JDR: Clear sessions
            UserAccount.ActiveUser = null;          //JDR: Clear Static Active User
        }
    }
}

using Bank.Models;
using System;
using System.Web.Mvc;

namespace Bank.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.FormSubmited = 0;

            return View();
        }

        //JDR: For ContactUsForm
        [HttpPost]
        public ActionResult Contact(String FormName, String FormEmail, String FormPhone, String FormMessage)
        {
            //JDR: Logic to store this in DB or sent to admin
            //...LOGIC...FORM DATA VALIDATION

            ViewBag.FormSubmited = 1;
            ViewBag.FormNotification = FormName + ", we will contact you as soon as possible.";

            return View();
        }
    }
}

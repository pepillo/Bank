using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DropDownList
{
    public static class DropDownList
    {
        public static IEnumerable<SelectListItem> EmploymentTypes()
        {
            var EmpType = GLOBAL.JsonGetLocal("~/App_data/Json/EmploymentTypes.json");
            var EmpList = GLOBAL.ArrayToSelectList(EmpType, "description", "description");

            EmpList.Insert(0, new SelectListItem { Text = "Select ...", Value = "" });

            return EmpList;
        }

        public static IEnumerable<SelectListItem> LoanTypes()
        {
            var LoanTypes = GLOBAL.JsonGetLocal("~/App_data/Json/LoanTypes.json");
            var LoanList = GLOBAL.ArrayToSelectList(LoanTypes, "type", "description");

            LoanList.Insert(0, new SelectListItem { Text = "Select ...", Value = "" });

            return LoanList;
        }
    }
}
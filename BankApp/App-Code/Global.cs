/*JDR: Global Class used to access common method/classes/variables within the project*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public static class GLOBAL
{
    public static string Example = "";

    public static dynamic JsonGetLocal(string location)
    {
        string file = HttpContext.Current.Server.MapPath(location);

        using (StreamReader reader = new StreamReader(file))
        {
            string json = reader.ReadToEnd();
            return JsonConvert.DeserializeObject(json);
        }
    }

    public static IEnumerable<SelectListItem> ArrayToSelectList(dynamic Array, string Key, string Value, string Default = "Select ...")
    {
        IList<SelectListItem> items = new List<SelectListItem> { };

        //items.Add(new SelectListItem { Text = "Select ...", Value = "" });

        foreach (var Element in Array)
        {
            items.Add(new SelectListItem { Text = (string)Element[Value], Value = (string)Element[Key] });
        }

        return items;
    }

    public static int DaysPassed(DateTime? Date)
    {
        DateTime DateToday = DateTime.Now.Date;

        TimeSpan TimeSpan = DateToday - (DateTime)Date;

        return TimeSpan.Days;
    }

    public static string Hash(string Str, string Salt="")
    {
        return System.Web.Helpers.Crypto.Hash(Str + Salt, "MD5");
    }
}

public static class DEBUG
{
    public static string ObjectToString(dynamic Object)
    {
        string str = "<br>";
        str += "<b>Object</b><br>";
        str += JsonConvert.SerializeObject(Object, Formatting.Indented,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

        return "<pre>" + str + "</pre>";
    }

    public static string DatabaseExceptionString(dynamic dbException, dynamic Object = null) {
        string str = "<br>";
        str += "<b>Object</b><br>";
        str += JsonConvert.SerializeObject(Object, Formatting.Indented,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
        str += "<hr>";
        str += "<b>Exception</b><br>";
        foreach (var validationErrors in dbException.EntityValidationErrors)
        {
            foreach (var validationError in validationErrors.ValidationErrors)
            {
                //System.Console.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                str += "Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage + "<br />";
            }
        }

        return "<pre>"+str+"</pre>";
    }
}

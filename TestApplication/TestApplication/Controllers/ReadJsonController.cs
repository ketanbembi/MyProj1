using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TestApplication.Models;

namespace TestApplication.Controllers
{
    public class ReadJsonController : Controller
    {
        // GET: ReadJson
        public ActionResult Index()
        {
            string file = Server.MapPath("~/App_Data/data_small.json");
            string Json = System.IO.File.ReadAllText(file);
            JavaScriptSerializer ser = new JavaScriptSerializer();
            var personlist = ser.Deserialize<Data>(Json);
            return View(personlist.People);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCProject.Context;
using MVCProject.Models;
namespace MVCProject.Controllers
{
    public class HomeController : Controller
    {
        MVCPetsContext db = new MVCPetsContext();

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
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Products()
        {
            ViewBag.Message = "Your Products page.";
            ViewBag.Products = db.Products.ToList();
            ViewData["Products"] = db.Products.ToList();
            return View(db.Categories.ToList());
        }
        public ActionResult Product()
        {
            ViewBag.Message = "Your Product page.";
            ViewBag.Products = db.Products.ToList();
            ViewData["Products"] = db.Products.ToList();
            return View(db.Categories.ToList());
        }
    }
}
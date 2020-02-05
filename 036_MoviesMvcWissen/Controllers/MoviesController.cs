using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _036_MoviesMvcWissen.Contexts;
using _036_MoviesMvcWissen.Entities;

namespace _036_MoviesMvcWissen.Controllers
{
    public class MoviesController : Controller
    {
        MoviesContext db = new MoviesContext();

        public ViewResult Index()
        {
            var model = db.Movies.ToList();
            return View(model);
        }

        [HttpGet]
        public ViewResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(string inputName, string inputProductionYear, string inputBoxOfficeReturn)
        {
            var entity = new Movie()
            {
                Name = inputName,
                ProductionYear = inputProductionYear,
                BoxOfficeReturn = Convert.ToDouble(inputBoxOfficeReturn.Replace(",", "."), CultureInfo.InvariantCulture)
            };

            db.Movies.Add(entity);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
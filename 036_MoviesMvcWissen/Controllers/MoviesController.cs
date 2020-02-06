using _036_MoviesMvcWissen.Contexts;
using _036_MoviesMvcWissen.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.UI.WebControls;

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
            ViewBag.Message = "You have to fill required information areas.";
            return new ViewResult();
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
            TempData["Info"] = "Your entry saved to the database.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Id is required!");
            }
            var model = db.Movies.Find(id.Value);

            List<SelectListItem> years = new List<SelectListItem>();
            SelectListItem year;

            for (int i = DateTime.Now.Year; i > 1950; i--)
            {
                year = new SelectListItem()
                {
                    Value = i.ToString(),
                    Text = i.ToString()
                };
                years.Add(year);
            }
            ViewBag.years = new SelectList(years, "Value", "Text", model.ProductionYear);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id, Name, ProductionYear")]Movie movie, string BoxOfficeReturn)
        {
            var entity = db.Movies.SingleOrDefault(e => e.Id == movie.Id);
            entity.Name = movie.Name;
            entity.ProductionYear = movie.ProductionYear;
            if (BoxOfficeReturn == "")
            {
                BoxOfficeReturn = "0";
            }
            entity.BoxOfficeReturn = Convert.ToDouble(BoxOfficeReturn.Replace(",", "."), CultureInfo.InvariantCulture);


            db.Entry(entity).State = EntityState.Modified;
            db.SaveChanges();


            TempData["Info"] = "Changes saved successfully!";
            return RedirectToRoute(new { controller = "Movies", action = "Index" });
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Id is required!");
            }

            var model = db.Movies.Find(id);

            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var entity = db.Movies.Find(id);
            TempData["Info"] = entity.Name + " deleted as your request!";


            db.Movies.Remove(entity);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
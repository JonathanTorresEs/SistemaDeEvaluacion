using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProyectoIntegrador.Models;

namespace ProyectoIntegrador.Controllers
{
    public class PlanEstudiosController : Controller
    {
        private EgelTrainingEntities db = new EgelTrainingEntities();

        // GET: PlanEstudios
        public ActionResult Index()
        {
            var planEstudios = db.PlanEstudios.Include(p => p.Carrera);
            return View(planEstudios.ToList());
        }

        // GET: PlanEstudios/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlanEstudios planEstudios = db.PlanEstudios.Find(id);
            if (planEstudios == null)
            {
                return HttpNotFound();
            }
            return View(planEstudios);
        }

        // GET: PlanEstudios/Create
        public ActionResult Create()
        {
            ViewBag.Siglas = new SelectList(db.Carrera, "Siglas", "NombreLargo");
            return View();
        }

        // POST: PlanEstudios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Siglas,Plan")] PlanEstudios planEstudios)
        {
            if (ModelState.IsValid)
            {
                db.PlanEstudios.Add(planEstudios);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Siglas = new SelectList(db.Carrera, "Siglas", "NombreLargo", planEstudios.Siglas);
            return View(planEstudios);
        }

        // GET: PlanEstudios/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlanEstudios planEstudios = db.PlanEstudios.Find(id);
            if (planEstudios == null)
            {
                return HttpNotFound();
            }
            ViewBag.Siglas = new SelectList(db.Carrera, "Siglas", "NombreLargo", planEstudios.Siglas);
            return View(planEstudios);
        }

        // POST: PlanEstudios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Siglas,Plan")] PlanEstudios planEstudios)
        {
            if (ModelState.IsValid)
            {
                db.Entry(planEstudios).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Siglas = new SelectList(db.Carrera, "Siglas", "NombreLargo", planEstudios.Siglas);
            return View(planEstudios);
        }

        // GET: PlanEstudios/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlanEstudios planEstudios = db.PlanEstudios.Find(id);
            if (planEstudios == null)
            {
                return HttpNotFound();
            }
            return View(planEstudios);
        }

        // POST: PlanEstudios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            PlanEstudios planEstudios = db.PlanEstudios.Find(id);
            db.PlanEstudios.Remove(planEstudios);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

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
    public class RespuestasErroneasController : Controller
    {
        private EgelTrainingEntities db = new EgelTrainingEntities();

        // GET: RespuestasErroneas
        public ActionResult Index()
        {
            var respuestasErroneas = db.RespuestasErroneas.Include(r => r.Pregunta);
            return View(respuestasErroneas.ToList());
        }

        // GET: RespuestasErroneas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RespuestasErroneas respuestasErroneas = db.RespuestasErroneas.Find(id);
            if (respuestasErroneas == null)
            {
                return HttpNotFound();
            }
            return View(respuestasErroneas);
        }

        // GET: RespuestasErroneas/Create
        public ActionResult Create()
        {
            ViewBag.ID_Pregunta = new SelectList(db.Pregunta, "IDPregunta", "Texto");
            return View();
        }

        // POST: RespuestasErroneas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Consecutivo,ID_Pregunta,Opcion,TextoRespuesta,Explicacion")] RespuestasErroneas respuestasErroneas)
        {
            if (ModelState.IsValid)
            {
                db.RespuestasErroneas.Add(respuestasErroneas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ID_Pregunta = new SelectList(db.Pregunta, "IDPregunta", "Texto", respuestasErroneas.ID_Pregunta);
            return View(respuestasErroneas);
        }

        // GET: RespuestasErroneas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RespuestasErroneas respuestasErroneas = db.RespuestasErroneas.Find(id);
            if (respuestasErroneas == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID_Pregunta = new SelectList(db.Pregunta, "IDPregunta", "Texto", respuestasErroneas.ID_Pregunta);
            return View(respuestasErroneas);
        }

        // POST: RespuestasErroneas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Consecutivo,ID_Pregunta,Opcion,TextoRespuesta,Explicacion")] RespuestasErroneas respuestasErroneas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(respuestasErroneas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID_Pregunta = new SelectList(db.Pregunta, "IDPregunta", "Texto", respuestasErroneas.ID_Pregunta);
            return View(respuestasErroneas);
        }

        // GET: RespuestasErroneas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RespuestasErroneas respuestasErroneas = db.RespuestasErroneas.Find(id);
            if (respuestasErroneas == null)
            {
                return HttpNotFound();
            }
            return View(respuestasErroneas);
        }

        // POST: RespuestasErroneas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RespuestasErroneas respuestasErroneas = db.RespuestasErroneas.Find(id);
            db.RespuestasErroneas.Remove(respuestasErroneas);
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

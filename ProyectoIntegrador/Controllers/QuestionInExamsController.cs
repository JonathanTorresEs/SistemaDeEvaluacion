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
    public class QuestionInExamsController : Controller
    {
        private EgelTrainingEntities db = new EgelTrainingEntities();

        // GET: QuestionInExams
        public ActionResult Index()
        {
            var questionInExam = db.QuestionInExam.Include(q => q.Alumno).Include(q => q.Examen).Include(q => q.Pregunta);
            return View(questionInExam.ToList());
        }

        // GET: QuestionInExams/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuestionInExam questionInExam = db.QuestionInExam.Find(id);
            if (questionInExam == null)
            {
                return HttpNotFound();
            }
            return View(questionInExam);
        }

        // GET: QuestionInExams/Create
        public ActionResult Create()
        {
            ViewBag.Matricula = new SelectList(db.Alumno, "Matricula", "Nombre");
            ViewBag.IDExamen = new SelectList(db.Examen, "IDExamen", "Nombre");
            ViewBag.IDPregunta = new SelectList(db.Pregunta, "IDPregunta", "Texto");
            return View();
        }

        // POST: QuestionInExams/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDExamen,IDPregunta,Respuesta,Matricula")] QuestionInExam questionInExam)
        {
            if (ModelState.IsValid)
            {
                db.QuestionInExam.Add(questionInExam);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Matricula = new SelectList(db.Alumno, "Matricula", "Nombre", questionInExam.Matricula);
            ViewBag.IDExamen = new SelectList(db.Examen, "IDExamen", "Nombre", questionInExam.IDExamen);
            ViewBag.IDPregunta = new SelectList(db.Pregunta, "IDPregunta", "Texto", questionInExam.IDPregunta);
            return View(questionInExam);
        }

        // GET: QuestionInExams/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuestionInExam questionInExam = db.QuestionInExam.Find(id);
            if (questionInExam == null)
            {
                return HttpNotFound();
            }
            ViewBag.Matricula = new SelectList(db.Alumno, "Matricula", "Nombre", questionInExam.Matricula);
            ViewBag.IDExamen = new SelectList(db.Examen, "IDExamen", "Nombre", questionInExam.IDExamen);
            ViewBag.IDPregunta = new SelectList(db.Pregunta, "IDPregunta", "Texto", questionInExam.IDPregunta);
            return View(questionInExam);
        }

        // POST: QuestionInExams/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDExamen,IDPregunta,Respuesta,Matricula")] QuestionInExam questionInExam)
        {
            if (ModelState.IsValid)
            {
                db.Entry(questionInExam).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Matricula = new SelectList(db.Alumno, "Matricula", "Nombre", questionInExam.Matricula);
            ViewBag.IDExamen = new SelectList(db.Examen, "IDExamen", "Nombre", questionInExam.IDExamen);
            ViewBag.IDPregunta = new SelectList(db.Pregunta, "IDPregunta", "Texto", questionInExam.IDPregunta);
            return View(questionInExam);
        }

        // GET: QuestionInExams/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuestionInExam questionInExam = db.QuestionInExam.Find(id);
            if (questionInExam == null)
            {
                return HttpNotFound();
            }
            return View(questionInExam);
        }

        // POST: QuestionInExams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            QuestionInExam questionInExam = db.QuestionInExam.Find(id);
            db.QuestionInExam.Remove(questionInExam);
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

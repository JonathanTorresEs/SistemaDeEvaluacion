using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProyectoIntegrador.Models;
using System.Security.Cryptography;
using System.Text;
using ProyectoIntegrador.ViewModels;

namespace ProyectoIntegrador.Controllers
{
    public class AlumnoesController : Controller
    {
        private EgelTrainingEntities db = new EgelTrainingEntities();

        [HttpPost]
        public ActionResult ValidateUser(String username, String password)
        {
            var alumno = db.Alumno.Where(a => a.Matricula == username).FirstOrDefault();
            if (alumno != null)
            {
                var sha1 = new SHA1CryptoServiceProvider();
                byte[] sha1data = sha1.ComputeHash(new System.IO.MemoryStream(Encoding.UTF8.GetBytes(password)));
                Array.Resize(ref sha1data, 64);
                if (alumno.PasswordHash.SequenceEqual(sha1data))
                {
                    //System.Web.HttpContext.Current.Session["role"] = "alumno";
                    HttpCookie c = new HttpCookie("matricula");
                    c.Value = username;
                    c.Expires = DateTime.Now.AddHours(1);
                    HttpCookie c2 = new HttpCookie("u");
                    c2.Value = "false";
                    c2.Expires = DateTime.Now.AddHours(1);
                    Response.Cookies.Add(c);
                    Response.Cookies.Add(c2);
                    return RedirectToAction("examenes_disponibles_alumno", "Examen");
                }
                else
                    return View("login_students");
            }
            return View("login_students");
        }

        // GET: Alumnoes
        public ActionResult Index()
        {
            var alumno = db.Alumno.Include(a => a.Carrera1);
            return View(alumno.ToList());
        }

        public ActionResult view_for_students(String Matricula)
        {
            var alumno = db.Alumno.Include(a => a.Carrera1).Where(a=>a.Matricula==Matricula).FirstOrDefault();
            return View(alumno);
        }

        public ActionResult results()
        {
            var alumno = db.Alumno.Include(a => a.Carrera1);
            return View(alumno.ToList());
        }

        public ActionResult users_list()
        {
            var alumno = db.Alumno.Include(a => a.Carrera1);
            return View(alumno.ToList());
        }

        public ActionResult login_students()
        {
            var alumno = db.Alumno.Include(a => a.Carrera1);
            return View(alumno.ToList());
        }

        public ActionResult Asignar(string id)
        {
            AlumnoExamenes vm = new AlumnoExamenes();
            vm.alumno = db.Alumno.Where(a => a.Matricula == id).FirstOrDefault();
            vm.examenes = db.Examen.ToList();
            return View(vm);
        }

        [HttpPost]
        public ActionResult Asignar(List<String> examenes, String Matricula)
        {
            foreach(var e in examenes)
            {
                var examen = db.Examen.Where(a => a.Nombre == e).FirstOrDefault();
                examen.Alumno.Add(db.Alumno.Where(a => a.Matricula == Matricula).FirstOrDefault());
                db.Entry(examen).State = EntityState.Modified;
                db.SaveChanges();
            }

            var alumno = db.Alumno.Include(a => a.Carrera1);
            return View("Index",alumno.ToList());
        }

        // GET: Alumnoes/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Alumno alumno = db.Alumno.Find(id);
            if (alumno == null)
            {
                return HttpNotFound();
            }
            return View(alumno);
        }

        // GET: Alumnoes/Create
        public ActionResult Create()
        {
            ViewBag.Carrera = new SelectList(db.Carrera, "Siglas", "NombreLargo");
            return View();
        }

        // POST: Alumnoes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Matricula,Nombre,ApellidoPaterno,ApellidoMaterno,Carrera,CorreoElectronico,PasswordHash")] Alumno alumno, String password)
        {
            if (ModelState.IsValid)
            {
                var sha1 = new SHA1CryptoServiceProvider();
                var sha1data = sha1.ComputeHash(new System.IO.MemoryStream(Encoding.UTF8.GetBytes(password)));
                alumno.PasswordHash = sha1data;
                db.Alumno.Add(alumno);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Carrera = new SelectList(db.Carrera, "Siglas", "NombreLargo", alumno.Carrera);
            return View(alumno);
        }

        // GET: Alumnoes/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Alumno alumno = db.Alumno.Find(id);
            if (alumno == null)
            {
                return HttpNotFound();
            }
            ViewBag.Carrera = new SelectList(db.Carrera, "Siglas", "NombreLargo", alumno.Carrera);
            return View(alumno);
        }

        // POST: Alumnoes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Matricula,Nombre,ApellidoPaterno,ApellidoMaterno,Carrera,CorreoElectronico,PasswordHash")] Alumno alumno)
        {
            if (ModelState.IsValid)
            {
                db.Entry(alumno).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Carrera = new SelectList(db.Carrera, "Siglas", "NombreLargo", alumno.Carrera);
            return View(alumno);
        }

        // GET: Alumnoes/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Alumno alumno = db.Alumno.Find(id);
            if (alumno == null)
            {
                return HttpNotFound();
            }
            return View(alumno);
        }

        // POST: Alumnoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Alumno alumno = db.Alumno.Find(id);
            db.Alumno.Remove(alumno);
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

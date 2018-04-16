using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ProyectoIntegrador.Models;

namespace ProyectoIntegrador.Controllers
{
    public class TemasController : Controller
    {
        private EgelTrainingEntities db = new EgelTrainingEntities();

        // GET: Temas
        public ActionResult Index()
        {
            if (!IsAuthorized())
            {
                return RedirectToAction("Index", "Home");
            }
            return View(db.Tema.ToList());
        }

        public ActionResult view_temas()
        {
            return View(db.Tema.ToList());
        }

        // GET: Temas
        public List<Tema> list_temas()
        {
            return db.Tema.ToList();
        }

        // GET: Temas/Details/5
        public ActionResult Details(string id)
        {
            if (!IsAuthorized())
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tema tema = db.Tema.Find(id);
            if (tema == null)
            {
                return HttpNotFound();
            }
            return View(tema);
        }

        // GET: Temas/Create
        public ActionResult Create()
        {
            if (!IsAuthorized())
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: Temas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ClaveTema,NombreTema")] Tema tema)
        {
            if (ModelState.IsValid)
            {
                string parsedClave = RemoveSpecialCharacters(tema.ClaveTema);
                tema.ClaveTema = parsedClave;

                db.Tema.Add(tema);

                try
                {
                    db.SaveChanges();
                } catch (DbUpdateException ex)
                {
                    return View(tema);
                }

                
                return RedirectToAction("Index");
            }

            return View(tema);
        }

        // GET: Temas/Edit/5
        public ActionResult Edit(string id)
        {
            if (!IsAuthorized())
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tema tema = db.Tema.Find(id);
            if (tema == null)
            {
                return HttpNotFound();
            }
            return View(tema);
        }

        // POST: Temas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ClaveTema,NombreTema")] Tema tema)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tema).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tema);
        }

        // GET: Temas/Delete/5
        public ActionResult Delete(string id)
        {
            if (!IsAuthorized())
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tema tema = db.Tema.Find(id);
            if (tema == null)
            {
                return HttpNotFound();
            }
            return View(tema);
        }

        // POST: Temas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Tema tema = db.Tema.Include(m => m.Examen).Include(m => m.Pregunta).Where(m => m.ClaveTema == id).FirstOrDefault();
            db.Tema.Remove(tema);
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

        public bool IsAuthorized()
        {
            HttpCookie c = HttpContext.Request.Cookies.Get("u");
            if (c != null)
            {
                if (c.Value.Equals("false"))
                    return false;
                else if (c.Value.Equals("true"))
                    return true;
            }

            return false;
        }

        public string RemoveSpecialCharacters(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z'))
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }


    }
}

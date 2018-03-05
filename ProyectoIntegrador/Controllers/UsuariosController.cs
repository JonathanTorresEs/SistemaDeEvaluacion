using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProyectoIntegrador.Models;
using System.Text;
using System.Security.Cryptography;
using ProyectoIntegrador.Controllers;

namespace ProyectoIntegrador.Controllers
{
    public class UsuariosController : Controller
    {
        private EgelTrainingEntities db = new EgelTrainingEntities();

        [HttpPost]
        public ActionResult ValidateUser(String username, String password)
        {
            Usuario user = db.Usuario.Where(a => a.Nombre == username).FirstOrDefault();
            if (user != null)
            {
                var sha1 = new SHA1CryptoServiceProvider();
                byte[] sha1data = sha1.ComputeHash(new System.IO.MemoryStream(Encoding.UTF8.GetBytes(password)));
                Array.Resize(ref sha1data, 64);
                if (user.PasswordHash.SequenceEqual(sha1data))
                {
                    //System.Web.HttpContext.Current.Session["role"] = "usuario";
                    HttpCookie c2 = new HttpCookie("u");
                    c2.Value = "true";
                    c2.Expires = DateTime.Now.AddHours(1);
                    Response.Cookies.Add(c2);
                    return RedirectToAction("Index");
                }
                else
                    return View("login_users");
            }
            return View("login_users");
        }

        // GET: Usuarios
        public ActionResult Index()
        {
            if (!IsAuthorized()) {
                return RedirectToAction("Index", "Home");
            }
            return View(db.Usuario.ToList());
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

        public ActionResult login_users()
        {
            return View(db.Usuario.ToList());
        }

        // GET: Usuarios/Details/5
        public ActionResult Details(int? id)
        {
            if (!IsAuthorized())
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuario.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // GET: Usuarios/Create
        public ActionResult Create()
        {
                if (!IsAuthorized())
                 {
                     return RedirectToAction("Index", "Home");
                 }
               
            return View();
        }

        // POST: Usuarios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Nombre,ApellidoPaterno,ApellidoMaterno,CorreoElectronico")] Usuario usuario, String password)
        {
             if (!IsAuthorized())
             {
                 return RedirectToAction("Index", "Home");
             }
            if (ModelState.IsValid)
            {
                var sha1 = new SHA1CryptoServiceProvider();
                var sha1data = sha1.ComputeHash(new System.IO.MemoryStream(Encoding.UTF8.GetBytes(password)));
                usuario.PasswordHash = sha1data;
                usuario.Id = Convert.ToInt32(db.Usuario.Max(u => (int?)u.Id))+1;
                db.Usuario.Add(usuario);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!IsAuthorized())
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuario.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,ApellidoPaterno,ApellidoMaterno,CorreoElectronico,PasswordHash")] Usuario usuario)
        {
            if (!IsAuthorized())
            {
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                db.Entry(usuario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!IsAuthorized())
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuario.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Usuario usuario = db.Usuario.Find(id);
            db.Usuario.Remove(usuario);
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

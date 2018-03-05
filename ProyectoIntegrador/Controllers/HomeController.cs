using ProyectoIntegrador.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoIntegrador.Controllers
{
    public class HomeController : Controller
    {
        private EgelTrainingEntities db = new EgelTrainingEntities();

        public ActionResult Index()
        {
            HttpCookie cmat = HttpContext.Request.Cookies.Get("matricula");
            HttpCookie c = HttpContext.Request.Cookies.Get("u");

            if (c != null && c.Value != "")
            {
                if (c.Value == "false")
                {
                    if (cmat != null && cmat.Value != "")
                    {
                        if (db.Alumno.Where(a => a.Matricula == cmat.Value).FirstOrDefault() != null)
                            return RedirectToAction("examenes_disponibles_alumno", "Examen");
                    }
                }
                else if (c.Value == "true")
                {
                    return RedirectToAction("Index", "Usuarios");
                }
            }
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

        public ActionResult logout()
        {
            HttpContext.Response.Cookies.Remove("matricula");
            HttpCookie c1 = new HttpCookie("matricula");
            c1.Value = "";
            c1.Expires = DateTime.Now.AddHours(1);
            Response.Cookies.Add(c1);
            HttpContext.Response.Cookies.Remove("u");
            HttpCookie c2 = new HttpCookie("u");
            c2.Value = "false";
            c2.Expires = DateTime.Now.AddHours(1);
            Response.Cookies.Add(c2);

            return View("Index");
        }
    }
}
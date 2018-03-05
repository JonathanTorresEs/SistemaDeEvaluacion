using ProyectoIntegrador.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoIntegrador.Controllers
{
    public class Autentificacion : Controller
    {
        private EgelTrainingEntities db = new EgelTrainingEntities();

        // GET: Autentificacion
        public ActionResult Index()
        {
            return View();
        }
    }
}
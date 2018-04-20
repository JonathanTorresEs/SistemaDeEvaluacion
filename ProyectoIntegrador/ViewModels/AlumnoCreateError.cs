using ProyectoIntegrador.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoIntegrador.ViewModels
{
    public class AlumnoCreateError
    {

        public Alumno alumno { get; set; }
        public string error { get; set; }
        public IEnumerable<SelectListItem> carrera { get; set; }
    }
}
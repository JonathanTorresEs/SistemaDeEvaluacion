using ProyectoIntegrador.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoIntegrador.ViewModels
{
    public class ExamenCreateError
    {
        public Examen examen { get; set; }
        public String error { get; set; }
    }
}
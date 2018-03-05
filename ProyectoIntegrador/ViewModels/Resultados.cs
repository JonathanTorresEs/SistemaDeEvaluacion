using ProyectoIntegrador.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoIntegrador.ViewModels
{
    public class Resultados
    {
        public Alumno alumno { get; set; }
        public Examen examen { get; set; }
        public double calificacion { get; set; }
    }
}
using ProyectoIntegrador.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoIntegrador.ViewModels
{
    public class AlumnoExamenes
    {
        public Alumno alumno { get; set; }
        public List<Examen> examenes { get; set; }
    }
}
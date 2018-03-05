using ProyectoIntegrador.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoIntegrador.ViewModels
{
    public class RetroExamenVM
    {
        public Examen examen { get; set; }
        public List<Tema> temas { get; set; }
        public List<RetroPreguntaVM> preguntas { get; set; }
        public double calificacion { get; set; }
    }
}
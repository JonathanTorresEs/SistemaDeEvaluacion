using ProyectoIntegrador.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoIntegrador.ViewModels
{
    public class RetroPreguntaVM
    {
        public Pregunta pregunta { get; set; }
        //public QuestionInExam respuesta { get; set; }
        public String respuestaInciso { get; set; }
        public String respuestaTexto { get; set; }
        public String explicacion { get; set; }
    }
}
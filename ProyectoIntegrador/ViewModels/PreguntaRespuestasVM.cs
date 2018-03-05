using ProyectoIntegrador.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoIntegrador.ViewModels
{
    public class PreguntaRespuestasVM
    {
        public Pregunta pregunta { get; set; }
        public List<RespuestasErroneas> respuestasErroneas { get; set; }
    }
}
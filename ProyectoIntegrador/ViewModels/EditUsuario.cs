using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoIntegrador.ViewModels
{
    public class EditUsuario
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string ApellidoPaterno { get; set; }
        [Required]
        public string ApellidoMaterno { get; set; }
        [Required]
        public string CorreoElectronico { get; set; }

    }
}
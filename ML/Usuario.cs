using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ML
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        [Required(ErrorMessage ="Favor de Ingresar un Nombre")]
        public string? Nombre { get; set; }
        [Required(ErrorMessage = "Favor de Ingresar un Apellido Paterno")]
        public string? ApellidoPaterno { get; set; }
        [Required(ErrorMessage = "Favor de Ingresar un Apellido Materno")]
        public string? ApellidoMaterno { get; set; }
        [Required(ErrorMessage = "Favor de Ingresar un Email")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Favor de Ingresar un User Name")]
        public string? UserName { get; set; }
        [Required(ErrorMessage = "Favor de Ingresar el Sexo")]
        public string? Sexo { get; set; }
        [Required(ErrorMessage = "Favor de Ingresar un Teléfono")]
        public string? Telefono { get; set; }
        [Required(ErrorMessage = "Favor de Ingresar un Celular")]
        public string? Celular { get; set; }
        [Required(ErrorMessage = "Favor de Ingresar una Fecha de Nacimiento")]
        public string? FechaNacimiento { get; set; }
        [Required(ErrorMessage = "Favor de Ingresar un Password")]
        public string? Password { get; set; }
        [Required(ErrorMessage = "Favor de Ingresar un CURP")]
        public string? CURP { get; set; }
        [Required(ErrorMessage = "Favor de Ingresar un Rol")]
        public ML.Rol Rol { get; set; }
        public string? Imagen { get; set; }
        public bool Status { get; set; }
        public ML.Direccion Direccion { get; set; }
        public List<object> Usuarios { get; set; }
        

    }
}

using System;
using System.Collections.Generic;

namespace DL
{
    public partial class Usuario
    {
        public Usuario()
        {
            Direccions = new HashSet<Direccion>();
        }

        public int IdUsuario { get; set; }
        public string UserName { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public string ApellidoPaterno { get; set; } = null!;
        public string? ApellidoMaterno { get; set; }
        public string Email { get; set; } = null!;
        public string Sexo { get; set; } = null!;
        public string Telefono { get; set; } = null!;
        public string? Celular { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string? Password { get; set; }
        public string? CURP { get; set; }
        public int? IdRol { get; set; }
        public string RolNombre { get; set; }
        public bool Status { get; set; }
        public string Calle { get; set; }
        public string NumeroExterior { get; set; }
        public string NumeroInterior { get; set; }
        public string CodigoPostal { get; set; }
        public int IdColonia { get; set; }
        public string? Imagen { get; set; }
        public int IdEstado { get; set; }
        public string EstadoNombre { get; set; }
        public int IdPais { get; set; }
        public string PaisNombre { get; set; }
        public string ColoniaNombre { get; set; }
        public string MunicipioNombre { get; set; }
        public int IdMunicipio { get; set; }

        public virtual Rol? IdRolNavigation { get; set; }
        public virtual ICollection<Direccion> Direccions { get; set; }
    }
}

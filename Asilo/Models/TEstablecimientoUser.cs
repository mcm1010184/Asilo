using System.ComponentModel.DataAnnotations;

namespace Asilo.Models
{
    public class TEstablecimientoUser
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;

        public string Nit { get; set; } = null!;

        public string RepresentantePrincipal { get; set; } = null!;

        public string Email { get; set; } = null!;

        public int? Telefono { get; set; }

        public int Celular { get; set; }

        public string? Direccion { get; set; }
        public double? Latitud { get; set; }
        public double? Longitud { get; set; }

        public string TipoEstablecimiento { get; set; } = null!;
        public int IdUser { get; set; }
        [Display(Name = "Nombre de Usuario")]
  
        public string Usuario1 { get; set; } = null!;
        [Display(Name = "Contraseña")]

        public string Password { get; set; } = null!;
        [ScaffoldColumn(false)]
        public string Role { get; set; } = null!;
        [ScaffoldColumn(false)]
        public byte Estado { get; set; }
        [ScaffoldColumn(false)]
        public DateTime FechaRegistro { get; set; }
        [ScaffoldColumn(false)]
        public DateTime? FechaModificacion { get; set; }


    }
}

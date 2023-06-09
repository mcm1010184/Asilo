using System.ComponentModel.DataAnnotations;

namespace Asilo.Models
{
    public class TRecolectorUser
    {
        public int Id { get; set; }

        public string? Nonbre { get; set; } = null!;

        public string Apellido { get; set; } = null!;

        public string? SegundoApellido { get; set; }
        [Display(Name = "Cedula de Identidad")]
        public string Ci { get; set; } = null!;

        public int Celular { get; set; }

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

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Asilo.Models;

public partial class Usuario
{
    public int Id { get; set; }
    [Display(Name = "Nombre de Usuario")]
    public string Usuario1 { get; set; } = null!;
    [Display(Name = "Contraseña")]
    public string Password { get; set; } = null!;

    public string Role { get; set; } = null!;

    public byte Estado { get; set; }

    public DateTime FechaRegistro { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public  Benefactor? Benefactor { get; set; }

    public  Establecimiento? IdNavigation { get; set; } = null!;

    public  Recolector? Recolector { get; set; }
}

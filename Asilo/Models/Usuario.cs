using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Asilo.Models;

public class UsuarioMongo
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public int? IdAux { get; set; }
    public string? Nombre { get; set; }
    public string? Apellido { get; set; }
    public string? SegundoApellido { get; set; }
    public string? Ci { get; set; }
    public string? CorreoElectrónico { get; set; }
    public int? Celular { get; set; }
    public string? Usuario { get; set; }
    public string? Contraseña { get; set; }
    public string? Rol { get; set; }
    public int Estado { get; set; }
    public DateTime FechaRegistro { get; set; }
    public DateTime? FechaActualizada { get; set; }
}
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

    public virtual Establecimiento? Establecimiento { get; set; }

    public virtual Recolector? Recolector { get; set; }



}

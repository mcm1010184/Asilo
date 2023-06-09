using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace Asilo.Models;

public class UsuarioRecolector
{
    [BsonId]
    public int? IdAux { get; set; }
    public string? nonbre { get; set; } = null!;

    public string apellido { get; set; } = null!;

    public string? segundoApellido { get; set; }

    public string? ci { get; set; } = null!;
    public int? celular { get; set; }

    public string? usuario { get; set; }
    public string? contraseña { get; set; }
    public string? rol { get; set; }
    public int estado { get; set; }
    public DateTime fechaRegistro { get; set; }
    public DateTime? fechaActualizada { get; set; }
}
public partial class Recolector
{
    public int Id { get; set; }

    public string? Nonbre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public string? SegundoApellido { get; set; }

    public string Ci { get; set; } = null!;

    public int Celular { get; set; }

    public virtual ICollection<Donacion>? Donacions { get; set; } = new List<Donacion>();


    public virtual Usuario? IdNavigation { get; set; } = null!;

    public virtual ICollection<RecojosRealizado>? RecojosRealizados { get; set; } = new List<RecojosRealizado>();
}

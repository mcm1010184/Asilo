using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace Asilo.Models;
public class UsuarioEstablecimiento
{
    [BsonId]

    public int? IdAux { get; set; }
    public string? nombre { get; set; }
    public string? nit { get; set; }
    public string? representantePrincipal { get; set; }
    [BsonElement("correoElectrónico")]
    public string? correoElectrónico { get; set; }
    public int? telefono { get; set; }
    public int? celular { get; set; }
    public string? direccion { get; set; }
    public double? latitud { get; set; }
    public double? longitud { get; set; }
    public string? tipoEstablecimiento { get; set; }
    public string? usuario { get; set; }
    public string? contraseña { get; set; }
    public string? rol { get; set; }
    public int estado { get; set; }
    public DateTime fechaRegistro { get; set; }
    public DateTime? fechaActualizada { get; set; }
}
public partial class Establecimiento
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

    public virtual ICollection<Campana>? Campanas { get; set; } = new List<Campana>();

    public virtual Usuario? IdNavigation { get; set; } = null!;

    public virtual ICollection<RecojosRealizado>? RecojosRealizados { get; set; } = new List<RecojosRealizado>();
}

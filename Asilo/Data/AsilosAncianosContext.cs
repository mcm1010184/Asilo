using System;
using System.Collections.Generic;
using Asilo.Models;
using Microsoft.EntityFrameworkCore;

namespace Asilo.Data;

public partial class AsilosAncianosContext : DbContext
{
    public AsilosAncianosContext()
    {
    }

    public AsilosAncianosContext(DbContextOptions<AsilosAncianosContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Benefactor> Benefactors { get; set; }

    public virtual DbSet<Campana> Campanas { get; set; }

    public virtual DbSet<Donacion> Donacions { get; set; }

    public virtual DbSet<Establecimiento> Establecimientos { get; set; }

    public virtual DbSet<Imagen> Imagens { get; set; }

    public virtual DbSet<RecojosRealizado> RecojosRealizados { get; set; }

    public virtual DbSet<Recolector> Recolectors { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-T59A902;Initial Catalog=AsilosAncianos;User ID=sa;Password=Solamente603;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Benefactor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Benefact__3213E83F7AF1E425");

            entity.ToTable("Benefactor");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Apellidos)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasDefaultValueSql("((0))")
                .HasColumnName("apellidos");
            entity.Property(e => e.Carnet)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("carnet");
            entity.Property(e => e.Celular).HasColumnName("celular");
            entity.Property(e => e.Dirreccion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("dirreccion");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Latitud).HasColumnName("latitud");
            entity.Property(e => e.Longitud).HasColumnName("longitud");
            entity.Property(e => e.Nombres)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("nombres");
            entity.Property(e => e.Telefono).HasColumnName("telefono");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.Benefactor)
                .HasForeignKey<Benefactor>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Benefactor_Usuario");
        });

        modelBuilder.Entity<Campana>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Campana__3213E83FF1A179E0");

            entity.ToTable("Campana");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AsiloId).HasColumnName("asiloId");
            entity.Property(e => e.Estado)
                .HasDefaultValueSql("((1))")
                .HasColumnName("estado");
            entity.Property(e => e.FechaCierre)
                .HasColumnType("date")
                .HasColumnName("fechaCierre");
            entity.Property(e => e.FechaInicio)
                .HasColumnType("date")
                .HasColumnName("fechaInicio");
            entity.Property(e => e.FechaModificacion)
                .HasColumnType("datetime")
                .HasColumnName("fechaModificacion");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Requerimiento)
                .HasColumnType("text")
                .HasColumnName("requerimiento");
            entity.Property(e => e.TipoCampaña)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tipoCampaña");

            entity.HasOne(d => d.Asilo).WithMany(p => p.Campanas)
                .HasForeignKey(d => d.AsiloId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Campana__asilo_i__267ABA7A");
        });

        modelBuilder.Entity<Donacion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Donacion__3213E83FE40E785E");

            entity.ToTable("Donacion");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BenefactorId).HasColumnName("benefactorId");
            entity.Property(e => e.CampanaId).HasColumnName("campanaId");
            entity.Property(e => e.Recibida)
                .HasDefaultValueSql("((0))")
                .HasColumnName("recibida");
            entity.Property(e => e.RecolectorId).HasColumnName("recolectorId");
            entity.Property(e => e.TipoBenefactor)
                .HasDefaultValueSql("((1))")
                .HasColumnName("tipoBenefactor");
            entity.Property(e => e.TipoDonacion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tipoDonacion");

            entity.HasOne(d => d.Benefactor).WithMany(p => p.Donacions)
                .HasForeignKey(d => d.BenefactorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Donacion__benefa__300424B4");

            entity.HasOne(d => d.Campana).WithMany(p => p.Donacions)
                .HasForeignKey(d => d.CampanaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Donacion__campan__2F10007B");

            entity.HasOne(d => d.Recolector).WithMany(p => p.Donacions)
                .HasForeignKey(d => d.RecolectorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Donacion_Recolector");
        });

        modelBuilder.Entity<Establecimiento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Asilo__3213E83FB5C8991C");

            entity.ToTable("Establecimiento");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Celular).HasColumnName("celular");
            entity.Property(e => e.Direccion)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("direccion");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Latitud).HasColumnName("latitud");
            entity.Property(e => e.Longitud).HasColumnName("longitud");
            entity.Property(e => e.Nit)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NIT");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.RepresentantePrincipal)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("representantePrincipal");
            entity.Property(e => e.Telefono).HasColumnName("telefono");
            entity.Property(e => e.TipoEstablecimiento)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tipoEstablecimiento");
        });

        modelBuilder.Entity<Imagen>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Imagen__3213E83FC752657D");

            entity.ToTable("Imagen");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CampanaId).HasColumnName("campanaId");
            entity.Property(e => e.Imagen1).HasColumnName("imagen");

            entity.HasOne(d => d.Campana).WithMany(p => p.Imagens)
                .HasForeignKey(d => d.CampanaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Imagen__campana___29572725");
        });

        modelBuilder.Entity<RecojosRealizado>(entity =>
        {
            entity.ToTable("recojos_realizados");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.EstablecimientoId).HasColumnName("establecimientoId");
            entity.Property(e => e.Fecha)
                .HasColumnType("date")
                .HasColumnName("fecha");
            entity.Property(e => e.RecolectorId).HasColumnName("recolectorId");

            entity.HasOne(d => d.Establecimiento).WithMany(p => p.RecojosRealizados)
                .HasForeignKey(d => d.EstablecimientoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_recojos_realizados_Establecimiento");

            entity.HasOne(d => d.Recolector).WithMany(p => p.RecojosRealizados)
                .HasForeignKey(d => d.RecolectorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_recojos_realizados_Recolector");
        });

        modelBuilder.Entity<Recolector>(entity =>
        {
            entity.ToTable("Recolector");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Apellido)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("apellido");
            entity.Property(e => e.Celular).HasColumnName("celular");
            entity.Property(e => e.Ci)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("ci");
            entity.Property(e => e.Nonbre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nonbre");
            entity.Property(e => e.SegundoApellido)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("segundoApellido");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.Recolector)
                .HasForeignKey<Recolector>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Recolector_Usuario");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("Usuario");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Estado)
                .HasDefaultValueSql("((1))")
                .HasColumnName("estado");
            entity.Property(e => e.FechaModificacion)
                .HasColumnType("datetime")
                .HasColumnName("fechaModificacion");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("role");
            entity.Property(e => e.Usuario1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("usuario");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.Usuario)
                .HasForeignKey<Usuario>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuario_Establecimiento");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

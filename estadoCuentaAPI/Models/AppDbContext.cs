using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace estadoCuentaAPI.Models
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cliente> Clientes { get; set; } = null!;
        public virtual DbSet<MovimientoTarjetum> MovimientoTarjeta { get; set; } = null!;
        public virtual DbSet<TarjetaCredito> TarjetaCreditos { get; set; } = null!;

        public DbSet<ClienteTarjetaView> ClienteTarjetaViews { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=DefaultConnection");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.ToTable("Cliente");

                entity.Property(e => e.ClienteId).HasColumnName("Cliente_ID");

                entity.Property(e => e.Apellido)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DUI)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MovimientoTarjetum>(entity =>
            {
                entity.HasKey(e => e.MovimientoTarjetaId)
                    .HasName("PK__Movimien__134EFDDBF51FF81E");

                entity.Property(e => e.MovimientoTarjetaId).HasColumnName("Movimiento_Tarjeta_ID");

                entity.Property(e => e.Descripcion).IsUnicode(false);

                entity.Property(e => e.FechaMovimiento)
                    .HasColumnType("datetime")
                    .HasColumnName("Fecha_Movimiento");

                entity.Property(e => e.Monto).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.TarjetaCreditoId).HasColumnName("TarjetaCredito_ID");

                entity.Property(e => e.TipoMovimiento)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("Tipo_Movimiento");

                entity.HasOne(d => d.TarjetaCredito)
                    .WithMany(p => p.MovimientoTarjeta)
                    .HasForeignKey(d => d.TarjetaCreditoId)
                    .HasConstraintName("FK__Movimient__Tarje__3C69FB99");
            });

            modelBuilder.Entity<TarjetaCredito>(entity =>
            {
                entity.ToTable("TarjetaCredito");

                entity.Property(e => e.TarjetaCreditoId).HasColumnName("TarjetaCredito_ID");

                entity.Property(e => e.ClienteId).HasColumnName("Cliente_ID");

                entity.Property(e => e.FechaCorte)
                    .HasColumnType("date")
                    .HasColumnName("Fecha_Corte");

                entity.Property(e => e.FechaPago)
                    .HasColumnType("date")
                    .HasColumnName("Fecha_Pago");

                entity.Property(e => e.LimiteCredito)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("Limite_Credito");

                entity.Property(e => e.NumeroTarjeta)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("Numero_Tarjeta");

                entity.Property(e => e.SaldoUtilizado)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("Saldo_Utilizado");

                entity.Property(e => e.TasaInteresConfigurable)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("TasaInteresConfigurable");

                entity.Property(e => e.PorcentajeSaldoMin)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("porcentaje_Saldo_min");

                entity.HasOne(d => d.Cliente)
                    .WithMany(p => p.TarjetaCreditos)
                    .HasForeignKey(d => d.ClienteId)
                    .HasConstraintName("FK__TarjetaCr__Clien__398D8EEE");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

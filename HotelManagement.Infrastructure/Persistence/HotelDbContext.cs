using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using HotelManagement.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace HotelManagement.Infrastructure.Persistence;

public partial class HotelDbContext : IdentityDbContext<IdentityUser>
{
    public HotelDbContext(DbContextOptions<HotelDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Emergencycontact> Emergencycontacts { get; set; }
    public virtual DbSet<Hotel> Hotels { get; set; }
    public virtual DbSet<Reservation> Reservations { get; set; }
    public virtual DbSet<Reservationguest> Reservationguests { get; set; }
    public virtual DbSet<Room> Rooms { get; set; }
    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); 

        modelBuilder.Entity<Emergencycontact>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("emergencycontacts_pkey");
            entity.HasOne(d => d.Reservation).WithMany(p => p.Emergencycontacts)
                .HasConstraintName("fk_emergencycontacts_reservations");
        });

        modelBuilder.Entity<Hotel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("hotels_pkey");
            entity.Property(e => e.Isactive).HasDefaultValue(true);
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("reservations_pkey");
            entity.Property(e => e.Createdat).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Hotel)
                .WithMany(p => p.Reservations)
                .HasForeignKey(d => d.Hotelid)  // 🔹 Asegura la clave foránea
                .OnDelete(DeleteBehavior.Restrict) // 🔹 Evita eliminación en cascada
                .HasConstraintName("fk_reservations_hotels");

            entity.HasOne(d => d.Room)
                .WithMany(p => p.Reservations)
                .HasForeignKey(d => d.Roomid)  // 🔹 Asegura la clave foránea
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_reservations_rooms");

            entity.HasOne(d => d.Traveler)
                .WithMany(p => p.Reservations)
                .HasForeignKey(d => d.Travelerid)  // 🔹 Asegura la clave foránea
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_reservations_users");
        });

        modelBuilder.Entity<Reservationguest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("reservationguests_pkey");
            entity.HasOne(d => d.Reservation).WithMany(p => p.Reservationguests)
                .HasConstraintName("fk_reservationguests_reservations");
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("rooms_pkey");
            entity.Property(e => e.Isactive).HasDefaultValue(true);
            entity.HasOne(d => d.Hotel).WithMany(p => p.Rooms)
                .HasConstraintName("fk_rooms_hotels");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
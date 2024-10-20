using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ServerBuilding.Models;

public partial class TamiDBContext : DbContext
{
    public TamiDBContext()
    {
    }

    public TamiDBContext(DbContextOptions<TamiDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AppUser> AppUsers { get; set; }

    public virtual DbSet<Cake> Cakes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server = (localdb)\\MSSQLLocalDB;Initial Catalog=TamiDB;User ID=TaskAdminLogin;Password=kukuPassword;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AppUsers__3214EC07B853B00E");

            entity.Property(e => e.ProfilePicture).HasDefaultValue("AnonimusPic.jpg");
            entity.Property(e => e.UserPhone).HasDefaultValue("--");
        });

        modelBuilder.Entity<Cake>(entity =>
        {
            entity.HasKey(e => e.CakeId).HasName("PK__Cakes__C56DBF15A2E5D727");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

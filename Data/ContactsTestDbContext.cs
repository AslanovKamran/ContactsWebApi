using System;
using System.Collections.Generic;
using AspWebApiGlebTest.Models;
using Microsoft.EntityFrameworkCore;

namespace AspWebApiGlebTest.Data;

public partial class ContactsTestDbContext : DbContext
{
    public ContactsTestDbContext()
    {
    }

    public ContactsTestDbContext(DbContextOptions<ContactsTestDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Contact> Contacts { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contact>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Surname).HasMaxLength(100);
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RefreshT__3214EC0752F77A93");

            entity.Property(e => e.Expires).HasColumnType("datetime");
            entity.Property(e => e.Token).HasMaxLength(255);

           
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_Users_RoleId");

            entity.HasIndex(e => e.Login, "UQ__Users__5E55825BBC38279F").IsUnique();

            entity.Property(e => e.Login).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(100);
            entity.Property(e => e.Salt)
                .HasMaxLength(255)
                .HasDefaultValueSql("(N'')");

           
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

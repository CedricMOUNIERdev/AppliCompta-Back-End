using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using AppliComptaApi.src.Models;

namespace AppliComptaApi.Models;

public class AppliContext : DbContext
{
    public AppliContext(DbContextOptions<AppliContext> options)
   : base(options)
    {

    }
    public DbSet<AppliComptaApi.src.Models.Customer> Customers { get; set; } = default!;

    public DbSet<AppliComptaApi.src.Models.AccountingDocument> AccountingDocuments { get; set; } = default!;

    public DbSet<AppliComptaApi.src.Models.User> Users { get; set; } = default!; 

    // configuration de la relation One To Many
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        modelBuilder.Entity<AccountingDocument>()
        .HasOne(a => a.Customer)
        .WithMany(c => c.AccountingDocuments)
        .HasForeignKey(a => a.CustomerId)
        .IsRequired(); // Indique que la relation est obligatoire pour un objet AccountingDocument

        modelBuilder.Entity<User>(o => o.HasIndex(p => p.Username).IsUnique());

        modelBuilder.Entity<Customer>(o => o.HasIndex(p => p.Number).IsUnique());

        modelBuilder.Entity<AccountingDocument>(o => o.HasIndex(p => p.Number).IsUnique());


    }

}



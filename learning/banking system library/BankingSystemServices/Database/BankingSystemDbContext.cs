﻿using BankingSystemServices.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingSystemServices.Database;

public class BankingSystemDbContext : DbContext
{
    public DbSet<Client> Clients { get; init; }
    public DbSet<Employee> Employees { get; init; }
    public DbSet<Account> Accounts { get; init; }
    public DbSet<Currency> Currencies { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>()
            .HasOne(accounts => accounts.Client)
            .WithMany()
            .HasForeignKey(accounts => accounts.ClientId);

        modelBuilder.Entity<Account>()
            .HasOne(accounts => accounts.Currency)
            .WithMany()
            .HasForeignKey(accounts => accounts.CurrencyId);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(
            "Host=localhost;" +
            "Port=5432;" +
            "Database=db_banking_system;" +
            "Username=postgres;" +
            "Password=2404");
    }
}
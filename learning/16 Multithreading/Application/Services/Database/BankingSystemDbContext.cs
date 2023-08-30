using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using BankingSystemServices;

namespace Services.Database;

public class BankingSystemDbContext : DbContext
{
    public DbSet<Client> Clients { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Currency> Currencies { get; set; }

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
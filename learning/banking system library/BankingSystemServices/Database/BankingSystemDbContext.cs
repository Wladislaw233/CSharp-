using BankingSystemServices.Models;
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
        // Названия таблиц.
        modelBuilder.Entity<Client>().ToTable("clients");
        modelBuilder.Entity<Employee>().ToTable("employees");
        modelBuilder.Entity<Account>().ToTable("accounts");
        modelBuilder.Entity<Currency>().ToTable("currencies");
        
        // Установка связей в таблицах.
        modelBuilder.Entity<Account>()
            .HasOne(accounts => accounts.Client)
            .WithMany()
            .HasForeignKey(accounts => accounts.ClientId);

        modelBuilder.Entity<Account>()
            .HasOne(accounts => accounts.Currency)
            .WithMany()
            .HasForeignKey(accounts => accounts.CurrencyId);

        // Настройка таблицы clients.
        modelBuilder.Entity<Client>(client =>
        {
            client.Property(c => c.ClientId)
                .HasColumnName("client_id")
                .IsRequired()
                .HasColumnOrder(0);

            client.Property(c => c.FirstName)
                .HasColumnName("first_name")
                .IsRequired()
                .HasColumnOrder(1)
                .HasMaxLength(150);

            client.Property(c => c.LastName)
                .HasColumnName("last_name")
                .IsRequired()
                .HasColumnOrder(2)
                .HasMaxLength(150);

            client.Property(c => c.DateOfBirth)
                .HasColumnName("date_of_birth")
                .IsRequired()
                .HasColumnOrder(3);

            client.Property(c => c.Age)
                .HasColumnName("age")
                .IsRequired()
                .HasColumnOrder(4);

            client.Property(c => c.Bonus)
                .HasColumnName("bonus")
                .HasColumnOrder(5)
                .HasPrecision(14,2);

            client.Property(c => c.PhoneNumber)
                .HasColumnName("phone_number")
                .IsRequired()
                .HasColumnOrder(6)
                .HasMaxLength(64);

            client.Property(c => c.Email)
                .HasColumnName("email")
                .IsRequired()
                .HasColumnOrder(7)
                .HasMaxLength(256);

            client.Property(c => c.Address)
                .HasColumnName("address")
                .IsRequired()
                .HasColumnOrder(8)
                .HasMaxLength(1024);
        });

        // Настройка таблицы employees.

        modelBuilder.Entity<Employee>(employee =>
        {
            employee.Property(e => e.EmployeeId)
                .HasColumnName("employee_id")
                .IsRequired()
                .HasColumnOrder(0);

            employee.Property(e => e.FirstName)
                .HasColumnName("first_name")
                .IsRequired()
                .HasColumnOrder(1)
                .HasMaxLength(150);

            employee.Property(e => e.LastName)
                .HasColumnName("last_name")
                .IsRequired()
                .HasColumnOrder(2)
                .HasMaxLength(150);

            employee.Property(e => e.DateOfBirth)
                .HasColumnName("date_of_birth")
                .IsRequired()
                .HasColumnOrder(3);

            employee.Property(e => e.Age)
                .HasColumnName("age")
                .IsRequired()
                .HasColumnOrder(4);

            employee.Property(e => e.Bonus)
                .HasColumnName("bonus")
                .HasColumnOrder(5)
                .HasPrecision(14, 2);

            employee.Property(e => e.PhoneNumber)
                .HasColumnName("phone_number")
                .IsRequired()
                .HasColumnOrder(6)
                .HasMaxLength(64);

            employee.Property(e => e.Email)
                .HasColumnName("email")
                .IsRequired()
                .HasColumnOrder(7)
                .HasMaxLength(256);

            employee.Property(e => e.Address)
                .HasColumnName("address")
                .IsRequired()
                .HasColumnOrder(8)
                .HasMaxLength(1024);
            employee.Property(e => e.Contract)
                .HasColumnName("contract")
                .IsRequired()
                .HasColumnOrder(9);

            employee.Property(e => e.Salary)
                .HasColumnName("salary")
                .IsRequired()
                .HasColumnOrder(10)
                .HasPrecision(14, 2);

            employee.Property(e => e.IsOwner)
                .HasColumnName("is_owner")
                .HasColumnOrder(11);
        });

        // Настройка таблицы accounts.

        modelBuilder.Entity<Account>(account =>
        {
            account.Property(a => a.AccountId)
                .HasColumnName("account_id")
                .IsRequired()
                .HasColumnOrder(0);

            account.Property(a => a.AccountNumber)
                .HasColumnName("account_number")
                .IsRequired()
                .HasColumnOrder(1)
                .HasMaxLength(25);

            account.Property(a => a.Amount)
                .HasColumnName("amount")
                .IsRequired()
                .HasColumnOrder(2)
                .HasPrecision(14, 2);

            account.Property(a => a.CurrencyId)
                .HasColumnName("currency_id")
                .IsRequired()
                .HasColumnOrder(3);

            account.Property(a => a.ClientId)
                .HasColumnName("client_id")
                .IsRequired()
                .HasColumnOrder(4);
        });

        // Настройка таблицы currencies.

        modelBuilder.Entity<Currency>(currency =>
        {
            currency.Property(c => c.CurrencyId)
                .HasColumnName("currency_id")
                .IsRequired()
                .HasColumnOrder(0);

            currency.Property(c => c.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasColumnOrder(1)
                .HasMaxLength(256);

            currency.Property(c => c.Code)
                .HasColumnName("code")
                .IsRequired()
                .HasColumnOrder(2)
                .HasMaxLength(3);

            currency.Property(c => c.ExchangeRate)
                .HasColumnName("exchange_rate")
                .IsRequired()
                .HasColumnOrder(3)
                .HasPrecision(14, 2);
        });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(
            "Host=localhost;" +
            "Port=5432;" +
            "Database=banking_system;" +
            "Username=postgres;" +
            "Password=2404");
    }
}
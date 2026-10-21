using Microsoft.EntityFrameworkCore;
using TestInvoiceWebAPI.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Invoice> Invoices { get; set; } = null!;
    public DbSet<InvoiceItem> InvoiceItems { get; set; } = null!;
    public DbSet<AppUser> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppUser>()
            .HasIndex(user => user.UserName)
            .IsUnique();

        modelBuilder.Entity<Invoice>()
            .HasMany(invoice => invoice.Items)
            .WithOne(item => item.Invoice)
            .HasForeignKey(item => item.InvoiceID)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

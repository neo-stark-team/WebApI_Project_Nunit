using Microsoft.EntityFrameworkCore;

namespace dotnetapp.Models
{
    public class ExpenseTrackerApiDbContext : DbContext
    {
        public ExpenseTrackerApiDbContext(DbContextOptions<ExpenseTrackerApiDbContext> options) : base(options)
        {
        }

        public DbSet<ExpenseTrackerApi> ExpenseTrackerApis { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExpenseTrackerApi>().ToTable("ExpenseTrackerApi");
            modelBuilder.Entity<ExpenseTrackerApi>().HasKey(t => t.Id);
            modelBuilder.Entity<ExpenseTrackerApi>().Property(t => t.Expense_Date).HasColumnType("datetime2").IsRequired();
            modelBuilder.Entity<ExpenseTrackerApi>().Property(t => t.Amount).IsRequired().HasColumnType("decimal(10,2)");
            modelBuilder.Entity<ExpenseTrackerApi>().Property(t => t.Description).HasMaxLength(100);

        }
    }
}

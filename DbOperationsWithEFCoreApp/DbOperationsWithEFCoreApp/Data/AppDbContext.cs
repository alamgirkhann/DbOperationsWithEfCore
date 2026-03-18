using Microsoft.EntityFrameworkCore;

namespace DbOperationsWithEFCoreApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Currency>().HasData(
                new Currency() { Id = 1, Title = "PKR", Description = "Pakistani Currency"},
                new Currency() { Id = 2, Title = "USD", Description = "American Currency" },
                new Currency() { Id = 4, Title = "EUR", Description = "Europien Currency" },
                new Currency() { Id = 5, Title = "GBP", Description = "Britsh Currency" }
            );

            modelBuilder.Entity<Language>().HasData(
                new Language() { Id = 1, Title = "Urdu", Description = "Pakistani Language" },
                new Language() { Id = 2, Title = "English", Description = "American Language" },
                new Language() { Id = 4, Title = "German", Description = "German Language" },
                new Language() { Id = 5, Title = "GBP", Description = "Britsh Language" }
            );
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<BookPrice> BookPrices { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}

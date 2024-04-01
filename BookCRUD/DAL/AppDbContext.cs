using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class AppDbContext: DbContext
{
    public DbSet<Book> Books { get; set; } = null!;
    public DbSet<Author> Authors { get; set; } = null!;
    public DbSet<Publisher> Publishers { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Comment> Comments { get; set; } = null!;
    public DbSet<BookAuthor> BookAuthors { get; set; } = null!;
    public DbSet<BookCategory> BookCategories { get; set; } = null!;
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}
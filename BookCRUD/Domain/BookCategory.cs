namespace Domain;

public class BookCategory: BaseEntity
{
    public Guid BookId { get; set; }
    public Book? Book { get; set; } 
    
    public Guid CategoryId { get; set; }
    public Category? Category { get; set; }
}
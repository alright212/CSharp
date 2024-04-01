using System.ComponentModel.DataAnnotations;

namespace Domain;

public class Book: BaseEntity
{
    [MaxLength(128)]
    public string Title { get; set; } = default!;
    public string Description { get; set; } =default!;
    public ICollection<BookAuthor>? Authors { get; set; }
    
    public Guid PublisherId { get; set; }
    public Publisher? Publisher { get; set; }
    
    public ICollection<BookCategory>? Categories { get; set; }
    
    public ICollection<Comment>? Comments { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace Domain;

public class Category : BaseEntity
{
    [MaxLength(32)] 
    public string Label { get; set; }= default!;
    public ICollection<BookCategory>? Books { get; set; }
}
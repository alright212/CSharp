using System.ComponentModel.DataAnnotations;

namespace Domain;

public class Publisher: BaseEntity
{
    [MaxLength(128)]
    public string Name { get; set; } = default!;
    
    public ICollection<Book>? Books { get; set; }
}
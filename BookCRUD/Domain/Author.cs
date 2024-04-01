using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Domain;

public class Author: BaseEntity
{
    [MaxLength(128)]
    public string FirstName { get; set; } = default!;
    [MaxLength(128)]
    public string LastName { get; set; } = default!;
    public EGender Gender { get; set; }
    public ICollection<BookAuthor>? Books { get; set; }
    
    
}
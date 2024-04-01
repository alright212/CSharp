using System.ComponentModel.DataAnnotations;

namespace Domain;

public class Comment: BaseEntity
{
    [MaxLength(128)]
    public string CommentText { get; set; }= default!;

    public DateTime CreatedAt { get; set; }= DateTime.Now;
    public Guid? ParentCommentId { get; set; }
    public Comment? ParentComment { get; set; }
    
    public Guid BookId { get; set; }
    public Book? Book { get; set; }
}
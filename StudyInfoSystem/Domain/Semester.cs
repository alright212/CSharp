using System.ComponentModel.DataAnnotations;

namespace Domain;

public class Semester
{
    public int Id { get; set; }

    [MaxLength(64)]
    public string? Name { get; set; }=default!;

    public ICollection<StudentSubject>? StudentSubjects { get; set; }
}

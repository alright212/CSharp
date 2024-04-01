using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain;

public class Student
{
    public int Id { get; set; }

    [MaxLength(128)]
    public string Name { get; set; }=default!;

    public ICollection<StudentSubject>? StudentSubjects { get; set; }
    [NotMapped]
    public int TotalECTS { get; set; }
}

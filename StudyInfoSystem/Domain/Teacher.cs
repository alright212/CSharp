using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain;

public class Teacher
{
    public int Id { get; set; }

    [MaxLength(128)]
    public string Name { get; set; }= default!;

    public ICollection<SubjectTeacher>? SubjectTeachers { get; set; }
    [NotMapped]
    public string Subjects { get; set; }= default!;
}

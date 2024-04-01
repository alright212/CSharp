namespace Domain;

public class StudentSubject
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public Student? Student { get; set; }

    public int SubjectId { get; set; }
    public Subject? Subject { get; set; }

    public int SemesterId { get; set; }
    public Semester? Semester { get; set; }

    public ICollection<Grade>? Grades { get; set; }

    public double AverageGrade { get; set; }
    public EFinalGrade EFinalGrade { get; set; }
}

using System.ComponentModel.DataAnnotations;

namespace Domain;

public class Grade
{
    public int Id { get; set; }
    [Range(0, 5, ErrorMessage = "Value must be between 0 and 5.")]

    public double Value { get; set; }
    public int StudentSubjectId { get; set; }
    public StudentSubject? StudentSubject { get; set; }
    public string StudentSubjectName => $"{StudentSubject?.Student!.Name} - {StudentSubject?.Subject!.Name}";

    public string Description { get; set; } = default!;

}
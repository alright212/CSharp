using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain;

[Index(nameof(TeacherId), nameof(SubjectId), IsUnique = true)]
public class SubjectTeacher
{
    public int Id { get; set; }

    public int TeacherId { get; set; }
    public Teacher? Teacher { get; set; }

    public int SubjectId { get; set; }
    public Subject? Subject { get; set; }
}
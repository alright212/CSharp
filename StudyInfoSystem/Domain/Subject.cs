using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Subject
    {
        public int Id { get; set; }

        [MaxLength(128)]
        public string Name { get; set; } = default!;

        [MaxLength(512)]
        public string Description { get; set; } = default!;

        public int ECTS { get; set; }

        public ICollection<SubjectTeacher>? SubjectTeachers { get; set; }
        public ICollection<StudentSubject>? StudentSubjects { get; set; }
        public ICollection<Exercise>? Exercises { get; set; }
    }
}
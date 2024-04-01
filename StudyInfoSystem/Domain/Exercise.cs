using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Exercise
    {
        public int Id { get; set; }

        [MaxLength(128)]
        public string Name { get; set; } = default!;

        [MaxLength(512)]
        public string Description { get; set; } = default!;

        public double Grade { get; set; }

        public int SubjectId { get; set; }
        public Subject? Subject { get; set; }
    }
}
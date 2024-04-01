using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Student> Students { get; set; } = null!;
    public DbSet<Teacher> Teachers { get; set; } = null!;
    public DbSet<Subject> Subjects { get; set; } = null!;
    public DbSet<Semester> Semesters { get; set; } = null!;
    public DbSet<Grade> Grades { get; set; } = null!;
    public DbSet<SubjectTeacher> SubjectTeachers { get; set; } = null!;
    public DbSet<StudentSubject> StudentSubjects { get; set; } = null!;
    public DbSet<Exercise> Exercises { get; set; } = null!;
}
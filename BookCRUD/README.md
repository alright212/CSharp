dotnet aspnet-codegenerator razorpage -m Domain.Author -outDir Pages/Authors -dc AppDbContext -udl --referenceScriptLibraries
dotnet aspnet-codegenerator razorpage -m Domain.Book -outDir Pages/Books -dc AppDbContext -udl --referenceScriptLibraries
dotnet aspnet-codegenerator razorpage -m Domain.BookAuthor -outDir Pages/BookAuthors -dc AppDbContext -udl --referenceScriptLibraries
dotnet aspnet-codegenerator razorpage -m Domain.BookCategory -outDir Pages/BookCategories -dc AppDbContext -udl --referenceScriptLibraries
dotnet aspnet-codegenerator razorpage -m Domain.Category -outDir Pages/Categories -dc AppDbContext -udl --referenceScriptLibraries
dotnet aspnet-codegenerator razorpage -m Domain.Comment -outDir Pages/Comments -dc AppDbContext -udl --referenceScriptLibraries
dotnet aspnet-codegenerator razorpage -m Domain.Publisher -outDir Pages/Publishers -dc AppDbContext -udl --referenceScriptLibraries

dotnet aspnet-codegenerator razorpage -m Domain.Recipe -outDir Pages/Recipes -dc AppDbContext -udl --referenceScriptLibraries
dotnet aspnet-codegenerator razorpage -m Domain.Ingredient -outDir Pages/Ingredients -dc AppDbContext -udl --referenceScriptLibraries
dotnet aspnet-codegenerator razorpage -m Domain.RecipeIngredient -outDir Pages/RecipeIngredients -dc AppDbContext -udl --referenceScriptLibraries

dotnet aspnet-codegenerator razorpage -m Domain.Student -outDir Pages/Students -dc AppDbContext -udl --referenceScriptLibraries
dotnet aspnet-codegenerator razorpage -m Domain.Teacher -outDir Pages/Teachers -dc AppDbContext -udl --referenceScriptLibraries
dotnet aspnet-codegenerator razorpage -m Domain.Subject -outDir Pages/Subjects -dc AppDbContext -udl --referenceScriptLibraries
dotnet aspnet-codegenerator razorpage -m Domain.Semester -outDir Pages/Semesters -dc AppDbContext -udl --referenceScriptLibraries
dotnet aspnet-codegenerator razorpage -m Domain.Grade -outDir Pages/Grades -dc AppDbContext -udl --referenceScriptLibraries
dotnet aspnet-codegenerator razorpage -m Domain.StudentSubject -outDir Pages/StudentSubjects -dc AppDbContext -udl --referenceScriptLibraries
dotnet aspnet-codegenerator razorpage -m Domain.SubjectTeacher -outDir Pages/SubjectTeachers -dc AppDbContext -udl --referenceScriptLibraries

dotnet aspnet-codegenerator razorpage -m Domain.Exercise -outDir Pages/Exercises -dc AppDbContext -udl --referenceScriptLibraries


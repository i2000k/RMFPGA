

namespace Domain.Entities
{
    public class Student : User
    {
        public string Group { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Grade { get; set; }
        public int GradeYear { get; set; }

        public Student(
            string email,
            byte[] passwordHash,
            byte[] passwordSalt,
            string group,
            string firstName,
            string secondName,
            string grade,
            int gradeYear
            ) : base(email, passwordHash, passwordSalt)
        {
            Group = group;
            FirstName = firstName;
            SecondName = secondName;
            Grade = grade;
            GradeYear = gradeYear;
        }
    }
}

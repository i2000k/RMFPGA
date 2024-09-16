using Domain.Entities;
using System.ComponentModel;

namespace Domain.Dtos;

public class StudentDto : UserDto
{
    public string Group { get; set; }
    public string FirstName { get; set; }
    public string SecondName { get; set; }
    public string Grade { get; set; }
    public int GradeYear { get; set; }
    public IEnumerable<Session>? Sessions { get; set; }

    public StudentDto(
        string email,
        string password,
        string group,
        string firstName,
        string secondName,
        string grade,
        int gradeYear,
        IEnumerable<Session>? sessions) : base(email, password)
    {
        Group = group;
        FirstName = firstName;
        SecondName = secondName;
        Grade = grade;
        GradeYear = gradeYear;
        Sessions = sessions;
    }
}
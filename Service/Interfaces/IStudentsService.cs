using System.Runtime.InteropServices;
using Domain.Dtos;
using Domain.Entities;

namespace Application.Interfaces;

public interface IStudentsService
{
    IEnumerable<GetStudentDto>? GetStudents();
    GetStudentDto? GetStudent(Guid id);
    GetStudentDto CreateStudent(StudentDto dto);
    GetStudentDto? UpdateStudent(StudentDto dto, Guid id);
    GetStudentDto? RemoveStudent(Guid id);
}
using Application.Interfaces;
using Application.Utils;
using Domain.Dtos;
using Domain.Entities;
using Persistence;

namespace Application.Services;

public class StudentsService : IStudentsService
{
    private readonly RfContext _dbContext;
    public StudentsService(RfContext dbContext)
    {
        _dbContext = dbContext;
    }
    public IEnumerable<GetStudentDto>? GetStudents()
    {
        var students = _dbContext.Students?.ToList();

        if (students?.Count == 0 || students == null)
        {
            return null;
        }

        var studentsDto = students.Select(student => new GetStudentDto()
        {
            Id = student.Id,
            Email = student.Email,
            FirstName = student.FirstName,
            SecondName = student.SecondName,
            Grade = student.Grade,
            GradeYear = student.GradeYear,
            Group = student.Group,
            Sessions = _dbContext.Sessions?
                                 .Where(s => s.UserId == student.Id)
                                 .Select(s => new GetSessionDto(s.UserId, s.StandId, s.State))
        });
        return studentsDto;
    }

    public GetStudentDto? GetStudent(Guid id)
    {
        var student = _dbContext.Students?.FirstOrDefault(x => x.Id == id);
        if (student == null)
        {
            return null;
        }

        var result = new GetStudentDto()
        {
            Id = student.Id,
            Email = student.Email,
            FirstName = student.FirstName,
            SecondName = student.SecondName,
            Grade = student.Grade,
            GradeYear = student.GradeYear,
            Group = student.Group,
            Sessions = _dbContext.Sessions?
                                 .Where(s => s.UserId == student.Id)
                                 .Select(s => new GetSessionDto(s.UserId, s.StandId, s.State))
        };

        return result;
    }

    public GetStudentDto CreateStudent(StudentDto dto)
    {
        AuthUtils.CreatePasswordHash(dto.Password, out byte[] passwordHash, out byte[] passwordSalt);
        var student = new Student(
            dto.Email,
            passwordHash,
            passwordSalt,
            dto.Group,
            dto.FirstName,
            dto.SecondName,
            dto.Grade,
            dto.GradeYear
            );
        _dbContext.Students?.Add(student);
        _dbContext.SaveChanges();

        var result = new GetStudentDto()
        {
            Id = student.Id,
            Email = student.Email,
            FirstName = student.FirstName,
            SecondName = student.SecondName,
            Grade = student.Grade,
            GradeYear = student.GradeYear,
            Group = student.Group,
            Sessions = _dbContext.Sessions?
                                 .Where(s => s.UserId == student.Id)
                                 .Select(s => new GetSessionDto(s.UserId, s.StandId, s.State))
        };

        return result;
    }

    public GetStudentDto? UpdateStudent(StudentDto dto, Guid id)
    {
        var target = _dbContext.Students?.FirstOrDefault(y => y.Id == id);
        if (target == null)
        {
            return null;
        }

        target.FirstName = dto.FirstName;
        target.SecondName = dto.SecondName;
        target.Grade = dto.Grade;
        target.GradeYear = dto.GradeYear;
        target.Group = dto.Group;
        target.Email = dto.Email;
        AuthUtils.CreatePasswordHash(dto.Password, out byte[] passwordHash, out byte[] passwordSalt);
        target.PasswordHash = passwordHash;
        target.PasswordSalt = passwordSalt;
        _dbContext.SaveChanges();

        var result = new GetStudentDto()
        {
            Id = target.Id,
            Email = target.Email,
            FirstName = target.FirstName,
            SecondName = target.SecondName,
            Grade = target.Grade,
            GradeYear = target.GradeYear,
            Group = target.Group,
            Sessions = _dbContext.Sessions?
                         .Where(s => s.UserId == target.Id)
                         .Select(s => new GetSessionDto(s.UserId, s.StandId, s.State))
        };

        return result;
    }

    public GetStudentDto? RemoveStudent(Guid id)
    {
        var target = _dbContext.Students?.FirstOrDefault(x => x.Id == id);
        if (target == null)
        {
            return null;
        }

        var sessions = _dbContext.Sessions?.Where(s => s.UserId == id).ToList();
        if (sessions is not null)
        {
            _dbContext.RemoveRange(sessions);
        }

        var student = _dbContext.Students?.Remove(target).Entity;
        if (student == null)
        {
            return null;
        }

        var result = new GetStudentDto()
        {
            Email = student.Email,
            FirstName = student.FirstName,
            SecondName = student.SecondName,
            Grade = student.Grade,
            GradeYear = student.GradeYear,
            Group = student.Group,
            Sessions = _dbContext.Sessions?
                                 .Where(s => s.UserId == student.Id)
                                 .Select(s => new GetSessionDto(s.UserId, s.StandId, s.State))
        };

        _dbContext.SaveChanges();
        return result;
    }
}
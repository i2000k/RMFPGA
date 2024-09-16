namespace Domain.Dtos;

public class GetStudentDto
{
    public Guid? Id { get; set; }
    public string? Email { get; set; }
    public string? Group { get; set; }
    public string? FirstName { get; set; }
    public string? SecondName { get; set; }
    public string? Grade { get; set; }
    public int GradeYear { get; set; }
    public IEnumerable<GetSessionDto>? Sessions { get; set; }
}
namespace Domain.Dtos;

public class BoardDataDto
{
    public Guid UserId { get; set; }
    public Guid StandId { get; set; }
    public short[] Data { get; set; }
}
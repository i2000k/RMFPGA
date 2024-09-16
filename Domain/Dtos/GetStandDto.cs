using Domain.Entities;
using Domain.Enums;

namespace Domain.Dtos;

public class GetStandDto
{
    public Guid? Id { get; set; }
    public string? BoardTitle { get; set; }
    public string? ConnectionUrl { get; set; }
    public StandState State { get; set; }
    public IEnumerable<GetSessionDto>? Sessions { get; set; }
}
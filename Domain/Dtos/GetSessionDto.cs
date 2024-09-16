using Domain.Entities;
using Domain.Enums;

namespace Domain.Dtos;

public class GetSessionDto
{
    public Guid UserId { get; set; }
    public Guid StandId { get; set; }
    public SessionState State { get; set; }

    public GetSessionDto(Guid userId, Guid standId, SessionState state)
    {
        UserId = userId;
        StandId = standId;
        State = state;
    }
}
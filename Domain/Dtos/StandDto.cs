using Domain.Entities;
using Domain.Enums;

namespace Domain.Dtos;

public class StandDto
{
    public string? BoardTitle { get; set; }
    public string? ConnectionUrl { get; set; }
    public StandState State { get; set; } = StandState.Available;

    public StandDto() { }

    public StandDto(string? boardTitle, string? connectionUrl, StandState state)
    {
        BoardTitle = boardTitle;
        ConnectionUrl = connectionUrl;
        State = state;
    }
}
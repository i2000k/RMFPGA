using Domain.Dtos;

namespace Application.Interfaces;

public interface IStandsService
{
    IEnumerable<GetStandDto>? GetStands();
    IEnumerable<GetAvailableStandDto>? GetAvailable();
    GetStandDto? GetStand(Guid id);
    Guid CreateStand(StandDto dto);
    Guid? UpdateStand(StandDto dto, Guid id);
    bool? DisableStand(Guid id);
    bool? EnableStand(Guid id);
    Guid? RemoveStand(Guid id);
}
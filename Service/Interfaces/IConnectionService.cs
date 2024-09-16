using Domain.Dtos;

namespace Application.Interfaces;

public interface IConnectionService
{
    Guid? StartConnection(StartConnectionDto sessionParams);
    GetSessionDto? CloseConnection(Guid id);
    void CloseAllOpened();
}
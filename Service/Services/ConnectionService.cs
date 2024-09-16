using Application.Interfaces;
using Domain.Dtos;
using Domain.Entities;
using Domain.Enums;
using Microsoft.IdentityModel.Tokens;
using Persistence;

namespace Application.Services;

public class ConnectionService : IConnectionService
{
    private readonly RfContext _dbContext;

    public ConnectionService(RfContext dbContext)
    {
        _dbContext = dbContext;
    }
    public Guid? StartConnection(StartConnectionDto sessionParams)
    {
        // Проверка, что у данного пользователя и у стенда нет активных подключений
        bool admin = false;
        User? user = _dbContext.Students?.FirstOrDefault(x => x.Id == sessionParams.UserId);

        if (user is null)
        {
            user = _dbContext.Administrators?.Find(sessionParams.UserId);
            admin = true;
        }

        if (user is null)
        {
            return null;
        }

        user.Sessions = _dbContext.Sessions?.Where(s => s.UserId == user.Id).ToList();

        if (user.Sessions != null && user.Sessions.Any(s => s.State == SessionState.Active))
        {
            return null;
        }

        var stand = _dbContext.Stands?.FirstOrDefault(x => x.Id == sessionParams.StandId);

        if (stand == null)
        {
            return null;
        }

        if (stand.State != StandState.Available)
        {
            return null;
        }

        stand.Sessions = _dbContext.Sessions?.Where(s => s.StandId == stand.Id).ToList();

        if (stand.Sessions != null && stand.Sessions.Any(s => s.State == SessionState.Active))
        {
            return null;
        }

        // Создаем новую сессию

        var session = new Session
        {
            User = admin ? 
                _dbContext.Administrators?.FirstOrDefault(x => x.Id == sessionParams.UserId) : 
                _dbContext.Students?.FirstOrDefault(x => x.Id == sessionParams.UserId),
            Stand = _dbContext.Stands?.FirstOrDefault(x => x.Id == sessionParams.StandId),
            StandId = sessionParams.StandId,
            UserId = sessionParams.UserId,
            State = SessionState.Active,
        };

        user.Sessions?.Append(session);
        stand.Sessions?.Append(session);
        stand.State = StandState.Active;
        _dbContext.Sessions?.Add(session);
        _dbContext.SaveChanges();
        return session.Id;
    }

    public GetSessionDto? CloseConnection(Guid id)
    {
        var target = _dbContext.Sessions?.FirstOrDefault(x => x.Id == id);

        if (target == null)
        {
            return null;
        }

        if (target.State == SessionState.Active)
        {
            target.State = SessionState.Inactive;
        }

        var stand = _dbContext.Stands?.FirstOrDefault(x => x.Id == target.StandId);

        if (stand != null) 
            stand.State = StandState.Available;

        var result = new GetSessionDto(target.UserId, target.StandId, target.State);

        _dbContext.SaveChanges();
        return result;
    }


    public void CloseAllOpened()
    {
        var openedConnections = _dbContext.Sessions?.Where(s => s.State == SessionState.Active);
        if (openedConnections is not null && openedConnections.Count() > 0)
        {
            var activeStands = openedConnections.Select(c => c.Stand);
            
            foreach (var stand in activeStands)
            {
                Console.WriteLine(stand.Id);
                stand.State = StandState.Available;
            }

            foreach (var connection in openedConnections)
            {
                connection.State = SessionState.Inactive;
            }
            _dbContext.SaveChanges();
        }
    }
}
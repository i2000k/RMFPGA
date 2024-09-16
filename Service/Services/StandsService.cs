using Application.Interfaces;
using Domain.Dtos;
using Domain.Entities;
using Domain.Enums;
using Infrastructure;
using Persistence;

namespace Application.Services;

public class StandsService : IStandsService
{
    private readonly RfContext _dbContext;

    public StandsService(RfContext dbContext)
    {
        _dbContext = dbContext;
    }
    public IEnumerable<GetStandDto>? GetStands()
    {
        var stands = _dbContext.Stands?.ToList();

        if (stands?.Count == 0 || stands == null)
        {
            return null;
        }

        var standsDto = stands.Select(stand => new GetStandDto()
        {
            Id = stand.Id,
            BoardTitle = stand.BoardTitle,
            ConnectionUrl = stand.ConnectionUrl,
            State = stand.State,
            Sessions = _dbContext.Sessions?
            .Where(s => s.StandId == stand.Id)
            .Select(s => new GetSessionDto(s.UserId, s.StandId, s.State))
        });
        return standsDto;
    }

    public IEnumerable<GetAvailableStandDto>? GetAvailable()
    {
        var stands = _dbContext.Stands?.Where(s => s.State == StandState.Available).ToList();
        if (stands is null || stands.Count == 0)
        {
            return null;
        }

        var standsDto = stands.Select(stand => new GetAvailableStandDto()
        {
            Id = stand.Id,
            BoardTitle = stand.BoardTitle,
            State = stand.State,
        });

        return standsDto;
    }

    public GetStandDto? GetStand(Guid id)
    {
        var stand = _dbContext.Stands?.FirstOrDefault(x => x.Id == id);
        if (stand == null)
        {
            return null;
        }

        var result = new GetStandDto()
        {
            Id = stand.Id,
            BoardTitle = stand.BoardTitle,
            ConnectionUrl = stand.ConnectionUrl,
            State = stand.State,
            Sessions = _dbContext.Sessions?
            .Where(s => s.StandId == stand.Id)
            .Select(s => new GetSessionDto(s.UserId, s.StandId, s.State))
        };
        return result;
    }

    public Guid CreateStand(StandDto dto)
    {
        var stand = new Stand(
            boardTitle: dto.BoardTitle,
            connectionUrl: dto.ConnectionUrl,
            state: dto.State
        );
        _dbContext.Stands?.Add(stand);
        _dbContext.SaveChanges();
        return stand.Id;
    }

    public Guid? UpdateStand(StandDto dto, Guid id)
    {
        var target = _dbContext.Stands?.Find(id);
        if (target == null)
        {
            return null;
        }

        target.BoardTitle = dto.BoardTitle;
        target.ConnectionUrl = dto.ConnectionUrl;
        if (target.State != StandState.Active)
            target.State = dto.State;
        

        _dbContext.SaveChanges();

        return target.Id;
    }

    // Метод для выключения стенда
    // возвращает null, если нет стенда с указанным Id
    // возвращает false, если в данный момент используется
    // возвращает true, если выключить удалось успешно
    public bool? DisableStand(Guid id)
    {
        var target = _dbContext.Stands?.Find(id);
        if (target == null)
        {
            return null;
        }

        if (target.State == StandState.Active)
        {
            return false;
        }

        if (target.State == StandState.Available)
            target.State = StandState.Inactive;
        _dbContext.SaveChanges();
        return true;
    }

    // Метод для включения стенда
    // возвращает null, если нет стенда с указанным Id
    // возвращает false, если в данный момент используется
    // возвращает true, если включить удалось успешно
    public bool? EnableStand(Guid id)
    {
        var target = _dbContext.Stands?.Find(id);
        if (target == null)
        {
            return null;
        }

        if (target.State == StandState.Active)
        {
            return false;
        }

        if (target.State == StandState.Inactive)
            target.State = StandState.Available;

        _dbContext.SaveChanges();

        return true;
    }

    public Guid? RemoveStand(Guid id)
    {
        var target = _dbContext.Stands?.FirstOrDefault(x => x.Id == id);
        if (target == null || target.State == StandState.Active)
        {
            return null;
        }

        var sessions = _dbContext.Sessions?.Where(s => s.StandId == id).ToList();
        if (sessions is not null)
        {
            _dbContext.RemoveRange(sessions);
        }

        var stand = _dbContext.Stands?.Remove(target).Entity;

        if (stand == null)
        {
            return null;
        }

        var result = new StandDto(stand.BoardTitle, stand.ConnectionUrl, stand.State);

        _dbContext.SaveChanges();
        return id;
    } 
}
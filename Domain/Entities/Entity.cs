

namespace Domain.Entities;
public abstract class Entity
{
    protected Entity()
    {
        Id = Guid.NewGuid();
        DateCreated = DateTime.Now.ToUniversalTime();
    }
    public Guid Id { get; }
    public DateTime DateCreated { get; }
}



using Domain.Enums;

namespace Domain.Entities
{
    public class Session : Entity
    {
        public Guid UserId { get; set; }
        public Guid StandId { get; set; }
        public User? User { get; set; }
        public Stand? Stand { get; set; }
        public SessionState State { get; set; } = SessionState.Inactive;
        public byte[]? DesignFile { get; set; }
    }
}

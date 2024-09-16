using Domain.Enums;

namespace Domain.Dtos
{
    public class GetAvailableStandDto
    {
        public Guid Id { get; set; }
        public string? BoardTitle { get; set; }
        public StandState State { get; set;  }
    }
}

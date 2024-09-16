
using Domain.Enums;


namespace Domain.Entities
{
    public class Stand : Entity
    {
        public string? BoardTitle { get; set; }
        public string? ConnectionUrl { get; set; }
        public StandState State { get; set; }
        public IEnumerable<Session>? Sessions { get; set; } = new List<Session>();
        //public Session? CurrentSession { get; set; }
        public Stand() { }
        public Stand(string? boardTitle, string? connectionUrl, StandState state)
        {
            BoardTitle = boardTitle;
            ConnectionUrl = connectionUrl;
            State = state;
        }
    }
}


namespace Domain.Entities
{
    public abstract class User : Entity
    {
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime TokenCreated { get; set; } = DateTime.Now.ToUniversalTime();
        public DateTime TokenExpires { get; set; } = DateTime.Now.AddDays(7).ToUniversalTime();
        public IEnumerable<Session>? Sessions { get; set; } = new List<Session>();

        // public string Role { get; set; } = "Student";
        //public Session? CurrentSession { get; set; }
        protected User(string email, byte[] passwordHash, byte[] passwordSalt)
        {
            Email = email;
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
        }
    }
}

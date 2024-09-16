

namespace Domain.Entities
{
    public class Administrator : User
    {
        public Administrator(
            string email,
            byte[] passwordHash,
            byte[] passwordSalt
        ) : base(email, passwordHash, passwordSalt) { }
    }
}

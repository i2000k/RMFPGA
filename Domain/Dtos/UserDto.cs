namespace Domain.Dtos
{
    public class UserDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public UserDto(string email, string password) 
        {
            Email = email;
            Password = password;
        }
    }
}

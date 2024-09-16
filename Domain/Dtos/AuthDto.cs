using Domain.Enums;

namespace Domain.Dtos
{
	public class AuthDto
	{

        public Guid Id { get; set; }
        public string AccessToken { get; set; }
        public string Role { get; set; }

        public AuthDto(Guid id, string token, string role)
		{
			Id = id;
			AccessToken = token;
			Role = role;
		}
	}
}


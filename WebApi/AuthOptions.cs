using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WebApi
{
    public class AuthOptions
    {
        private readonly IConfiguration _configuration;
        public AuthOptions(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // представляет издателя токена. Здесь можно определить любое название.
        public const string ISSUER = "MyAuthServer"; // издатель токена
        // представляет потребителя токена, может быть любая строка, обычно это сайт, на котором применяется токен.
        public const string AUDIENCE = "MyAuthClient"; // потребитель токена
        // хранит ключ, который будет применяться для создания токена.
        const string KEY = "mysupersecret_secretkey!123";   // ключ для шифрации
        //возвращает ключ безопасности, который применяется для генерации токена.
        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
    }
}

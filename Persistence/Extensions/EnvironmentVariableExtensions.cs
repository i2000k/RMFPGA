using Microsoft.Extensions.Configuration;

namespace Persistence.Extensions
{
    public static class EnvironmentVariableExtensions
    {
        public static string? GetConnectionStringFromEnvironment(this IConfiguration configuration, string? name = null)
        {
            return Environment.GetEnvironmentVariable(string.IsNullOrEmpty(name) ? "Default" : name);
        }
    }
}

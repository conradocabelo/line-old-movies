using LEN.Services.Identidade.Extensions;

namespace LEN.Services.Identidade.Configuration
{
    public static class ServiceConfig
    {
        public static IServiceCollection AddServices(this IServiceCollection service)
        {
            return service.AddScoped<ITokenFactory, TokenFactory>();
        }
    }
}

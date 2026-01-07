using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using OfflineTicketing.Application.Abstractions;
using OfflineTicketing.Application.Services;
using OfflineTicketing.Domain.Entities;
using OfflineTicketing.Infrastructure.Persistence;
using OfflineTicketing.Infrastructure.Persistence.Repositories;
using OfflineTicketing.Infrastructure.Security;

namespace OfflineTicketing.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(opt =>
        {
            var cs = configuration.GetConnectionString("Default");
            opt.UseSqlServer(cs);
        });

        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));

        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUser, CurrentUser>();

        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITicketRepository, TicketRepository>();

        services.AddScoped<AuthService>();
        services.AddScoped<TicketService>();
        services.AddScoped<TicketStatsService>();

        services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();

        return services;
    }
}

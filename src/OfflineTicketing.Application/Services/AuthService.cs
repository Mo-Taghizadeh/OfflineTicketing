using Microsoft.AspNetCore.Identity;
using OfflineTicketing.Application.Abstractions;
using OfflineTicketing.Application.Contracts.Auth;
using OfflineTicketing.Domain.Entities;

namespace OfflineTicketing.Application.Services;

public sealed class AuthService
{
    private readonly IUserRepository _users;
    private readonly IJwtTokenService _tokens;
    private readonly IPasswordHasher<User> _passwordHasher;

    public AuthService(IUserRepository users, IJwtTokenService tokens, IPasswordHasher<User> passwordHasher)
    {
        _users = users;
        _tokens = tokens;
        _passwordHasher = passwordHasher;
    }

    public async Task<LoginResponse?> LoginAsync(LoginRequest request, CancellationToken ct)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var user = await _users.FindByEmailAsync(email, ct);
        if (user is null) return null;

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (result == PasswordVerificationResult.Failed) return null;

        var (token, expiresAtUtc) = _tokens.CreateToken(user);
        return new LoginResponse(token, expiresAtUtc);
    }
}

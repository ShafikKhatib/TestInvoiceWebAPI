using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestInvoiceWebAPI.Models;
using TestInvoiceWebAPI.Services;

namespace TestInvoiceWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IPasswordHasher<AppUser> _passwordHasher;
    private readonly JwtTokenService _tokenService;

    public AuthController(
        AppDbContext context,
        IPasswordHasher<AppUser> passwordHasher,
        JwtTokenService tokenService)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(
        RegisterRequest request,
        CancellationToken cancellationToken)
    {
        var userName = request.UserName.Trim();

        var exists = await _context.Users
            .AnyAsync(user => user.UserName == userName, cancellationToken);

        if (exists)
        {
            return Conflict("A user with this username already exists.");
        }

        var user = new AppUser
        {
            UserName = userName,
            Role = Roles.User
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        return Ok(_tokenService.CreateToken(user));
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(
        LoginRequest request,
        CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .SingleOrDefaultAsync(
                candidate => candidate.UserName == request.UserName.Trim(),
                cancellationToken);

        if (user is null)
        {
            return Unauthorized("Invalid username or password.");
        }

        var passwordResult = _passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            request.Password);

        if (passwordResult == PasswordVerificationResult.Failed)
        {
            return Unauthorized("Invalid username or password.");
        }

        return Ok(_tokenService.CreateToken(user));
    }
}
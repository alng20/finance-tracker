using System.Security.Principal;

using FinanceTracker.Api.Common.Options;
using FinanceTracker.Api.Responses;
using FinanceTracker.Application.Common.Models;
using FinanceTracker.Application.Queries.GetCurrentUser;
using FinanceTracker.Application.Users.Authentication.Login;
using FinanceTracker.Application.Users.Authentication.Logout;
using FinanceTracker.Application.Users.Authentication.Refresh;
using FinanceTracker.Application.Users.Authentication.Register;
using FinanceTracker.Application.Users.DTOs;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FinanceTracker.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthenticationController(
    IMediator mediator,
    IOptions<RefreshTokenCookieOptions> refreshTokenOptions,
    ILogger<AuthenticationController> logger
) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly RefreshTokenCookieOptions _refreshTokenOptions = refreshTokenOptions.Value;
    private readonly ILogger<AuthenticationController> _logger = logger;

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(
        [FromBody] RegisterCommand cmd,
        CancellationToken cancellationToken
    )
    {
        var result = await _mediator.Send(cmd, cancellationToken);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(
        [FromBody] LoginCommand cmd,
        CancellationToken cancellationToken
    )
    {
        var result = await _mediator.Send(cmd, cancellationToken);
        AppendRefreshTokenCookie(result.RefreshToken);

        var response = new LoginResponse(
            result.AccessToken.Token,
            result.AccessToken.ExpiresAt,
            result.FirstName,
            result.LastName
        );
        return Ok(response);
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<ActionResult> Logout(CancellationToken cancellationToken)
    {
        if (!Request.Cookies.TryGetValue(_refreshTokenOptions.Name, out var refreshToken))
            return Unauthorized();

        await _mediator.Send(new LogoutCommand(refreshToken), cancellationToken);
        DeleteRefreshTokenCookie();

        return NoContent();
    }

    // TODO: Move to UserController
    [Authorize]
    [HttpGet("/api/profile")]
    public async Task<ActionResult<UserDto>> Profile(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetCurrentUserQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<RefreshResponse>> Refresh(CancellationToken cancellationToken)
    {
        if (!Request.Cookies.TryGetValue(_refreshTokenOptions.Name, out var refreshToken))
            return Unauthorized();

        var result = await _mediator.Send(new RefreshTokenCommand(refreshToken), cancellationToken);
        AppendRefreshTokenCookie(result.RefreshToken);

        var response = new RefreshResponse(
            result.AccessToken.Token,
            result.AccessToken.ExpiresAt,
            result.FirstName,
            result.LastName
        );
        return Ok(response);
    }

    private void AppendRefreshTokenCookie(TokenResult result)
    {
        Response.Cookies.Append(
            _refreshTokenOptions.Name,
            result.Token,
            new CookieOptions
            {
                HttpOnly = _refreshTokenOptions.HttpOnly,
                Secure = _refreshTokenOptions.Secure,
                SameSite = Enum.Parse<SameSiteMode>(_refreshTokenOptions.SameSite),
                Expires = result.ExpiresAt,
                Path = _refreshTokenOptions.Path,
            }
        );
    }

    private void DeleteRefreshTokenCookie()
    {
        Response.Cookies.Delete(
            _refreshTokenOptions.Name,
            new CookieOptions { Path = _refreshTokenOptions.Path }
        );
    }
}

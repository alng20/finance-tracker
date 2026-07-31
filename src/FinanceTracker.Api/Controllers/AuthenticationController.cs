using FinanceTracker.Application.Queries.GetCurrentUser;
using FinanceTracker.Application.Users.Authentication.Login;
using FinanceTracker.Application.Users.Authentication.Refresh;
using FinanceTracker.Application.Users.Authentication.Register;
using FinanceTracker.Application.Users.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.Api.Controllers;

[ApiController]
[Route("api/users/authentication")]
public class AuthenticationController(IMediator mediator, ILogger<AuthenticationController> logger)
    : ControllerBase
{
    private readonly IMediator _mediator = mediator;
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
    public async Task<ActionResult<LoginResultDto>> Login(
        [FromBody] LoginCommand cmd,
        CancellationToken cancellationToken
    )
    {
        var result = await _mediator.Send(cmd, cancellationToken);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("profile")]
    public async Task<ActionResult<UserDto>> Profile(CancellationToken cancellationToken)
    {
        foreach (var claim in HttpContext.User.Claims)
        {
            _logger.LogInformation($"{claim.Type} = {claim.Value}");
        }

        var result = await _mediator.Send(new GetCurrentUserQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<RefreshTokenResultDto>> Refresh(
        RefreshTokenCommand cmd,
        CancellationToken cancellationToken
    )
    {
        var result = await _mediator.Send(cmd, cancellationToken);
        return Ok(result);
    }
}

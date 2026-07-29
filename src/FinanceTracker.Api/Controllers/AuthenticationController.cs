using FinanceTracker.Application.Queries.GetCurrentUser;
using FinanceTracker.Application.Users.Authentication.Login;
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
        var item = await _mediator.Send(cmd, cancellationToken);
        return Ok(item);
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResultDto>> Login(
        [FromBody] LoginCommand cmd,
        CancellationToken cancellationToken
    )
    {
        var item = await _mediator.Send(cmd, cancellationToken);
        return Ok(item);
    }

    [Authorize]
    [HttpGet("profile")]
    public async Task<ActionResult<UserDto>> Profile(CancellationToken cancellationToken)
    {
        foreach (var claim in HttpContext.User.Claims)
        {
            _logger.LogInformation($"{claim.Type} = {claim.Value}");
        }
        var user = await _mediator.Send(new GetCurrentUserQuery(), cancellationToken);

        return Ok(user);
    }
}

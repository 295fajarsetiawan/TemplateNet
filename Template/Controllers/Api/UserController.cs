using System.Net;
using System.Security.Claims;
using AutoMapper;
using Core;
using Core.DTO;
using Data.Entities;
using DataServices.Service.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Template.Config;

namespace Template.Controllers.Api;
[Route("api/user")]
public class UserController : ApiBaseController
{
    private readonly IUserDataService _userDataService;
    private readonly IdentityConfig _identityConfig;

    public UserController(
        IMapper mapper,
        IConfiguration configuration,
        IUserDataService userDataService,
        IdentityConfig identityConfig
    ) : base(mapper, configuration)
    {
        _userDataService = userDataService;
        _identityConfig = identityConfig;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<TokenResponseDto, IEnumerable<ResponseError>>>> Login(
        [FromBody] LoginDto model,
        [FromServices] IValidator<LoginDto> validator
    )
    {
        var resultValidator = await validator.ValidateAsync(model);
        if (!resultValidator.IsValid)
        {
            return ValidatorResponse<TokenResponseDto>(null, "Validation Error", resultValidator);
        }

        var findUsername = await _userDataService.FindUsername(model.Username);
        if (!findUsername.Result)
        {
            return ResponseApi<TokenResponseDto, IEnumerable<ResponseError>>(null, findUsername.ErrorCode,
                findUsername.Information, new List<ResponseError>()
                {
                    new ResponseError()
                    {
                        Type = "username",
                        Description = findUsername.Information
                    }
                });
        }

        var password = Password.VerifyPassword(model.Password, findUsername.Data.Password);
        if (!password)
        {
            return ResponseApi<TokenResponseDto, IEnumerable<ResponseError>>(null, (int)HttpStatusCode.NotFound,
                "Wrong Password", new List<ResponseError>()
                {
                    new ResponseError()
                    {
                        Type = "password",
                        Description = "Wrong Password"
                    }
                });
        }



        var token = _identityConfig.GenerateToken(findUsername.Data);


        return ResponseApi<TokenResponseDto, IEnumerable<ResponseError>>(token, (int)HttpStatusCode.OK, "success",
            null);
    }

    [HttpGet("profile")]
    public async Task<ActionResult<ApiResponse<User, IEnumerable<ResponseError>>>> Profile()
    {
        var claimUsername = GetClaim(ClaimTypes.Name);
        var result = await _userDataService.FindUsername(claimUsername);
        if (!result.Result)
        {
            return ResponseApi<User, IEnumerable<ResponseError>>(null, result.ErrorCode,
                result.Information, new List<ResponseError>()
                {
                    new ResponseError()
                    {
                        Type = "user",
                        Description = result.Information
                    }
                });
        }

        return ResponseApi<User, IEnumerable<ResponseError>>(result.Data, (int)HttpStatusCode.OK, "success", null);
    }
}
using System.Net;
using AutoMapper;
using Core.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FluentValidation.Results;

namespace Template.Controllers;

[Authorize]
[Route("api/[controller]")]
public class ApiBaseController : Controller
{
    
    protected readonly IConfiguration _configuration;
    protected readonly IMapper _mapper;

    public ApiBaseController(
        IMapper mapper,
        IConfiguration configuration
    )
    {
        _mapper = mapper;
        _configuration = configuration;
    }

    protected string GetClaim(string key)
    {
        var roleClaim = User.Claims.FirstOrDefault(c => c.Type == key);
        if (roleClaim != null)
        {
            string result = roleClaim.Value;
            return result;
        }
        return "";
    }

    protected ActionResult<ApiResponse<T, E>> ResponseApi<T, E>(T? data, int statusCode,
        string? message = null, E? error = default)
    {
        
        if (error == null 
            && (typeof(E) == typeof(ResponseError[]) 
                || typeof(IEnumerable<ResponseError>).IsAssignableFrom(typeof(E))) 
            && statusCode >= 400 && statusCode < 600)
        {
            error = (E)(object)new ResponseError[]
            {
                new ResponseError
                {
                    Type = "Error",
                    Description = message ?? "An error occurred"
                }
            };
        }
        
        var response = new ApiResponse<T, E>
        {
            Data = data,
            Message = message,
            Errors = error,
            StatusCode = statusCode,
        };

        return StatusCode((int)statusCode, response);
    }

    protected ActionResult<ApiResponse<T, IEnumerable<ResponseError>>> ValidatorResponse<T>(T? data,
        string? message = null, ValidationResult? error = default)
    {
        var errors = error.Errors.Select(error => new ResponseError
        {
            Type = error.PropertyName,
            Description = error.ErrorMessage
        });

        var response = new ApiResponse<T, IEnumerable<ResponseError>>
        {
            Data = data,
            Message = message,
            Errors = errors,
            StatusCode = (int)HttpStatusCode.BadRequest,
        };

        return StatusCode((int)HttpStatusCode.BadRequest, response);
    }
}
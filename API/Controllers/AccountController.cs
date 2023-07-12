using API.Models;
using API.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using API.Services;
using API.DTOs.University;
using API.Utilities.Handler;
using System.Net;
using API.DTOs.Account;
using API.DTOs.Auth;
using API.Utilities.Enum;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
//[Authorize(Roles =$"{nameof(RoleLevel.Admin)}")]
[AllowAnonymous]
public class AccountController : ControllerBase
{
    private readonly AccountService _accountService;

    public AccountController(AccountService service)
    {
        _accountService = service;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var entities = _accountService.GetAccount();

        if (entities is null || !entities.Any())
        {
            return NotFound(new ResponseHandler<GetAccountDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data not found"
            });
        }

        return Ok(new ResponseHandler<IEnumerable<GetAccountDto>>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data Found",
            Data = entities
        });
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var entity = _accountService.GetAccount(guid);
        if (entity is null)
        {
            return NotFound(new ResponseHandler<GetAccountDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data not found"
            });
        }

        return Ok(new ResponseHandler<GetAccountDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data Found",
            Data = entity
        });
    }

    [AllowAnonymous]
    [HttpPost]
    public IActionResult Create(NewAccountDto account)
    {
        var created = _accountService.CreateAccount(account);
        if (created is null)
        {
            return BadRequest(new ResponseHandler<GetAccountDto>
            {
                Code = StatusCodes.Status400BadRequest,
                Status = HttpStatusCode.BadRequest.ToString(),
                Message = "Data not Created"
            });
        }
        return Ok(new ResponseHandler<GetAccountDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Successfully created",
            Data = created
        });
    }

    [AllowAnonymous]
    [HttpPost("Login")]
    public IActionResult Login(LoginDto loginDto) 
    {
        var loginResult = _accountService.LoginAccount(loginDto);
        switch(loginResult)
        {
            case "0":
                return NotFound(new ResponseHandler<UpdateAccountDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Account Not Found"
                });
                break;
            case "-1":
                return NotFound(new ResponseHandler<UpdateAccountDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Password Incorrect"
                });
                break;
            case "-2":
                return NotFound(new ResponseHandler<UpdateAccountDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Error When creating Token"
                });
                break;
        }
        return Ok(new ResponseHandler<string>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Successfully updated",
            Data = loginResult
        });

    }

    [AllowAnonymous]
    [HttpPost("Register")]
    public IActionResult Register(NewRegisterDto account)
    {
        var created = _accountService.Register(account);
        if (created is null)
        {
            return BadRequest(new ResponseHandler<GetRegisterDto>
            {
                Code = StatusCodes.Status400BadRequest,
                Status = HttpStatusCode.BadRequest.ToString(),
                Message = "Data not Created"
            });
        }
        return Ok(new ResponseHandler<GetRegisterDto>{
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Successfully created",
            Data = created
        });
    }


    [HttpPut]
    public IActionResult Update(UpdateAccountDto account)
    {
        var isUpdated = _accountService.UpdateAccount(account);
        if (isUpdated is -1)
        {
            return NotFound(new ResponseHandler<UpdateAccountDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data not found"
            });
        }
        if (isUpdated is 0)
        {
            return BadRequest(new ResponseHandler<UpdateAccountDto>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Check your data"
            });
        }
        return Ok(new ResponseHandler<UpdateAccountDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Successfully updated"
        });
    }

    [HttpPut("changePassword")]
    public IActionResult UpdatePassword(ChangePasswordDto changePassword)
    {
        var update = _accountService.ChangePassword(changePassword);

        switch(update)
        {
            case -1:
                return NotFound(new ResponseHandler<ChangePasswordDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Email not Found"
                });
                break;
            case 0:
                return NotFound(new ResponseHandler<ChangePasswordDto>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "OTP Doesn't match"
                });
                break;
            case 1:
                return NotFound(new ResponseHandler<ChangePasswordDto> { 
                    Code = StatusCodes.Status404NotFound, 
                    Status = HttpStatusCode.NotFound.ToString(), 
                    Message = "Email not Found" });
                break;
            case 2: 
                return NotFound(new ResponseHandler<ChangePasswordDto> { 
                    Code = StatusCodes.Status404NotFound, 
                    Status = HttpStatusCode.NotFound.ToString(), 
                    Message = "Email not Found" });
                break;
        }

        return Ok(new ResponseHandler<ChangePasswordDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Password Changes Sucessfully"
        });

    }

    [Authorize(Roles = $"{nameof(RoleLevel.User)}")]
    [HttpPost("ForgotPassword")]
    public IActionResult ForgotPassword(ForgotPasswordDto forgotPassword)
    {
        var isUpdated = _accountService.ForgotPassword(forgotPassword);
        if (isUpdated == 0)
            return NotFound(new ResponseHandler<GetAccountDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Email not found"
            });

        if (isUpdated is -1)
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHandler<GetAccountDto>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Error retrieving data from the database"
            });

        return Ok(new ResponseHandler<GetAccountDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Otp has been sent to your email"
        });
    }


    [HttpDelete]
    public IActionResult Delete(Guid guid)
    {
        var delete = _accountService.DeleteAccount(guid);

        if (delete is -1)
        {
            return NotFound(new ResponseHandler<GetAccountDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Id not found"
            });
        }
        if (delete is 0)
        {
            return BadRequest(new ResponseHandler<GetAccountDto>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Check connection to database"
            });
        }

        return Ok(new ResponseHandler<GetAccountDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Successfully deleted"
        });
    }

}

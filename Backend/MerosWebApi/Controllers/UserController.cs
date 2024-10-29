using MerosWebApi.Application.Common.DTOs.UserService;
using MerosWebApi.Application.Common.Exceptions;
using MerosWebApi.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver.Core.WireProtocol.Messages;
using System.Net;

namespace MerosWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult> RegisterAsync([FromBody] RegisterReqDto dto)
        {
            try
            {
                var user = await _userService.RegisterAsync(dto);
                return Ok();
                //return CreatedAtAction(nameof(GetDetailsAsync), new {id = user.Id}, user);
            }
            catch (EmailNotSentException ex)
            {
                return StatusCode((int)HttpStatusCode.BadGateway, new { Message = ex.Message });
            }
            catch(AppException ex)
            {
                return BadRequest(new  { Message = ex.Message });
            }
        }
    }
}

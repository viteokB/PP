using MerosWebApi.Application.Common.DTOs.UserService;
using MerosWebApi.Application.Common.Exceptions;
using MerosWebApi.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver.Core.WireProtocol.Messages;
using System.Net;
using MerosWebApi.Application.Common.SecurityHelpers;
using Microsoft.AspNetCore.Http.HttpResults;

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
        [HttpPost("authenticate")]
        public async Task<ActionResult<AuthenticationResDto>> AuthenticateAsync(
            [FromBody] AuthenticateReqDto reqDto)
        {
            try
            {
                var authecateResult = await _userService.AuthenticateAsync(reqDto);
                SetRefreshTokenToCookie(authecateResult.Item2);
                Response.Cookies.Append("tasty", authecateResult.Item1.Token);

                return Ok(authecateResult.Item1);
            }
            catch (AppException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<GetDetailsResDto>> GetDetailsAsync(Guid id)
        {
            try
            {
                return Ok(await _userService.GetDetailsAsync(id));
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (AppException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
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

        [AllowAnonymous]
        [HttpGet("confirm-email")]
        public async Task<ActionResult> ConfirmEmailAsync(string code)
        {
            try
            {
                await _userService.ConfirmEmailAsync(code);
                return NoContent();
            }
            catch (AppException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        #region Private Helpers Methods

        private void SetRefreshTokenToCookie(RefreshToken token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = token.Expires
            };

            Response.Cookies.Append("refreshToken", token.Token, cookieOptions);
        }

        #endregion
    }
}

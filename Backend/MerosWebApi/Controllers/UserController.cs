using MerosWebApi.Application.Common.DTOs.UserService;
using MerosWebApi.Application.Common.Exceptions;
using MerosWebApi.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver.Core.WireProtocol.Messages;
using System.Net;
using MerosWebApi.Application.Common.SecurityHelpers;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Web;

namespace MerosWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        private readonly IAuthHelper _authHelper;

        public UserController(IUserService userService, IAuthHelper authHelper)
        {
            _userService = userService;

            _authHelper = authHelper;
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
        [HttpGet("refresh-token")]
        public async Task<ActionResult> RefreshToken(string token)
        {
            try
            {
                var newToken = await _userService.RefreshAccessToken(token);
                Response.Cookies.Append("tasty", newToken);

                return Ok(newToken);
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
            catch (AppException ex)
            {
                return BadRequest(new { Message = ex.Message });
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

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            try
            {
                var result = await _userService.DeleteAsync(id, _authHelper.GetUserId(this));
                if (result == true)
                    return Ok("User deleted");

                return NotFound("User not found.");
            }
            catch (ForbiddenException ex)
            {
                return StatusCode((int)HttpStatusCode.Forbidden, new { Message = ex.Message });
            }
        }

        [Authorize]
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdateReqDto dto)
        {
            try
            {
                var user = await _userService.UpdateAsync(id, _authHelper.GetUserId(this), dto);
                return Ok();
            }
            catch (ForbiddenException ex)
            {
                return StatusCode((int)HttpStatusCode.Forbidden, new { Message = ex.Message });
            }
            catch (EmailNotSentException ex)
            {
                return StatusCode((int)HttpStatusCode.BadGateway, new { Message = ex.Message });
            }
            catch (AppException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("password-reset")]
        public async Task<ActionResult> PasswordResetAsync([FromBody] PasswordResetDto dto)
        {
            try
            {
                await _userService.PasswordResetAsync(dto);
                return NoContent();
            }
            catch (EmailNotSentException ex)
            {
                return StatusCode((int)HttpStatusCode.BadGateway, new { Message = ex.Message });
            }
            catch (AppException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet("confirm-password-reset")]
        public async Task<ActionResult> ConfirmPasswordResetAsync([FromQuery] string code, [FromQuery] string email)
        {
            try
            {
                return Ok(await _userService.ConfirmResetPasswordAsync(code, HttpUtility.UrlDecode(email)));
            }
            catch (AppException ex)
            {
                return BadRequest(new  { Message = ex.Message });
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

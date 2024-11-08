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

        /// <summary>
        /// Authenticates the user
        /// </summary>
        /// <param name="reqDto">The request data</param>
        /// <returns>The result containing user info and authorization token, if authentication was successful
        /// </returns>
        [AllowAnonymous]
        [HttpPost("authenticate")]
        [ActionName(nameof(AuthenticateAsync))]
        public async Task<ActionResult<AuthenticationResDto>> AuthenticateAsync(
            [FromBody] AuthenticateReqDto reqDto)
        {
            try
            {
                var authecateResult = await _userService.AuthenticateAsync(reqDto);

                return Ok(authecateResult);
            }
            catch (AppException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        [ActionName(nameof(GetDetailsAsync))]
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
        [ActionName(nameof(RefreshToken))]
        public async Task<ActionResult> RefreshToken(string token)
        {
            try
            {
                var newToken = await _userService.RefreshAccessToken(token);

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
        [ActionName(nameof(RegisterAsync))]
        public async Task<ActionResult> RegisterAsync([FromBody] RegisterReqDto dto)
        {
            try
            {
                var user = await _userService.RegisterAsync(dto);

                return CreatedAtAction(nameof(GetDetailsAsync), new { id = user.Id }, user);
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
        [ActionName(nameof(ConfirmEmailAsync))]
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
        [ActionName(nameof(DeleteAsync))]
        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            try
            {
                var result = await _userService.DeleteAsync(id, _authHelper.GetUserId(this));
                if (result == true)
                    return NoContent();

                return NotFound("User not found.");
            }
            catch (ForbiddenException ex)
            {
                return StatusCode((int)HttpStatusCode.Forbidden, new { Message = ex.Message });
            }
        }

        [Authorize]
        [HttpPatch("{id}")]
        [ActionName(nameof(UpdateAsync))]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdateReqDto dto)
        {
            try
            {
                var user = await _userService.UpdateAsync(id, _authHelper.GetUserId(this), dto);
                return CreatedAtAction(nameof(GetDetailsAsync), new {id = user.Id}, user);
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
        [ActionName(nameof(PasswordResetAsync))]
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
        [ActionName(nameof(ConfirmPasswordResetAsync))]
        public async Task<ActionResult> ConfirmPasswordResetAsync([FromQuery] ConfirmResetPasswordQuery query)
        {
            try
            {
                return Ok(await _userService.ConfirmResetPasswordAsync(query.Code,
                    HttpUtility.UrlDecode(query.Email)));
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

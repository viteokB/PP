using Asp.Versioning;
using MerosWebApi.Application.Common.DTOs;
using MerosWebApi.Application.Common.DTOs.MeroService;
using MerosWebApi.Application.Common.DTOs.UserService;
using MerosWebApi.Application.Common.Exceptions;
using MerosWebApi.Application.Interfaces;
using MerosWebApi.Core.Models.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MerosWebApi.Controllers.V1
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    [Authorize]
    public class MeroController : ControllerBase
    {
        private readonly IMeroService _meroService;

        private readonly IAuthHelper _authHelper;

        public MeroController(IMeroService meroService, IAuthHelper authHelper)
        {
            _meroService = meroService;

            _authHelper = authHelper;
        }

        /// <summary>
        /// Creates a new event profile with the specified fields and time periods
        /// </summary>
        /// <param name="meroReqDto">The request data</param>
        /// <returns></returns>
        [HttpPost]
        [ActionName(nameof(CreateMeroAsync))]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MeroResDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(MeroValidationErrorDto), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> CreateMeroAsync(MeroReqDto meroReqDto)
        {
            try
            {
                var creatorId = _authHelper.GetUserId(this);
                var meroResDto = await _meroService.CreateNewMeroAsync(creatorId, meroReqDto);

                return CreatedAtAction(nameof(GetMeroDetailsAsync), new {id = meroResDto.Id}, meroResDto);
            }
            catch (MeroTimeException timeException)
            {
                return BadRequest(new MeroValidationErrorDto(timeException.TimePeriodsReqDto, timeException.Message));
            }
            catch (MeroFieldException fieldTypeException)
            {
                return BadRequest(new MeroValidationErrorDto(fieldTypeException.FieldReqDto, fieldTypeException.Message));
            }
            catch (AppException ex)
            {
                return StatusCode((int)HttpStatusCode.BadGateway, new MeroValidationErrorDto(null, ex.Message));
            }
        }

        /// <summary>
        /// Receives the event questionnaire by the specified id
        /// </summary>
        /// <param name="meroId">Event id</param>
        /// <returns></returns>
        [HttpGet]
        [ActionName(nameof(GetMeroDetailsAsync))]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MeroResDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(MyResponseMessage), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<MeroResDto>> GetMeroDetailsAsync(string meroId)
        {
            try
            {
                var meroDto = await _meroService.GetMeroByIdAsync(meroId);

                return Ok(meroDto);
            }
            catch (MeroNotFoundException ex)
            {
                return NotFound(new MyResponseMessage{Message = ex.Message});
            }
        }
    }
}

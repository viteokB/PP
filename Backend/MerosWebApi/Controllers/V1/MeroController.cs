﻿using Asp.Versioning;
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
        [ProducesResponseType(typeof(MyResponseMessage), (int)HttpStatusCode.BadGateway)]
        public async Task<ActionResult> CreateMeroAsync([FromBody] MeroReqDto meroReqDto)
        {
            try
            {
                var creatorId = _authHelper.GetUserId(this);
                var meroResDto = await _meroService.CreateNewMeroAsync(creatorId, meroReqDto);

                return CreatedAtAction(nameof(GetMeroDetailsAsync), new {meroId = meroResDto.Id}, meroResDto);
            }
            catch (AppException ex)
            {
                return StatusCode((int)HttpStatusCode.BadGateway, new MeroValidationErrorDto(null, ex.Message));
            }
        }

        /// <summary>
        /// Completely updates the event data with the current ID
        /// </summary>
        /// <param name="meroReqDto">The request data</param>
        /// <returns></returns>
        [HttpPost("update/{meroId}")]
        [ActionName(nameof(UpdateMeroAsync))]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MeroResDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(MeroValidationErrorDto), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(MyResponseMessage), (int)HttpStatusCode.BadGateway)]
        public async Task<ActionResult> UpdateMeroAsync(string meroId, [FromBody] MeroReqDto meroReqDto)
        {
            try
            {
                var creatorId = _authHelper.GetUserId(this);
                var updateMero = await _meroService.FullMeroUpdateAsync(creatorId, meroId, meroReqDto);

                return CreatedAtAction(nameof(GetMeroDetailsAsync), new { meroId = updateMero.Id }, updateMero);
            }
            catch (ForbiddenException ex)
            {
                return StatusCode((int)HttpStatusCode.Forbidden,
                    new MyResponseMessage(ex.Message) );
            }
            catch (MeroNotFoundException ex)
            {
                return NotFound(new MyResponseMessage(ex.Message));
            }
            catch (NotPossibleUpdateException ex)
            {
                return BadRequest(new MeroValidationErrorDto(null, ex.Message));
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
        [HttpGet("by-id/{meroId}")]
        [ActionName(nameof(GetMeroDetailsAsync))]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MeroResDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(MyResponseMessage), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(MyResponseMessage), (int)HttpStatusCode.BadGateway)]
        public async Task<ActionResult<MeroResDto>> GetMeroDetailsAsync(string meroId)
        {
            try
            {
                var meroDto = await _meroService.GetMeroByIdAsync(meroId);
                return Ok(meroDto);
            }
            catch (MeroNotFoundException ex)
            {
                return NotFound(new MyResponseMessage(ex.Message));
            }
            catch (AppException ex)
            {
                return StatusCode((int)HttpStatusCode.BadGateway,
                    new MeroValidationErrorDto(null, ex.Message));
            }
        }

        /// <summary>
        /// Receives the event questionnaire by the specified unique invite code
        /// </summary>
        /// <param name="inviteCode">Unique invite code</param>
        /// <returns></returns>
        [HttpGet("by-invite-code/{inviteCode}")]
        [ActionName(nameof(GetMeroDetailsByInviteCodeAsync))]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MeroResDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(MyResponseMessage), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(MyResponseMessage), (int)HttpStatusCode.BadGateway)]
        public async Task<ActionResult<MeroResDto>> GetMeroDetailsByInviteCodeAsync(string inviteCode)
        {
            try
            {
                var meroDto = await _meroService.GetMeroByInviteCodeAsync(inviteCode);
                return Ok(meroDto);
            }
            catch (MeroNotFoundException ex)
            {
                return NotFound(new MyResponseMessage(ex.Message));
            }
            catch (AppException ex)
            {
                return StatusCode((int)HttpStatusCode.BadGateway,
                    new MeroValidationErrorDto(null, ex.Message));
            }
        }

        /// <summary>
        /// Deletes the event with the passed _id and all its associated data
        /// </summary>
        /// <param name="meroId">Event id</param>
        /// <returns></returns>
        [HttpDelete("{meroId}")]
        [ActionName(nameof(DeleteMeroAsync))]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(MyResponseMessage), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(MyResponseMessage), (int)HttpStatusCode.UnprocessableEntity)]
        [ProducesResponseType(typeof(MyResponseMessage), (int)HttpStatusCode.BadGateway)]
        public async Task<ActionResult<MeroResDto>> DeleteMeroAsync(string meroId)
        {
            try
            {
                var userId = _authHelper.GetUserId(this);
                var querryResult = await _meroService.DelereMeroByIdAsync(userId, meroId);

                if (querryResult.IsSuccess)
                    return NoContent();

                return UnprocessableEntity(new MyResponseMessage(querryResult.Message));
            }
            catch (MeroNotFoundException ex)
            {
                return NotFound(new MyResponseMessage(ex.Message));
            }
            catch (ForbiddenException ex)
            {
                return StatusCode((int)HttpStatusCode.Forbidden,
                    new MyResponseMessage(ex.Message));
            }
            catch (AppException ex)
            {
                return StatusCode((int)HttpStatusCode.BadGateway, 
                    new MeroValidationErrorDto(null, ex.Message));
            }
        }

        /// <summary>
        /// Creates a new form of the questionnaire-the response of the user's entry to the event
        /// </summary>
        /// <param name="phormAnswerReqDto">Completed questionnaire response form</param>
        /// <returns></returns>
        [HttpPost("phorm-answer/create")]
        [ActionName(nameof(CreatePhormAnswerAsync))]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PhormAnswerResDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(MyResponseMessage), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(MyResponseMessage), (int)HttpStatusCode.BadGateway)]
        public async Task<ActionResult> CreatePhormAnswerAsync([FromBody] PhormAnswerReqDto phormAnswerReqDto)
        {
            try
            {
                var creatorId = _authHelper.GetUserId(this);
                var phormAnswerResDto = await _meroService.CreateNewPhormAnswerAsync(creatorId, phormAnswerReqDto);

                return CreatedAtAction(nameof(GetPhormAnswerDetails),
                    new { phormId = phormAnswerResDto.Id }, phormAnswerResDto);
            }
            catch (AppException ex)
            {
                return BadRequest(new MeroValidationErrorDto(null, ex.Message));
            }
            catch (CoreException ex)
            {
                return BadRequest(new MeroValidationErrorDto(null, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.BadGateway, new MeroValidationErrorDto(null, ex.Message));
            }
        }

        [HttpGet("phorm-answer/get-one/{phormId}")]
        [ActionName(nameof(GetPhormAnswerDetails))]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MeroResDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(MyResponseMessage), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(MyResponseMessage), (int)HttpStatusCode.BadGateway)]
        public async Task<ActionResult<PhormAnswerResDto>> GetPhormAnswerDetails(string phormId)
        {
            try
            {
                var phormAnswerResDto = await _meroService.GetMeroPhormAnswerByIdAsync(phormId);
                return Ok(phormAnswerResDto);
            }
            catch (MeroNotFoundException ex)
            {
                return NotFound(new MyResponseMessage(ex.Message));
            }
            catch (AppException ex)
            {
                return StatusCode((int)HttpStatusCode.BadGateway,
                    new MeroValidationErrorDto(null, ex.Message));
            }
        }

        [HttpGet("phorm-answer/get-list-by-mero")]
        [ActionName(nameof(GetListMeroPhormsAnswersForMero))]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MeroResDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(MyResponseMessage), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(MyResponseMessage), (int)HttpStatusCode.BadGateway)]
        public async Task<ActionResult<List<ShowWritenPhromResDto>>> 
            GetListMeroPhormsAnswersForMero(int startIndex, int count, string meroId)
        {
            try
            {
                var phormAnswerResDtos = await _meroService
                    .GetMeroPhormsListByMeroAsync(startIndex, count, meroId);

                return Ok(phormAnswerResDtos);
            }
            catch (AppException ex)
            {
                return StatusCode((int)HttpStatusCode.BadGateway,
                    new MeroValidationErrorDto(null, ex.Message));
            }
        }

        [HttpGet("list-meros/for-user")]
        [ActionName(nameof(GetListMyMerosForUser))]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MyMeroResDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(MyResponseMessage), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(MyResponseMessage), (int)HttpStatusCode.BadGateway)]
        public async Task<ActionResult<List<MyMeroResDto>>>
            GetListMyMerosForUser(int startIndex, int count, string userId)
        {
            try
            {
                var myMeroList = await _meroService
                    .GetListMyMeroListForUser(startIndex, count, userId);

                return Ok(myMeroList);
            }
            catch (AppException ex)
            {
                return StatusCode((int)HttpStatusCode.BadGateway,
                    new MeroValidationErrorDto(null, ex.Message));
            }
        }

        [HttpGet("list-meros/for-creator")]
        [ActionName(nameof(GetListMyMerosForCreator))]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MyMeroResDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(MyResponseMessage), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(MyResponseMessage), (int)HttpStatusCode.BadGateway)]
        public async Task<ActionResult<List<MyMeroResDto>>>
            GetListMyMerosForCreator(int startIndex, int count, string userId)
        {
            try
            {
                var myMeroList = await _meroService
                    .GetListMyMeroListForCreator(startIndex, count, userId);

                return Ok(myMeroList);
            }
            catch (AppException ex)
            {
                return StatusCode((int)HttpStatusCode.BadGateway,
                    new MeroValidationErrorDto(null, ex.Message));
            }
        }
    }
}

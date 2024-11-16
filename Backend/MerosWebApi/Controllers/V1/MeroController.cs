using Asp.Versioning;
using MerosWebApi.Application.Common.DTOs.MeroService;
using MerosWebApi.Application.Common.DTOs.UserService;
using MerosWebApi.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost]
        public async Task<ActionResult> CreateMeroAsync(MeroReqDto meroReqDto)
        {
            try
            {
                var creatorId = _authHelper.GetUserId(this);
                await _meroService.CreateNewMeroAsync(creatorId, meroReqDto);

                return Created();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [HttpGet]
        public async Task<ActionResult<MeroResDto>> GetMeroDetailsAsync(string meroId)
        {
            var meroDto = await _meroService.GetMeroByIdAsync(meroId);

            return Ok(meroDto);
        }
    }
}

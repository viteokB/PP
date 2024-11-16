using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MerosWebApi.Application.Common.DTOs.MeroService;
using MerosWebApi.Core.Models.Mero;

namespace MerosWebApi.Application.Interfaces
{
    public interface IMeroService
    {
        public Task<MeroResDto> GetMeroByIdAsync(string id);

        public Task<MeroResDto> CreateNewMeroAsync(string creatorId, MeroReqDto createResDto);

        public Task UpdateMeroAsync(Mero mero);

    }
}

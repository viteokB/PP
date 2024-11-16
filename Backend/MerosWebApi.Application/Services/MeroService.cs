using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MerosWebApi.Application.Common.DTOs.MeroService;
using MerosWebApi.Application.Interfaces;
using MerosWebApi.Core.Models;
using MerosWebApi.Core.Models.Mero;
using MerosWebApi.Core.Models.QuestionFields;
using MerosWebApi.Core.Repository;
using MongoDB.Bson;

namespace MerosWebApi.Application.Services
{
    public class MeroService : IMeroService
    {
        private readonly IMeroRepository _repository;
        public MeroService(IMeroRepository repository)
        {
            _repository = repository;
        }

        public async Task<MeroResDto> GetMeroByIdAsync(string id)
        {
            var mero = await _repository.GetMeroByIdAsync(id);

            return MeroResDto.Map(mero);
        }

        public async Task CreateNewMeroAsync(string creatorId, MeroReqDto createReqDto)
        {
            var meroId = ObjectId.GenerateNewId().ToString();

            var timePeriods = new List<TimePeriod>();

            foreach (var periodDto in createReqDto.Periods)
            {
                var periodId = ObjectId.GenerateNewId().ToString();

                var timePeriod = TimePeriod.CreateTimePeriod(periodId, periodDto.StartTime, periodDto.EndTime,
                    periodDto.TotalPlaces, 0);

                _repository.AddTimePeriodAsync(timePeriod);
                timePeriods.Add(timePeriod);
            }

            var fields = new List<Field>();

            foreach (var fieldReqDto in createReqDto.Fields)
            {
                var field = FieldFactoryMethod.CreateField(fieldReqDto.Text, fieldReqDto.Type, fieldReqDto.Required,
                    fieldReqDto.Answers);

                fields.Add(field);
            }

            var mero = Mero.CreateMero(meroId, createReqDto.Name, creatorId, createReqDto.CreatorEmail,
                createReqDto.Description, timePeriods, fields, null);

            await _repository.AddMeroAsync(mero);
        }

        public Task UpdateMeroAsync(Mero mero)
        {
            throw new NotImplementedException();
        }
    }
}

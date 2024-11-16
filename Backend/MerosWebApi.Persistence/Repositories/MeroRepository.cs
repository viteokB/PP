using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MerosWebApi.Core.Models;
using MerosWebApi.Core.Models.Mero;
using MerosWebApi.Core.Repository;
using MerosWebApi.Persistence.Entites;
using MerosWebApi.Persistence.Helpers;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;

namespace MerosWebApi.Persistence.Repositories
{
    public class MeroRepository : IMeroRepository
    {
        private readonly MongoDbService _dbService;

        public MeroRepository(MongoDbService dbContext)
        {
            _dbService = dbContext;
        }

        public async Task<Mero> GetMeroByIdAsync(string meroId)
        {
            var fitler = Builders<DatabaseMero>.Filter
                .Eq( "_id", new ObjectId(meroId));
            var meros = await _dbService.Meros.FindAsync(fitler);
            var mero = meros.FirstOrDefault();

            if (mero == null)
                return null;

            var timePeriods = GetTimePeriodsAsync(mero.TimePeriods);

            var fields = mero.Fields
                .Select(f => FieldPropertyAssigner.MapFrom(f))
                .ToList();

            return Mero.CreateMero(mero.Id, mero.Name, mero.CreatorId, mero.CreatorEmail,
                mero.Description, await timePeriods, fields, mero.Files);
        }

        public async Task AddMeroAsync(Mero mero)
        {
            var dbMero = MeroPropertyAssigner.MapFrom(mero);

            await _dbService.Meros.InsertOneAsync(dbMero);
        }

        public async Task AddTimePeriodAsync(TimePeriod period)
        {
            var dbTimePeriod = TimePeriodPropertyAssigner.MapFrom(period);

            _dbService.TimePeriods.InsertOneAsync(dbTimePeriod);
        }

        public async Task<List<TimePeriod>> GetTimePeriodsAsync(IEnumerable<string> ids)
        {
            var periodFilter = Builders<DatabaseTimePeriod>.Filter
                .In(doc => doc.Id, ids);
            var dbTimePeridos = await _dbService.TimePeriods
                .FindAsync(periodFilter);

            var timePeriods = dbTimePeridos.ToEnumerable()
                .Select(period => TimePeriodPropertyAssigner.MapFrom(period))
                .ToList();

            return timePeriods;
        }
    }
}

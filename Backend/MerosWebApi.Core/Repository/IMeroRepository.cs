﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MerosWebApi.Core.Models.Mero;
using MerosWebApi.Core.Models.PhormAnswer;

namespace MerosWebApi.Core.Repository
{
    public interface IMeroRepository
    {
        public Task<Mero> GetMeroByIdAsync(string meroId);

        public Task<Mero> GetMeroByInviteCodeAsync(string uniqueInviteCode);

        public Task AddMeroAsync(Mero mero);

        public Task AddTimePeriodAsync(TimePeriod period);

        public Task<List<TimePeriod>> GetTimePeriodsAsync(IEnumerable<string> ids);

        public Task<QuerryStatus> DeleteMeroByIdAsync(string meroId);

        public Task<bool> AddMeroPhormAnswerAsync(PhormAnswer phormAnswer);
    }
}

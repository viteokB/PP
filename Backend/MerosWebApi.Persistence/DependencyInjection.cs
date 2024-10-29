using MerosWebApi.Core.Repository;
using MerosWebApi.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerosWebApi.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection services,
            IConfiguration configuration)
        {
            var mongoSection = configuration.GetSection("MongoDB");

            var connectionString = mongoSection["ConnectionURI"];
            var dbName = mongoSection["DatabaseName"];

            var mongoClient = new MongoClient(connectionString);

            services.AddDbContext<MerosDbContext>(options =>
            {
                options.UseMongoDB(mongoClient, dbName);
            });

            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}

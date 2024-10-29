using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MerosWebApi.Application.Common.EmailSender;
using MerosWebApi.Application.Interfaces;

namespace MerosWebApi.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDevEmailConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {
            var devConfiguration = configuration.GetSection("DevelopmentEmailConfiguration");

            var hostAddress = devConfiguration["Host"];
            int.TryParse(devConfiguration["Port"], out var hostPort);
            var emailAddress = devConfiguration["UserName"];
            var emailPassword = devConfiguration["Password"];

            if(string.IsNullOrWhiteSpace(hostAddress) || hostPort == default(int) ||
                string.IsNullOrWhiteSpace(emailAddress) || string.IsNullOrWhiteSpace(emailPassword))
            {
                throw new ArgumentException($"Не правильно сконфигурирована" +
                    $"секция '{nameof(devConfiguration.Value)}' в 'appsettings.json'");
            }
            

            var emailConfiguration = new DevelopmentConfiguration(emailAddress, emailPassword, hostAddress,
                hostPort);

            services.AddSingleton<DevelopmentConfiguration>(emailConfiguration);

            return services;
        }
    }
}

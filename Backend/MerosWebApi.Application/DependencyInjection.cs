using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MerosWebApi.Application.Common.EmailSender;
using MerosWebApi.Application.Interfaces;
using MerosWebApi.Application.Common;
using MerosWebApi.Application.Services;
using MerosWebApi.Application.Common.SecurityHelpers;
using MerosWebApi.Application.Common.Mapping;
using System.Reflection;

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

            if (string.IsNullOrWhiteSpace(hostAddress) || hostPort == default(int) ||
                string.IsNullOrWhiteSpace(emailAddress) || string.IsNullOrWhiteSpace(emailPassword))
            {
                throw new ArgumentException($"Не правильно сконфигурирована" +
                    $"секция '{nameof(devConfiguration.Value)}' в 'appsettings.json'");
            }


            var emailConfiguration = new DevelopmentConfiguration(emailAddress, emailPassword, hostAddress,
                hostPort);

            services.AddSingleton<IEmailConfiguration>(emailConfiguration);

            return services;
        }

        public static IServiceCollection AddAppSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettingsSection = configuration.GetSection("AppSettings");

            var secret = appSettingsSection["Secret"];
            var name = appSettingsSection["Name"];
            int.TryParse(appSettingsSection["MaxLoginFailedCount"], out var maxLoginFailed);
            int.TryParse(appSettingsSection["LoginFailedWaitingTime"], out var loginFailedWait);
            int.TryParse(appSettingsSection["MaxUnconfirmedEmailCount"], out var maxUnconfEmail);
            int.TryParse(appSettingsSection["UnconfirmedEmailWaitingTime"], out var unconfEmailWait);
            var confEmailUrl = appSettingsSection["ConfirmEmailUrl"];
            int.TryParse(appSettingsSection["MaxResetPasswordCount"], out var maxResetPasswd);
            int.TryParse(appSettingsSection["ResetPasswordWaitingTime"], out var resetPasswdWait);
            int.TryParse(appSettingsSection["ResetPasswordValidTime"], out var resetPasswdValid);
            var resetPasswdUrl = appSettingsSection["ResetPasswordUrl"];

            //В будущем добавить логику проверки полученных значений
            //И/или переделать конфигурирование AppSettings

            var appSettings = new AppSettings
            {
                Secret = secret,
                Name = name,
                MaxLoginFailedCount = maxLoginFailed,
                LoginFailedWaitingTime = loginFailedWait,
                MaxUnconfirmedEmailCount = maxUnconfEmail,
                UnconfirmedEmailWaitingTime = unconfEmailWait,
                ConfirmEmailUrl = confEmailUrl,
                MaxResetPasswordCount = maxResetPasswd,
                ResetPasswordWaitingTime = resetPasswdWait,
                ResetPasswordValidTime = resetPasswdValid,
                ResetPasswordUrl = resetPasswdUrl
            };

            services.AddSingleton<AppSettings>(appSettings);

            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPasswordHelper, PasswordHelper>();
            services.AddScoped<IEmailSender, EmailSender>();

            services.AddAutoMapper(config =>
            {
                config.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
            });


            return services;
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MerosWebApi.Application.Common;
using MerosWebApi.Application.Common.DTOs.UserService;
using MerosWebApi.Application.Common.Exceptions;
using MerosWebApi.Application.Common.SecurityHelpers;
using MerosWebApi.Application.Interfaces;
using MerosWebApi.Core.Models;
using MerosWebApi.Core.Repository;
using Microsoft.Extensions.FileProviders;
using ZstdSharp.Unsafe;

namespace MerosWebApi.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        private readonly IPasswordHelper _passwordHelper;

        private readonly IMapper _mapper;

        private readonly IEmailSender _emailSender;

        private readonly AppSettings _appSettings;

        private readonly EmbeddedFileProvider _embedded;

        public UserService(IUserRepository repository, IPasswordHelper passwordHelper,
            IMapper mapper, IEmailSender emailSender, AppSettings appSettings)
        {
            _repository = repository;
            _passwordHelper = passwordHelper;
            _mapper = mapper;
            _emailSender = emailSender;
            _appSettings = appSettings;
            _embedded = new EmbeddedFileProvider(Assembly.GetExecutingAssembly());
        }

        public async Task<AuthenticationResDto> AuthenticateAsync(AuthenticateReqDto dto)
        {
            throw new NotImplementedException();
        }

        //Did
        public async Task<GetDetailsResDto> RegisterAsync(RegisterReqDto dto)
        {
            if(string.IsNullOrEmpty(dto.Password))
                throw new InvalidPasswordException("Password is required");

            var existingUser = await _repository.GetUserByEmail(dto.Email);

            if (existingUser?.Email == dto.Email)
                throw new EmailTakenException($"Email {dto.Email} us already taken.");

            var (passwordHash, passwordSalt) = _passwordHelper.CreateHash(dto.Password);

            var user = new User
            {
                Full_name = dto.Full_name,
                CreatedAt = DateTime.Now,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                IsActive = false
            };

            var emailSuccess = await ChangeEmailAsync(user, dto.Email);
            //Must confirm Email

            if (!emailSuccess)
                throw new EmailNotSentException("Sending of confirmation email failed.");

            await _repository.AddUser(user);

            return _mapper.Map<User, GetDetailsResDto>(user);
        }

        public async Task<GetDetailsResDto> GetDetailsAsync(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<GetDetailsResDto> UpdateAsync(string email, string userEmail,
            UpdateReqDto dto)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(string email, string userEmail)
        {
            throw new NotImplementedException();
        }

        public async Task ConfirmEmailAsync(string code)
        {
            throw new NotImplementedException();
        }

        public async Task PasswordResetAsync(PasswordResetDto dto)
        {
            throw new NotImplementedException();
        }

        public async Task<ConfirmResetPswdDto> ConfirmResetPasswordAsync(string code,
            string email)
        {
            throw new NotImplementedException();
        }

        #region Private helper methods

        private async Task<bool> ChangeEmailAsync(User user, string newEmail)
        {
            if (newEmail == user.Email) return true;

            var secondsPassed = DateTime.Now.Subtract(
                user.UnconfirmedEmailCreatedAt.GetValueOrDefault()).Seconds;

            var isMaxCountExceeded = user.UnconfirmedEmailCount >= _appSettings.MaxUnconfirmedEmailCount;
            var isWaitingTimePassed = secondsPassed > _appSettings.UnconfirmedEmailWaitingTime;

            if (isMaxCountExceeded && !isWaitingTimePassed)
            {
                var secondsToWait = _appSettings.UnconfirmedEmailWaitingTime - secondsPassed;

                throw new TooManyChangeEmailAttemptsException(
                    string.Format("You must wait for {0} seconds before you try to change email again."),
                    secondsToWait);
            }

            user.UnconfirmedEmail = newEmail;
            user.UnconfirmedEmailCode = _passwordHelper.GenerateRandomString(30) + Guid.NewGuid();
            user.UnconfirmedEmailCount += 1;
            user.UnconfirmedEmailCreatedAt = DateTime.UtcNow;

            // Prepare email template.
            await using var stream = _embedded
                .GetFileInfo($"Resources/EmailTemplates/Email_ConfirmEmail.html")
                .CreateReadStream();
            var emailBody = await new StreamReader(stream).ReadToEndAsync();
            emailBody = emailBody.Replace("{{APP_NAME}}", _appSettings.Name);
            emailBody = emailBody.Replace("{{EMAIL_CONFIRM_URL}}",
                $"{_appSettings.ConfirmEmailUrl}?code={user.UnconfirmedEmailCode}");

            // Send an email.
            return await _emailSender.SendAsync(newEmail, "Confirm your email", emailBody);
        }

        #endregion
    }
}

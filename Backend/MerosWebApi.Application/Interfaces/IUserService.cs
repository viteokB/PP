using MerosWebApi.Application.Common.DTOs.UserService;
using MerosWebApi.Application.Common.SecurityHelpers;

namespace MerosWebApi.Application.Interfaces
{
    public interface IUserService
    {
        public Task<(AuthenticationResDto, RefreshToken)> AuthenticateAsync(AuthenticateReqDto dto);
        public Task ConfirmEmailAsync(string code);
        public Task<ConfirmResetPswdDto> ConfirmResetPasswordAsync(string code, string email);
        public Task DeleteAsync(string email, string userEmail);
        public Task<GetDetailsResDto> GetDetailsAsync(Guid id);
        public Task PasswordResetAsync(PasswordResetDto dto);
        public Task<GetDetailsResDto> RegisterAsync(RegisterReqDto dto);
        public Task<GetDetailsResDto> UpdateAsync(string email, string userEmail, UpdateReqDto dto);
    }
}
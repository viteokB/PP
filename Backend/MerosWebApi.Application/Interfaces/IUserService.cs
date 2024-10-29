using MerosWebApi.Application.Common.DTOs.UserService;

namespace MerosWebApi.Application.Interfaces
{
    public interface IUserService
    {
        public Task<AuthenticationResDto> AuthenticateAsync(AuthenticateReqDto dto);
        public Task ConfirmEmailAsync(string code);
        public Task<ConfirmResetPswdDto> ConfirmResetPasswordAsync(string code, string email);
        public Task DeleteAsync(string email, string userEmail);
        public Task<GetDetailsResDto> GetDetailsAsync(string email);
        public Task PasswordResetAsync(PasswordResetDto dto);
        public Task<GetDetailsResDto> RegisterAsync(RegisterReqDto dto);
        public Task<GetDetailsResDto> UpdateAsync(string email, string userEmail, UpdateReqDto dto);
    }
}
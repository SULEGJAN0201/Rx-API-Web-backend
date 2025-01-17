﻿using Rx.Domain.DTOs.User;
using Rx.Domain.Entities.Identity;
using Rx.Domain.Wrappers;

namespace Rx.Domain.Interfaces.Identity;

public interface IUserService
{
    Task<ResponseMessage<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request);
    Task<ResponseMessage<RegisterResponse>> RegisterAsync(RegisterRequest request, string origin);
    Task<ResponseMessage<string>> ConfirmEmailAsync(string userId, string code);
    Task<string> ForgotPassword(ForgotPasswordRequest model, string origin);
    Task<ResponseMessage<string>> ResetPassword(ResetPasswordRequest model);
    Task<string> AddRoleAsync(AddRoleModel model);
    Task<AuthenticationResponse> RefreshTokenAsync(string token);
    Task<ApplicationUser> GetById(string id);
    bool RevokeToken(string token);
    Task<ResponseMessage<string>> AddUserAsync(AddUserRequest request,string origin);
    Task<ResponseMessage<string>> ChangePasswordAsync(ChangePasswordRequest request);
    Task<string> UpdateUserAsync(string userId, Guid organizationId);
    Task<string> EditUserDetails(string userId, UpdateUserRequest updateUserRequest);
    Task<IEnumerable<UserVm>> GetUsersForOrganization(Guid organizationId);
    Task<string> DeleteUserAsync(string email);
}
﻿using System.Text.Json.Serialization;

namespace Rx.Domain.DTOs.User;

public class AuthenticationResponse
{
    public string Id { get; set; }
    public string Message { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public List<string> Roles { get; set; }
    public bool IsAuthenticated { get; set; }
    public bool IsVerified { get; set; }
    public string JwtToken { get; set; }
    public string? ProfileUrl { get; set; }
    public string? OrganizationId { get; set; }
    [JsonIgnore]
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiration { get; set; }
}
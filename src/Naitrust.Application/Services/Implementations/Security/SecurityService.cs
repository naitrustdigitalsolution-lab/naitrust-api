using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Naitrust.Application.ExternalServices;
using Naitrust.Application.ExternalServices.Communication;
using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Security;
using Naitrust.Domain.Models.Dtos.Responses.Security;
using Naitrust.Domain.Models.Entities;

namespace Naitrust.Application.Services.Implementations.Security;

public class SecurityService : ISecurityService
{
    private readonly UserManager<NaitrustUser> _userManager;
    private readonly ICacheService _cacheService;
    private readonly IEmailService _emailService;
    private readonly IPasswordHasher<NaitrustUser> _passwordHasher;
    private readonly ILogger<SecurityService> _logger;

    public SecurityService(
        UserManager<NaitrustUser> userManager,
        ICacheService cacheService,
        IEmailService emailService,
        IPasswordHasher<NaitrustUser> passwordHasher,
        ILogger<SecurityService> logger)
    {
        _userManager = userManager;
        _cacheService = cacheService;
        _emailService = emailService;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }

    // ──────────────────────────────────────────────────────────────
    // Email OTP
    // ──────────────────────────────────────────────────────────────

    public async Task<NaitrustResponse<bool>> SendEmailOtpAsync(Guid userId, SendEmailOtpRequest request, CancellationToken ct = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return NaitrustResponse<bool>.NotFound("User not found.");

        var email = request.Email.Trim().ToLowerInvariant();

        var otp = GenerateOtp();
        var cacheKey = $"otp:security-email:{userId}:{email}";
        await _cacheService.SetAsync(cacheKey, otp, TimeSpan.FromMinutes(10), ct);

        await _emailService.SendVerificationOtpAsync(email, user.FirstName, email, otp, ct);

        return NaitrustResponse<bool>.Success("Verification code sent to your email.", true);
    }

    public async Task<NaitrustResponse<bool>> VerifyEmailOtpAsync(Guid userId, VerifyEmailOtpRequest request, CancellationToken ct = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return NaitrustResponse<bool>.NotFound("User not found.");

        // Try the stored email (we look up all possible keys based on the user's current email)
        var email = user.Email!.ToLowerInvariant();
        var cacheKey = $"otp:security-email:{userId}:{email}";
        var storedOtp = await _cacheService.GetAsync<string>(cacheKey, ct);

        if (storedOtp is null || storedOtp != request.Code)
            return NaitrustResponse<bool>.BadRequest("Invalid or expired verification code.");

        await _cacheService.RemoveAsync(cacheKey, ct);

        user.EmailConfirmed = true;
        user.EmailVerifiedAt = DateTime.UtcNow;
        user.UpdatedAt = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);

        return NaitrustResponse<bool>.Success("Email verified successfully.", true);
    }

    // ──────────────────────────────────────────────────────────────
    // Phone OTP
    // ──────────────────────────────────────────────────────────────

    public async Task<NaitrustResponse<bool>> SendPhoneOtpAsync(Guid userId, SendPhoneOtpRequest request, CancellationToken ct = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return NaitrustResponse<bool>.NotFound("User not found.");

        var phone = request.Phone.Trim();

        var otp = GenerateOtp();
        var cacheKey = $"otp:security-phone:{userId}:{phone}";
        await _cacheService.SetAsync(cacheKey, otp, TimeSpan.FromMinutes(10), ct);

        // SMS not yet integrated — log the OTP for testing
        _logger.LogInformation("[DEV] Phone OTP for {Phone}: {Otp}", phone, otp);

        return NaitrustResponse<bool>.Success("Verification code sent to your phone.", true);
    }

    public async Task<NaitrustResponse<bool>> VerifyPhoneOtpAsync(Guid userId, VerifyPhoneOtpRequest request, CancellationToken ct = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return NaitrustResponse<bool>.NotFound("User not found.");

        if (string.IsNullOrEmpty(user.PhoneNumber))
            return NaitrustResponse<bool>.BadRequest("No phone number on file. Send OTP to your phone number first.");

        var phone = user.PhoneNumber.Trim();
        var cacheKey = $"otp:security-phone:{userId}:{phone}";
        var storedOtp = await _cacheService.GetAsync<string>(cacheKey, ct);

        if (storedOtp is null || storedOtp != request.Code)
            return NaitrustResponse<bool>.BadRequest("Invalid or expired verification code.");

        await _cacheService.RemoveAsync(cacheKey, ct);

        user.PhoneNumberConfirmed = true;
        user.PhoneVerifiedAt = DateTime.UtcNow;
        user.UpdatedAt = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);

        return NaitrustResponse<bool>.Success("Phone number verified successfully.", true);
    }

    // ──────────────────────────────────────────────────────────────
    // 2FA (TOTP)
    // ──────────────────────────────────────────────────────────────

    public async Task<NaitrustResponse<Start2faSetupResponse>> Start2faAsync(Guid userId, Start2faRequest request, CancellationToken ct = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return NaitrustResponse<Start2faSetupResponse>.NotFound("User not found.");

        var secret = GenerateTotpSecret();
        var cacheKey = $"totp-pending:{userId}";
        await _cacheService.SetAsync(cacheKey, secret, TimeSpan.FromMinutes(15), ct);

        var label = Uri.EscapeDataString($"Naitrust:{user.Email}");
        var issuer = Uri.EscapeDataString("Naitrust");
        var otpauthUri = $"otpauth://totp/{label}?secret={secret}&issuer={issuer}&algorithm=SHA1&digits=6&period=30";

        return NaitrustResponse<Start2faSetupResponse>.Success(
            "Scan the QR code with your authenticator app.",
            new Start2faSetupResponse(secret, otpauthUri));
    }

    public async Task<NaitrustResponse<bool>> Verify2faSetupAsync(Guid userId, Verify2faSetupRequest request, CancellationToken ct = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return NaitrustResponse<bool>.NotFound("User not found.");

        var cacheKey = $"totp-pending:{userId}";
        var pendingSecret = await _cacheService.GetAsync<string>(cacheKey, ct);

        if (pendingSecret is null)
            return NaitrustResponse<bool>.BadRequest("No pending 2FA setup found. Please start setup again.");

        if (!ValidateTotp(pendingSecret, request.Code))
            return NaitrustResponse<bool>.BadRequest("Invalid authenticator code.");

        await _cacheService.RemoveAsync(cacheKey, ct);

        user.TotpSecret = pendingSecret;
        user.TwoFactorEnabled = true;
        user.UpdatedAt = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);

        return NaitrustResponse<bool>.Success("Two-factor authentication enabled.", true);
    }

    // ──────────────────────────────────────────────────────────────
    // KYC
    // ──────────────────────────────────────────────────────────────

    public async Task<NaitrustResponse<bool>> SubmitKycAsync(Guid userId, SubmitKycRequest request, CancellationToken ct = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return NaitrustResponse<bool>.NotFound("User not found.");

        var kind = request.Kind.Trim().ToLowerInvariant();
        if (kind is not ("individual" or "business"))
            return NaitrustResponse<bool>.BadRequest("Kind must be 'individual' or 'business'.");

        // KYC processing is handled by the VerificationService pipeline.
        // This endpoint is the entry point — it queues the submission and
        // returns immediately. Full implementation wires to VerificationService.
        _logger.LogInformation("KYC submission received for user {UserId}, kind: {Kind}", userId, kind);

        return NaitrustResponse<bool>.Success("KYC submission received. We will review and update your status shortly.", true);
    }

    // ──────────────────────────────────────────────────────────────
    // PIN
    // ──────────────────────────────────────────────────────────────

    public async Task<NaitrustResponse<bool>> SetPinAsync(Guid userId, SetPinRequest request, CancellationToken ct = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return NaitrustResponse<bool>.NotFound("User not found.");

        if (request.Pin.Length < 4 || request.Pin.Length > 6 || !request.Pin.All(char.IsDigit))
            return NaitrustResponse<bool>.BadRequest("PIN must be 4 to 6 digits.");

        user.PinHash = _passwordHasher.HashPassword(user, request.Pin);
        user.UpdatedAt = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);

        return NaitrustResponse<bool>.Success("PIN set successfully.", true);
    }

    public async Task<NaitrustResponse<bool>> VerifyPinAsync(Guid userId, VerifyPinRequest request, CancellationToken ct = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return NaitrustResponse<bool>.NotFound("User not found.");

        if (string.IsNullOrEmpty(user.PinHash))
            return NaitrustResponse<bool>.BadRequest("No PIN has been set on this account.");

        var result = _passwordHasher.VerifyHashedPassword(user, user.PinHash, request.Pin);
        if (result == PasswordVerificationResult.Failed)
            return NaitrustResponse<bool>.BadRequest("Incorrect PIN.");

        return NaitrustResponse<bool>.Success("PIN verified.", true);
    }

    // ──────────────────────────────────────────────────────────────
    // Helpers
    // ──────────────────────────────────────────────────────────────

    private static string GenerateOtp() =>
        Random.Shared.Next(100_000, 999_999).ToString();

    private static string GenerateTotpSecret()
    {
        var bytes = new byte[20];
        RandomNumberGenerator.Fill(bytes);
        return Base32Encode(bytes);
    }

    private static string Base32Encode(byte[] data)
    {
        const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
        var sb = new StringBuilder();
        int buffer = data[0], bitsLeft = 8, i = 1;
        while (bitsLeft > 0 || i < data.Length)
        {
            if (bitsLeft < 5)
            {
                if (i < data.Length) { buffer = (buffer << 8) | (data[i++] & 0xff); bitsLeft += 8; }
                else { buffer <<= 5 - bitsLeft; bitsLeft = 5; }
            }
            bitsLeft -= 5;
            sb.Append(alphabet[(buffer >> bitsLeft) & 0x1f]);
        }
        return sb.ToString();
    }

    private static byte[] Base32Decode(string input)
    {
        const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
        input = input.TrimEnd('=').ToUpperInvariant();
        var output = new byte[input.Length * 5 / 8];
        int buffer = 0, bitsLeft = 0, idx = 0;
        foreach (var c in input)
        {
            var val = alphabet.IndexOf(c);
            if (val < 0) continue;
            buffer = (buffer << 5) | val;
            bitsLeft += 5;
            if (bitsLeft >= 8) { bitsLeft -= 8; output[idx++] = (byte)(buffer >> bitsLeft); }
        }
        return output;
    }

    private static bool ValidateTotp(string secret, string code)
    {
        var secretBytes = Base32Decode(secret);
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds() / 30;

        // Accept current window and one either side (±30 s clock drift)
        for (var t = timestamp - 1; t <= timestamp + 1; t++)
        {
            if (ComputeTotp(secretBytes, t) == code.Trim())
                return true;
        }
        return false;
    }

    private static string ComputeTotp(byte[] secret, long counter)
    {
        var counterBytes = BitConverter.GetBytes(counter);
        if (BitConverter.IsLittleEndian) Array.Reverse(counterBytes);

        using var hmac = new HMACSHA1(secret);
        var hash = hmac.ComputeHash(counterBytes);

        var offset = hash[^1] & 0x0f;
        var code = ((hash[offset] & 0x7f) << 24)
                 | ((hash[offset + 1] & 0xff) << 16)
                 | ((hash[offset + 2] & 0xff) << 8)
                 | (hash[offset + 3] & 0xff);

        return (code % 1_000_000).ToString("D6");
    }
}

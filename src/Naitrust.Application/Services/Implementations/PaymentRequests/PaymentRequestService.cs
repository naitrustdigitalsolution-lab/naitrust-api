using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.PaymentRequests;
using Naitrust.Domain.Models.Dtos.Responses.PaymentRequests;
using Naitrust.Domain.Models.Entities;
using Naitrust.Infrastructure.Data.Interfaces;

namespace Naitrust.Application.Services.Implementations.PaymentRequests;

public class PaymentRequestService : IPaymentRequestService
{
    private readonly IUnitOfWork _unitOfWork;

    public PaymentRequestService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<NaitrustResponse<List<PaymentRequestResponse>>> ListAsync(
        Guid userId, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<PaymentRequest>();
        var items = await repo.GetAllDataAsync(
            r => r.RequestedByUserId == userId && !r.IsDeleted,
            orderBy: q => q.OrderByDescending(r => r.CreatedAt));

        // Expire stale pending requests
        var now = DateTime.UtcNow;
        bool dirty = false;
        foreach (var r in items.Where(r => r.Status == "pending" && r.ExpiresAt < now))
        {
            r.Status = "expired";
            await repo.UpdateAsync(r);
            dirty = true;
        }
        if (dirty) { await _unitOfWork.SaveChangesAsync(); }

        return NaitrustResponse<List<PaymentRequestResponse>>.Success(
            "Payment requests retrieved.",
            items.Select(MapToResponse).ToList());
    }

    public async Task<NaitrustResponse<PaymentRequestResponse>> CreateAsync(
        Guid userId, CreatePaymentRequestRequest request, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<PaymentRequest>();

        var paymentRequest = new PaymentRequest
        {
            Id = Guid.NewGuid(),
            RequestedByUserId = userId,
            Reference = GenerateReference(),
            RequestedFromName = request.RequestedFromName,
            AmountMinor = request.AmountMinor,
            Currency = request.Currency,
            Reason = request.Reason,
            Status = "pending",
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            IsActive = true,
        };

        await repo.AddAsync(paymentRequest);
        await _unitOfWork.SaveChangesAsync();

        return NaitrustResponse<PaymentRequestResponse>.Created("Payment request sent.", MapToResponse(paymentRequest));
    }

    public async Task<NaitrustResponse<PaymentRequestResponse>> RespondAsync(
        Guid userId, Guid requestId, RespondPaymentRequestRequest request, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<PaymentRequest>();
        var paymentRequest = await repo.GetByIdAsync(requestId);

        if (paymentRequest is null || paymentRequest.IsDeleted)
            return NaitrustResponse<PaymentRequestResponse>.NotFound("Payment request not found.");

        if (paymentRequest.Status != "pending")
            return NaitrustResponse<PaymentRequestResponse>.BadRequest(
                $"Payment request is already {paymentRequest.Status}.");

        if (paymentRequest.ExpiresAt < DateTime.UtcNow)
        {
            paymentRequest.Status = "expired";
            await repo.UpdateAsync(paymentRequest);
            await _unitOfWork.SaveChangesAsync();
            return NaitrustResponse<PaymentRequestResponse>.BadRequest("Payment request has expired.");
        }

        var allowed = new[] { "fulfil", "decline" };
        if (!allowed.Contains(request.Action))
            return NaitrustResponse<PaymentRequestResponse>.BadRequest("Action must be 'fulfil' or 'decline'.");

        paymentRequest.Status = request.Action == "fulfil" ? "fulfilled" : "declined";
        await repo.UpdateAsync(paymentRequest);
        await _unitOfWork.SaveChangesAsync();

        return NaitrustResponse<PaymentRequestResponse>.Success("Response recorded.", MapToResponse(paymentRequest));
    }

    private static string GenerateReference()
        => $"PR-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..8].ToUpper()}";

    private static PaymentRequestResponse MapToResponse(PaymentRequest r) => new(
        r.Id,
        r.Reference,
        r.RequestedFromName,
        r.AmountMinor,
        r.Currency,
        r.Reason,
        r.Status,
        r.CreatedAt,
        r.ExpiresAt);
}

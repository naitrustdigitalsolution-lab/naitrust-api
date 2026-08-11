using Naitrust.Application.Services.Interfaces;
using Naitrust.Domain.Models.Dtos.Common;
using Naitrust.Domain.Models.Dtos.Requests.Beneficiaries;
using Naitrust.Domain.Models.Dtos.Responses.Beneficiaries;
using Naitrust.Domain.Models.Entities;
using Naitrust.Infrastructure.Data.Interfaces;

namespace Naitrust.Application.Services.Implementations.Beneficiaries;

public class BeneficiaryService : IBeneficiaryService
{
    private readonly IUnitOfWork _unitOfWork;

    public BeneficiaryService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<NaitrustResponse<List<BeneficiaryResponse>>> ListAsync(
        Guid userId, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<Beneficiary>();
        var items = await repo.GetAllDataAsync(
            b => b.UserId == userId && !b.IsDeleted,
            orderBy: q => q.OrderByDescending(b => b.IsFavourite).ThenByDescending(b => b.CreatedAt));

        return NaitrustResponse<List<BeneficiaryResponse>>.Success(
            "Beneficiaries retrieved.",
            items.Select(MapToResponse).ToList());
    }

    public async Task<NaitrustResponse<BeneficiaryResponse>> CreateAsync(
        Guid userId, CreateBeneficiaryRequest request, CancellationToken ct = default)
    {
        var allowedTypes = new[] { "naitrust_user", "bank_account" };
        if (!allowedTypes.Contains(request.Type))
            return NaitrustResponse<BeneficiaryResponse>.BadRequest("Invalid beneficiary type.");

        var repo = _unitOfWork.GetRepository<Beneficiary>();

        var beneficiary = new Beneficiary
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Type = request.Type,
            Name = request.Name,
            Email = request.Email,
            Phone = request.Phone,
            NaitrustIdentifier = request.NaitrustIdentifier,
            NaitrustAccountNumber = request.NaitrustAccountNumber,
            NaitrustId = request.NaitrustId,
            BankName = request.BankName,
            AccountNumber = request.AccountNumber,
            IsFavourite = false,
            IsActive = true,
        };

        await repo.AddAsync(beneficiary);
        await _unitOfWork.SaveChangesAsync();

        return NaitrustResponse<BeneficiaryResponse>.Created("Beneficiary saved.", MapToResponse(beneficiary));
    }

    public async Task<NaitrustResponse<bool>> DeleteAsync(
        Guid userId, Guid beneficiaryId, CancellationToken ct = default)
    {
        var repo = _unitOfWork.GetRepository<Beneficiary>();
        var beneficiary = await repo.GetByIdAsync(beneficiaryId);

        if (beneficiary is null || beneficiary.IsDeleted || beneficiary.UserId != userId)
            return NaitrustResponse<bool>.NotFound("Beneficiary not found.");

        beneficiary.IsDeleted = true;
        await repo.UpdateAsync(beneficiary);
        await _unitOfWork.SaveChangesAsync();

        return NaitrustResponse<bool>.Success("Beneficiary removed.", true);
    }

    private static BeneficiaryResponse MapToResponse(Beneficiary b) => new(
        b.Id,
        b.Type,
        b.Name,
        b.Email,
        b.Phone,
        b.NaitrustIdentifier,
        b.NaitrustAccountNumber,
        b.NaitrustId,
        b.BankName,
        b.AccountNumber,
        b.IsFavourite,
        b.CreatedAt);
}

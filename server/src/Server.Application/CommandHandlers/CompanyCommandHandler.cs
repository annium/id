using System;
using System.Threading;
using System.Threading.Tasks;
using Annium.Architecture.Base;
using Annium.Architecture.CQRS.Commands;
using Annium.Data.Operations;
using Server.Db.Repositories;
using Server.Domain.Commands.Companies;
using Server.Domain.Models;

namespace Server.Application.CommandHandlers;

internal class CompanyCommandHandler :
    ICommandHandler<RegisterCompanyCommand, Guid>,
    ICommandHandler<UpdateCompanyCommand>,
    ICommandHandler<SetCompanyOwnerCommand>,
    ICommandHandler<UnregisterCompanyCommand>
{
    private readonly ICompanyRepository _companyRepository;

    public CompanyCommandHandler(
        ICompanyRepository companyRepository
    )
    {
        _companyRepository = companyRepository;
    }

    public async Task<IStatusResult<OperationStatus, Guid>> HandleAsync(
        RegisterCompanyCommand request,
        CancellationToken cancellationToken
    )
    {
        var me = request.Me;
        var parent = request.Parent;

        if (parent is not null && me.Id != parent.OwnerId)
            return Result.Status(OperationStatus.Forbidden, Guid.Empty).Error("Need to be owner of parent company to create child company");

        var company = new Company(me, parent, request.Name);

        company = await _companyRepository.CreateAsync(company);

        return Result.Status(OperationStatus.Ok, company.Id);
    }

    public async Task<IStatusResult<OperationStatus>> HandleAsync(
        UpdateCompanyCommand request,
        CancellationToken cancellationToken
    )
    {
        var myId = request.MyId;
        var parent = request.Parent;
        var company = request.Company;

        if (myId != company.OwnerId)
            return Result.Status(OperationStatus.Forbidden).Error("Need to be company owner to update company");

        if (parent is not null && myId != parent.OwnerId)
            return Result.Status(OperationStatus.Forbidden).Error("Need to be owner of parent company to set child company parent");

        company.Update(parent, request.Name);

        await _companyRepository.UpdateAsync(company);

        return Result.Status(OperationStatus.Ok);
    }

    public async Task<IStatusResult<OperationStatus>> HandleAsync(
        SetCompanyOwnerCommand request,
        CancellationToken cancellationToken
    )
    {
        var myId = request.MyId;
        var user = request.User;
        var company = request.Company;

        if (myId != company.OwnerId)
            return Result.Status(OperationStatus.Forbidden).Error("Need to be company owner to change company owner");

        company.SetOwner(user);

        await _companyRepository.UpdateAsync(company);

        return Result.Status(OperationStatus.Ok);
    }

    public async Task<IStatusResult<OperationStatus>> HandleAsync(
        UnregisterCompanyCommand request,
        CancellationToken cancellationToken
    )
    {
        var myId = request.MyId;
        var company = request.Company;

        if (myId != company.OwnerId)
            return Result.Status(OperationStatus.Forbidden).Error("Need to be company owner to unregister company");

        await _companyRepository.DeleteByIdAsync(company.Id);

        return Result.Status(OperationStatus.Ok);
    }
}
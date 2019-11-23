using System;
using System.Threading;
using System.Threading.Tasks;
using Annium.Architecture.Base;
using Annium.Architecture.CQRS.Commands;
using Annium.Data.Operations;
using Annium.Id.Application.Commands.Companies;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Application.CommandHandlers
{
    internal class CompanyCommandHandler :
        ICommandHandler<RegisterCompanyCommand, Guid>,
        ICommandHandler<UpdateCompanyCommand>,
        ICommandHandler<SetCompanyOwnerCommand>,
        ICommandHandler<UnregisterCompanyCommand>
    {
        private readonly ICompanyRepository companyRepository;

        public CompanyCommandHandler(
            ICompanyRepository companyRepository
        )
        {
            this.companyRepository = companyRepository;
        }

        public async Task<IStatusResult<OperationStatus, Guid>> HandleAsync(
            RegisterCompanyCommand request,
            CancellationToken cancellationToken
        )
        {
            var myId = request.MyId;
            var parentId = request.ParentId;

            if (parentId.HasValue)
            {
                var parent = await companyRepository.GetByIdAsync(parentId.Value);
                if (parent is null)
                    return Result.Status(OperationStatus.NotFound, Guid.Empty).Error("Parent company not found");

                if (myId != parent.OwnerId)
                    return Result.Status(OperationStatus.Forbidden, Guid.Empty).Error("Need to be owner of parent company to create child company");
            }

            var company = new Company(
                myId,
                parentId,
                request.Key,
                request.Name
            );

            company = await companyRepository.CreateAsync(company);

            return Result.Status(OperationStatus.OK, company.Id);
        }

        public async Task<IStatusResult<OperationStatus>> HandleAsync(
            UpdateCompanyCommand request,
            CancellationToken cancellationToken
        )
        {
            var myId = request.MyId;
            var parentId = request.ParentId;
            var company = request.Company;

            if (myId != company.OwnerId)
                return Result.Status(OperationStatus.Forbidden).Error("Need to be company owner to update company");

            if (parentId.HasValue)
            {
                var parent = await companyRepository.GetByIdAsync(parentId.Value);
                if (parent is null)
                    return Result.Status(OperationStatus.NotFound).Error("Parent company not found");

                if (myId != parent.OwnerId)
                    return Result.Status(OperationStatus.Forbidden).Error("Need to be owner of parent company to set child company parent");
            }

            if (request.Key != company.Key && (await companyRepository.FindByKeyAsync(request.Key)) != null)
                return Result.Status(OperationStatus.Conflict).Error($"Company key {request.Key} is already used");

            company.ParentId = request.ParentId;
            company.Key = request.Key;
            company.Name = request.Name;

            await companyRepository.UpdateAsync(company);

            return Result.Status(OperationStatus.OK);
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

            company.OwnerId = user.Id;

            await companyRepository.UpdateAsync(company);

            return Result.Status(OperationStatus.OK);
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

            await companyRepository.DeleteByIdAsync(company.Id);

            return Result.Status(OperationStatus.OK);
        }
    }
}
using System;
using System.Threading;
using System.Threading.Tasks;
using Annium.Architecture.Base;
using Annium.Architecture.CQRS.Commands;
using Annium.Data.Operations;
using Annium.Id.Application.Commands.CompanyClaims;
using Annium.Id.Db.Repositories;
using Annium.Id.Domain.Entities;

namespace Annium.Id.Application.CommandHandlers
{
    internal class CompanyClaimCommandHandler : ICommandHandler<CreateCompanyClaimCommand, Guid>, ICommandHandler<UpdateCompanyClaimCommand>, ICommandHandler<DeleteCompanyClaimCommand>
    {
        private readonly IAppRepository appRepository;
        private readonly ICompanyClaimRepository companyClaimRepository;

        public CompanyClaimCommandHandler(
            IAppRepository appRepository,
            ICompanyClaimRepository companyClaimRepository
        )
        {
            this.appRepository = appRepository;
            this.companyClaimRepository = companyClaimRepository;
        }

        public async Task<IStatusResult<OperationStatus, Guid>> HandleAsync(
            CreateCompanyClaimCommand request,
            CancellationToken cancellationToken
        )
        {
            var myId = request.MyId;
            var app = request.App;

            if (myId != app.OwnerId)
                return Result.Status(OperationStatus.Forbidden, Guid.Empty).Error($"Need to be application owner to create company claim");

            var claim = new CompanyClaim(
                app.Id,
                request.Key,
                request.Name
            );

            claim = await companyClaimRepository.CreateAsync(claim);

            return Result.Status(OperationStatus.OK, claim.Id);
        }

        public async Task<IStatusResult<OperationStatus>> HandleAsync(
            UpdateCompanyClaimCommand request,
            CancellationToken cancellationToken
        )
        {
            var myId = request.MyId;
            var app = await appRepository.GetByIdAsync(request.Claim.AppId);
            var claim = request.Claim;

            if (myId != app.OwnerId)
                return Result.Status(OperationStatus.Forbidden).Error($"Need to be application owner to update company claim");

            if (request.Key != claim.Key && (await companyClaimRepository.FindByKeyAsync(app.Id, request.Key)) != null)
                return Result.Status(OperationStatus.Conflict).Error($"Company claim key {request.Key} is already used");

            claim.Key = request.Key;
            claim.Name = request.Name;

            await companyClaimRepository.UpdateAsync(claim);

            return Result.Status(OperationStatus.OK);
        }

        public async Task<IStatusResult<OperationStatus>> HandleAsync(
            DeleteCompanyClaimCommand request,
            CancellationToken cancellationToken
        )
        {
            var myId = request.MyId;
            var app = await appRepository.GetByIdAsync(request.Claim.AppId);
            var claim = request.Claim;

            if (myId != app.OwnerId)
                return Result.Status(OperationStatus.Forbidden).Error($"Need to be application owner to delete company claim");

            await companyClaimRepository.DeleteByIdAsync(claim.Id);

            return Result.Status(OperationStatus.OK);
        }
    }
}
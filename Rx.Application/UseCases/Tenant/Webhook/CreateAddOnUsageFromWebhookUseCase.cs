﻿using AutoMapper;
using MediatR;
using Rx.Domain.DTOs.Tenant.AddOnUsage;
using Rx.Domain.Entities.Tenant;
using Rx.Domain.Interfaces;
using Rx.Domain.Interfaces.DbContext;

namespace Rx.Application.UseCases.Tenant.Webhook;

public record CreateAddOnUsageFromWebhookUseCase(AddOnUsageFromWebhookForCreationDto AddOnUsageFromWebhookForCreationDto):IRequest<string>;

public class CreateAddOnUsageFromWebhookUseCaseHandler : IRequestHandler<CreateAddOnUsageFromWebhookUseCase, string>
{
    private readonly ITenantServiceManager _tenantServiceManager;
    private readonly IMapper _mapper;
    private readonly ITenantDbContext _tenantDbContext;

    public CreateAddOnUsageFromWebhookUseCaseHandler(ITenantServiceManager tenantServiceManager,IMapper mapper,ITenantDbContext tenantDbContext)
    {
        _tenantServiceManager = tenantServiceManager;
        _mapper = mapper;
        _tenantDbContext = tenantDbContext;
    }

    public async Task<string> Handle(CreateAddOnUsageFromWebhookUseCase request, CancellationToken cancellationToken)
    {
        var addOnWebhookDto = new AddOnWebhookDto(
            SenderAddOnWebhookId: request.AddOnUsageFromWebhookForCreationDto.SenderAddOnWebhookId,
            AddOnId: request.AddOnUsageFromWebhookForCreationDto.AddOnId,
            SubscriptionId: request.AddOnUsageFromWebhookForCreationDto.SubscriptionId,
            Unit: request.AddOnUsageFromWebhookForCreationDto.Unit,
            RetrievedDateTime: DateTime.Now,
            OrganizationCustomerId:request.AddOnUsageFromWebhookForCreationDto.CustomerId
        );
        var addOnWebhook = _mapper.Map<AddOnWebhook>(addOnWebhookDto);
        _tenantDbContext.AddOnWebhooks.Add(addOnWebhook);
        await _tenantDbContext.SaveChangesAsync();

        return await _tenantServiceManager.AddOnUsageService.CreateAddOnUsageFromWebhook(addOnWebhook);
    }
}
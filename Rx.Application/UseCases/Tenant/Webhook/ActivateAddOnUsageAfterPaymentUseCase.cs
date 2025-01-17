﻿using MediatR;
using Rx.Domain.DTOs.Tenant.Transaction;
using Rx.Domain.Interfaces;
using Rx.Domain.Interfaces.DbContext;

namespace Rx.Application.UseCases.Tenant.Webhook;

public record ActivateAddOnUsageAfterPaymentUseCase(string WebhookId,long Amount):IRequest<string>;

public class ActivateAddOnUsageAfterPaymentUseCaseHandler:IRequestHandler<ActivateAddOnUsageAfterPaymentUseCase,string>
{
    private readonly ITenantServiceManager _tenantServiceManager;

    public ActivateAddOnUsageAfterPaymentUseCaseHandler(ITenantServiceManager tenantServiceManager)
    {
        _tenantServiceManager = tenantServiceManager;

    }
    public async Task<string> Handle(ActivateAddOnUsageAfterPaymentUseCase request, CancellationToken cancellationToken)
    {
        return await _tenantServiceManager.AddOnUsageService.ActivateAddOnUsageAfterPayment(request.WebhookId,request.Amount);
    }
}
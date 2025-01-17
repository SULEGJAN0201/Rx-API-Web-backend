﻿using AutoMapper;
using Azure.Storage.Blobs;
using Hangfire;
using Microsoft.Extensions.Logging;
using Rx.Domain.Interfaces;
using Rx.Domain.Interfaces.Blob;
using Rx.Domain.Interfaces.DbContext;
using Rx.Domain.Interfaces.Payment;
using Rx.Domain.Interfaces.Tenant;
using Rx.Domain.Interfaces.WebhookSendClient;

namespace Rx.Domain.Services.Tenant
{
    public sealed class TenantServiceManager : ITenantServiceManager
    {
        private readonly IBillingService _billingService;
        private readonly Lazy<IOrganizationCustomerService> _organizationCustomerService;
        private readonly Lazy<IProductService> _productService;
        private readonly Lazy<ISubscriptionService> _subscriptionService;
        private readonly Lazy<IProductPlanService> _productPlanService;
        private readonly Lazy<ITransactionService> _transactionService;
        private readonly Lazy<IAddOnService> _addOnService;
        private readonly Lazy<IAddOnUsageService> _addOnUsageService;

        public TenantServiceManager(ITenantDbContext tenantDbContext,
            ILogger<TenantServiceManager> logger,
            IPaymentService paymentService,
            IMapper mapper,
            IBackgroundJobClient backgroundJobClient,
            IRecurringJobManager recurringJobManager,
            IBlobStorage blobStorage,
            ISendWebhookService sendWebhookService,
            IBillingService billingService
            )
        {
            _billingService = billingService;
            _organizationCustomerService = new Lazy<IOrganizationCustomerService>(() => new OrganizationCustomerService(tenantDbContext, logger, mapper,paymentService,billingService,recurringJobManager));
            _productService = new Lazy<IProductService>(() => new ProductService(tenantDbContext, logger, mapper,blobStorage));
            _subscriptionService = new Lazy<ISubscriptionService>(() => new SubscriptionService(tenantDbContext, logger, mapper,backgroundJobClient,paymentService,recurringJobManager,sendWebhookService));
            _productPlanService = new Lazy<IProductPlanService>(() => new ProductPlanService(tenantDbContext, mapper, logger) );
            _transactionService = new Lazy<ITransactionService>(() => new TransactionService(tenantDbContext, mapper, logger));
            _addOnService = new Lazy<IAddOnService>(() => new AddOnService(tenantDbContext, mapper, logger));
            _addOnUsageService = new Lazy<IAddOnUsageService>(() => new AddOnUsageService(tenantDbContext, logger,mapper,paymentService,sendWebhookService));
        }


        public IOrganizationCustomerService OrganizationCustomerService => _organizationCustomerService.Value;
        public IProductService ProductService => _productService.Value;
        public ISubscriptionService SubscriptionService => _subscriptionService.Value;
        public IProductPlanService ProductPlanService => _productPlanService.Value;
        public ITransactionService TransactionService => _transactionService.Value;
        public IAddOnService AddOnService => _addOnService.Value;
        public IAddOnUsageService AddOnUsageService => _addOnUsageService.Value;

    }
}

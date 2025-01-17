﻿using AutoMapper;
using Microsoft.Extensions.Logging;
using Rx.Domain.Interfaces.DbContext;
using Rx.Domain.Interfaces.Primary;

namespace Rx.Domain.Services.Primary;

public class SystemSubscriptionPlanService:ISystemSubscriptionPlanService
{
    public SystemSubscriptionPlanService(IPrimaryDbContext primaryDbContext, ILogger<PrimaryServiceManager> logger, IMapper mapper)
    {
        throw new NotImplementedException();
    }
}
﻿using System.Globalization;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rx.Domain.DTOs.Tenant.Report;
using Rx.Domain.Interfaces;
using Rx.Domain.Interfaces.DbContext;

namespace Rx.Application.UseCases.Tenant.Report;

public record GetSubscriptionStatUseCase():IRequest<IEnumerable<Stats>>;

public class GetSubscriptionStatUseCaseHandler: IRequestHandler<GetSubscriptionStatUseCase, IEnumerable<Stats>>
{
    private readonly ITenantDbContext _tenantDbContext;

    public GetSubscriptionStatUseCaseHandler(ITenantDbContext tenantDbContext)
    {
        _tenantDbContext = tenantDbContext;
    }

    public async Task<IEnumerable<Stats>> Handle(GetSubscriptionStatUseCase request, CancellationToken cancellationToken)
    {
        var monthsRequired = 6;
        var result =await _tenantDbContext.Subscriptions!
            .Where(s=>s.CreatedDate>DateTime.Now.AddMonths(-1*monthsRequired))
            .ToListAsync(cancellationToken: cancellationToken);
        var statCount = result.GroupBy(x => x.CreatedDate.ToString("MMMM")).Select(x => new Stats(
                x.Key,
                x.Count()
            )
        ).ToList();
        var stats =new  Stats[monthsRequired];
        for (var i = 0; i < monthsRequired; i++)
        {
            var month =DateTimeFormatInfo.CurrentInfo.GetMonthName(DateTime.Now.AddMonths(i-(monthsRequired-1)).Month) ;
            var index = statCount.FindIndex(x => x.Type == month);
            var count = 0;
            if (index != -1)
            {
                count = statCount[index].Count;
            }
            stats[i] = new Stats(month, count);
        }
        return stats;
    }
}
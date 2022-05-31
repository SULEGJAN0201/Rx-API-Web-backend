﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rx.Domain.Interfaces.Blob;
using Rx.Domain.Interfaces.DbContext;
using Rx.Infrastructure.Persistence.Context;

namespace Rx.Infrastructure.Persistence
{
    public static class Dependencies
    {
        public static void AddPrimaryDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PrimaryDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("PrimaryDbConnection"),
                    b => b.MigrationsAssembly(typeof(PrimaryDbContext).Assembly.FullName)));
            // services.AddTransient<IPrimaryDbContext>(provider => provider.GetService<PrimaryDbContext>() ?? throw new InvalidOperationException());
            services.AddTransient<IPrimaryDbContext, PrimaryDbContext>();
        }
        public static void AddTenantDb(this IServiceCollection services, IConfiguration configuration)
        {
           
            services.AddDbContext<TenantDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("TenantDbConnection"),
                    b => b.MigrationsAssembly(typeof(TenantDbContext).Assembly.FullName)));

            services.AddTransient<ITenantDbContext,TenantDbContext>();
        }
        public static void AddBlobStorage(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAzureClients(b=>
            {
                b.AddBlobServiceClient(configuration.GetConnectionString("productBlobStorageConnectionString"));
            });
            services.AddScoped<IBlobStorage, BlobStorage>();
        }
    }
}

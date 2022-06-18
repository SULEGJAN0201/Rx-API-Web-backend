﻿using System.Diagnostics;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rx.Domain.DTOs.Primary.Organization;
using Rx.Domain.Entities.Primary;
using Rx.Domain.Interfaces.Blob;
using Rx.Domain.Interfaces.DbContext;
using Rx.Domain.Interfaces.Email;
using Rx.Domain.Interfaces.Identity;
using Rx.Domain.Interfaces.Primary;


namespace Rx.Domain.Services.Primary
{
    public class OrganizationService : IOrganizationService
    {
        private readonly IPrimaryDbContext _primaryDbContext;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IBlobStorage _blobStorage;
        private readonly IEmailService _emailService;
        private readonly IUserService _userService;

        public OrganizationService(IPrimaryDbContext primaryDbContext,
            ILogger<PrimaryServiceManager> logger,
            IMapper mapper,
            IBlobStorage blobStorage,
            IEmailService emailService,
            IUserService userService
            )
        {
            _primaryDbContext = primaryDbContext;
            _logger = logger;
            _mapper = mapper;
            _blobStorage = blobStorage;
            _emailService = emailService;
            _userService = userService;
        }
        public async Task<Guid> CreateOrganizationAsync(CreateOrganizationRequestDto createOrganizationRequestDto)
        {
            string? logoUrl = null;
            if (createOrganizationRequestDto.LogoImage != null)
            {
                var fileName = string.Empty;
                _logger.LogInformation("Upload Started");
                var logoImage = createOrganizationRequestDto.LogoImage;
                if (logoImage.Length > 0)
                {
                    await using var fileStream = new FileStream(logoImage.FileName, FileMode.Create);
                    _logger.LogInformation("file found");
                    await logoImage.CopyToAsync(fileStream);
                    fileName = fileStream.Name;
                }
                var stream = File.OpenRead(logoImage.FileName);
                logoUrl = await _blobStorage.UploadOrganizationLogo(stream);
                _logger.LogInformation("Upload Completed");
                stream.Close();
                File.Delete(fileName);
            }

            var organizationEntity = new Organization
            {
                Name = createOrganizationRequestDto.Name,
                LogoURL = logoUrl,
                AccountOwnerId = createOrganizationRequestDto.AccountOwnerId,
                Description = createOrganizationRequestDto.Description,
                Email = createOrganizationRequestDto.Email
            };
            await _primaryDbContext.Organizations!.AddAsync(organizationEntity);
            await _primaryDbContext.SaveChangesAsync();
            //Add Address
            var organizationAddress = new OrganizationAddress
            {
                AddressLine1 = createOrganizationRequestDto.OrganizationAddress.AddressLine1,
                AddressLine2 = createOrganizationRequestDto.OrganizationAddress.AddressLine2,
                City = createOrganizationRequestDto.OrganizationAddress.City,
                State = createOrganizationRequestDto.OrganizationAddress.State,
                Country = createOrganizationRequestDto.OrganizationAddress.Country,
                OrganizationId = organizationEntity.Id,
            };
            await _primaryDbContext.OrganizationAddresses!.AddAsync(organizationAddress);
            await _primaryDbContext.SaveChangesAsync();
            
            //Update OrganizationId in User
            await _userService.UpdateUserAsync(createOrganizationRequestDto.AccountOwnerId!, organizationEntity.Id);
            
            return organizationEntity.Id;
        }

        public async Task<IEnumerable<OrganizationDto>> GetOrganizationsAsync(bool trackChanges)
        {
            var organizations = await _primaryDbContext.Organizations!.ToListAsync();
            return _mapper.Map<IEnumerable<OrganizationDto>>(organizations);
        }
        
        public async Task AddPaymentGatewayIdForOrganization(Guid organizationId, string paymentGatewayId)
        {
            var organization = await _primaryDbContext.Organizations!.FindAsync(organizationId);
            organization!.PaymentGatewayId = paymentGatewayId;
            await _primaryDbContext.SaveChangesAsync();
        }

        public async Task AddPaymentMethodIdForOrganization(Guid organizationId, string paymentMethodId)
        {
            var organization = await _primaryDbContext.Organizations!.FindAsync(organizationId);
            organization.PaymentMethodId = paymentMethodId;
            await _primaryDbContext.SaveChangesAsync();
        }
        
        public async Task<OrganizationDto> CreateOrganizationTest(OrganizationForCreationDto organizationForCreationDto)
        {
            var organizationEntity = _mapper.Map<Organization>(organizationForCreationDto);
            Debug.Assert(_primaryDbContext.Organizations != null, "_primaryContext.Organizations != null");
            await _primaryDbContext.Organizations.AddAsync(organizationEntity);
            await _primaryDbContext.SaveChangesAsync();
            var organizationDto = _mapper.Map<OrganizationDto>(organizationEntity);
            return organizationDto;
        }
    }
}

﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rx.API.Middleware;
using Rx.Application.UseCases.Tenant.Customer;
using Rx.Application.UseCases.Tenant.Product;
using Rx.Domain.DTOs.Request;
using Rx.Domain.DTOs.Tenant.OrganizationCustomer;
using Swashbuckle.AspNetCore.Annotations;

namespace Rx.API.Controllers.Tenant
{
    [Route("api/customer")]
    [ApiController]
    [SubscriptionVerification]
    [Authorize(Roles = "Admin")]
    public class CustomerController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CustomerController> _logger;
        public CustomerController(IMediator mediator, ILogger<CustomerController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        
        [HttpGet]
        [SwaggerOperation(Summary = "Get all customers")]
        public async Task<IActionResult> GetCustomers([FromQuery] RequestParameters requestParameters)
        {
            _logger.LogInformation(requestParameters.SearchKey);
            var customers = await _mediator.Send(new GetCustomersUseCase(requestParameters.SearchKey??"") );
            return Ok(customers);
        }
        
        [HttpGet("dto")]
        [SwaggerOperation(Summary = "Get all customersDto")]
        public async Task<IActionResult> GetCustomersDto()
        {
            var customers = await _mediator.Send(new GetCustomersDtoUseCase() );
            return Ok(customers);
        }
        [HttpGet("{id:guid}")]
        [SwaggerOperation(Summary = "Get customer by id")]
        public async Task<IActionResult> GetCustomerById(Guid id)
        {
            var customer = await _mediator.Send(new GetCustomerByIdUseCase(id));
            return Ok(customer);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a new customer")]
        public async Task<IActionResult> CreateCustomer(OrganizationCustomerForCreationDto? organizationCustomerForCreationDto)
        {
            if (organizationCustomerForCreationDto is null)
            {
                return BadRequest("Empty body");
            }

            var createdCustomer =await _mediator.Send(new AddCustomerUseCase(organizationCustomerForCreationDto));
            return CreatedAtAction(nameof(GetCustomerById), new {id = createdCustomer.CustomerId}, createdCustomer);
        }

        [HttpGet("stats")]
        [SwaggerOperation(Summary = "Get Customer Stats")]
        public async Task<IActionResult> GetCustomerStats()
        {
            var stats = await _mediator.Send(new GetCustomerStatsUseCase() );
            return Ok(stats);
        }
        
        [HttpGet("product/{productId}")]
        [SwaggerOperation(Summary = "Get customers for given product")]
        public async Task<IActionResult> GetCustomersForProduct(Guid productId)
        {
            var customers = await _mediator.Send(new GetCustomersForProductUseCase(productId));
            return Ok(customers);
        }
        
    }
}
    
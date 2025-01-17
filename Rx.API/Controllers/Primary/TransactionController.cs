﻿using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Rx.API.Controllers.Primary;

[ApiController]
[Route("api/org/transaction")]
public class TransactionController:ControllerBase
{
    private readonly IMediator _mediator;

    public TransactionController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
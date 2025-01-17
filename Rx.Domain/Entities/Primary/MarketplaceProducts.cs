﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rx.Domain.Entities.Primary;

public class MarketplaceProducts
{
    [Key]
    public Guid ProductId { get; set; }
    public string? Description { get; set; }
    public string? Name { get; set; }
    public string? LogoUrl { get; set; }
    public bool? HaveTrial { get; set; }
    public string? ProviderName { get; set; }
    public int TrialDays { get; set; }
    public string? RedirectUrl { get; set; }
    
}
﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Rx.Domain.Entities.Tenant
{
    public class OrganizationCustomer
    {
        [Key]
        [Column("CustomerId")]
        public Guid CustomerId { get; set; }

        [Required(ErrorMessage = "Email is a required field.")]

        [EmailAddress]
        public string? Email { get; set; }


        [Required(ErrorMessage = "Customer name is a required field.")]

        [MaxLength(60, ErrorMessage = "Maximum length for the Name is 60 characters.")]
        public string? Name { get; set; }

        public string? PaymentGatewayId { get; set; }
        public string? Last4 { get; set; }
        public string? PaymentMethodId { get; set; }
        public ICollection<Subscription>? Subscriptions {get; set;}
        public ICollection<Bill>? Bills { get; set; }
    }
}

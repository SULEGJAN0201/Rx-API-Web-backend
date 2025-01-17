﻿namespace Rx.Domain.DTOs.Payment;

public class PaymentModel
{
    public class CustomerModel
    {
        public string Id { get; set; }
        public string? SystemId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }

        public List<PaymentMethodModel>? PaymentMethods { get; set; }

        public CustomerModel(string id)
        {
            Id = id;
        }
    }
    public class FuturePaymentIntentModel
    {
        public string? Id { get; set; }
        public string? IntentSecret { get; set; }
        public CustomerModel? Customer { get; set; }
    }



    public enum PaymentModelInclude
    {
    PaymentMethods
    }
    
    public class PaymentMethodModel
    {
        public string Id { get; set; }
        public PaymentMethodType? Type { get; set; }
        public PaymentMethodCardModel? Card { get; set; }

        public PaymentMethodModel(string id)
        {
            Id = id;
        }
    }

    public class PaymentMethodCardModel
    {
        public string? Brand { get; set; }
        public string? Country { get; set; }
        public string? Description { get; set; }
        public long ExpMonth { get; set; }
        public long ExpYear { get; set; }
        public string? Fingerprint { get; set; }
        public string? Funding { get; set; }
        public string? Iin { get; set; }
        public string? Issuer { get; set; }
        public string? Last4 { get; set; }
    }

    public class ChargeModel
    {
        public string Id { get; }
        public string? Status { get; set; }

        public ChargeModel(string id)
        {
            Id = id;
        }
    }

    public enum Currency
    {
        USD
    }
    public enum PaymentMethodType
    {
        Card
    }
}
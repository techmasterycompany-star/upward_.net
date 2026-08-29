
﻿using Upwork.Domain.Common;
using Upwork.Domain.Enums;

namespace Upwork.Domain.Entities
{
    public class Payment : BaseEntity
    {
        public long SubscriptionId { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; } = null!;

        public PaymentGateway Gateway { get; set; }

        public string? GatewayTransactionId { get; set; }

        public PaymentStatus Status { get; set; }

        public DateTime? PaidAt { get; set; }

        public Subscription Subscription { get; set; } = null!;
    }
}

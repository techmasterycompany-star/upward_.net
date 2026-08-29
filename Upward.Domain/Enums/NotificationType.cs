namespace Upward.Domain.Enums
{
    public enum NotificationType
    {
        ApplicationSubmitted = 1,
        ApplicationAccepted = 2,
        ApplicationRejected = 3,
        ApplicationStatusChanged = 4,

        NewApplicationReceived = 5,
        JobApproved = 6,
        JobRejected = 7,
        JobDeadlineApproaching = 8,
        JobExpired = 9,

        NewJobPendingApproval = 10,

        PaymentCompleted = 11,
        PaymentFailed = 12,
        PaymentRefunded = 13,
        SubscriptionRenewed = 14,
        SubscriptionCancelled = 15
    }
}

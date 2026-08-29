namespace Upward.Domain.Enums
{
    public enum NotificationType
    {
        ApplicationSubmitted = 1,
        ApplicationStatusChanged = 2,

        JobApproved = 3,
        JobRejected = 4,

        PaymentCompleted = 5,
        PaymentFailed = 6,
        PaymentRefunded = 7,

        SubscriptionRenewed = 8,
        SubscriptionCancelled = 9,
        AdminBroadcast = 10
    }
}

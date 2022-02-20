namespace Shared.Messages
{
    public record Payment
    {
        public decimal Amount { get; }
        public string RecipientAccountNumber { get; }
        public string SenderAccountNumber { get; }

        public Payment(decimal amount, string recipientAccountNumber, string senderAccountNumber)
        {
            Amount = amount;
            RecipientAccountNumber = recipientAccountNumber;
            SenderAccountNumber = senderAccountNumber;
        }
    }
}
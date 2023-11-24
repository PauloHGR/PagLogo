namespace PagLogo.Models
{
    public class TransactionFilterRequest
    {
        public string? UserSourceIdentifier { get; set; }
        public string? UserReceiverIdentifier { get; set; }
        public double Value { get; set; }
    }
}

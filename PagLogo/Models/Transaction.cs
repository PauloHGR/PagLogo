namespace PagLogo.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public double Value { get; set; }
        public int Payer { get; set; }
        public int Payee { get; set; }
    }
}

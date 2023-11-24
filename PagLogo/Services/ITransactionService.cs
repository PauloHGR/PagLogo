using PagLogo.Models;

namespace PagLogo.Services
{
    public interface ITransactionService
    {
        Task CallTransactionAsync(TransactionFilterRequest request);
    }
}

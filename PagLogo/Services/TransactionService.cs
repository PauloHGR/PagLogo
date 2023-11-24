using Microsoft.AspNetCore.Mvc;
using PagLogo.Exceptions;
using PagLogo.Models;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace PagLogo.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IAppDbContext _context;
        private readonly IUserService _userService;
        static HttpClient client = new HttpClient();

        public TransactionService(IAppDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        private async Task SendNotification(User user)
        {
            HttpResponseMessage response = await client.GetAsync("https://run.mocky.io/v3/54dc2cf1-3add-45b5-b5a9-6bf7e7f1f4a6");
            if (response.IsSuccessStatusCode)
            {
                var product = await response.Content.ReadAsAsync<AuthDTO>();
                Console.WriteLine(product.Message);
                
            }
            else
            {
                throw new TransactionException("Erro ao notificar usuário sobre transação.");
            }
        }

        public async Task CallTransactionAsync(TransactionFilterRequest request)
        {
            var userSource = await _userService.GetUserAsync(request.UserSourceIdentifier);

            if(userSource.UserType == Enums.UserType.Cnpj) {
                throw new TransactionException("Operação Inválida! Somente pessoas físicas podem realizar transações.");
            }

            if(userSource.Balance < request.Value)
            {
                throw new TransactionException("Saldo Insuficiente");
            }

            HttpResponseMessage response = await client.GetAsync("https://run.mocky.io/v3/5794d450-d2e2-4412-8131-73d0293ac1cc");
            if (response.IsSuccessStatusCode)
            {
                var product = await response.Content.ReadAsAsync<AuthDTO>();
                Console.WriteLine(product.Message);

                var userReceiver = await _userService.GetUserAsync(request.UserReceiverIdentifier);

                //Update userReceiver
                userReceiver.Balance = userReceiver.Balance + request.Value;
                //Update userSource
                userSource.Balance = userSource.Balance - request.Value;

                var transaction = new Transaction
                {
                    Value = request.Value,
                    Payer = userSource.Id,
                    Payee = userReceiver.Id
                };
                _context.Transactions.Add(transaction);

                _context.Users.Update(userReceiver);
                _context.Users.Update(userSource);
                _context.SaveChanges();
                //Send sms

                await SendNotification(userReceiver);

            } else
            {
                throw new TransactionException("Erro na autorização.");
            }
        }
    }
}

using Shared.Messages;
using TransactionAnalysisService.Interfaces;

namespace TransactionAnalysisService
{
    public class TransactionAnalysisProcessor : ITransactionAnalysisProcessor
    {
        private int transactionsDeclined = 0;
        private int transactionsAccepted = 0;

        public void ProcessTransaction(LaundryCheckResult laundryCheckResult)
        {
            if(laundryCheckResult.IsSuspicious)
            {
                transactionsDeclined++;
            }
            else
            {
                transactionsAccepted++;
            }

            Console.WriteLine($"Transactions accepted: {transactionsAccepted} \t Transactions declined: {transactionsDeclined}");
        }
    }
}

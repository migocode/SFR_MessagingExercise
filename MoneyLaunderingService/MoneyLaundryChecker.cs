using MoneyLaunderingService.Interfaces;
using Shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyLaunderingService
{
    public class MoneyLaundryChecker : IMoneyLaundryChecker
    {
        public LaundryCheckResult Check(Payment payment)
        {
            if (payment.Amount > 1000)
            {
                return new LaundryCheckResult(payment, "Transaction exceeds 1000", true);
            }

            return new LaundryCheckResult(payment, "", false);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Messages
{
    public record LaundryCheckResult
    {
        public Payment Payment { get; }
        public string Message { get; } = string.Empty;
        public bool IsSuspicious { get; }

        public LaundryCheckResult(Payment payment, string message, bool isSuspicious)
        {
            Payment = payment;
            Message = message;
            IsSuspicious = isSuspicious;
        }
    }
}

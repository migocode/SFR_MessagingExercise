using Shared.Messages;

namespace MoneyLaunderingService.Interfaces
{
    public interface IMoneyLaundryChecker
    {
        LaundryCheckResult Check(Payment payment);
    }
}

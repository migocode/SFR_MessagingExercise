using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyLaunderingService
{
    public class Settings
    {
        public string? TopicPayments { get; set; }
        public string? TopicLaundryCheck { get; set; }
        public string? KafkaServers { get; set; }
    }
}

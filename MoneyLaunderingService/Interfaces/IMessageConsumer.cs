﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyLaunderingService.Interfaces
{
    public interface IMessageConsumer
    {
        void StartConsumption(CancellationToken cancellationToken);
    }
}

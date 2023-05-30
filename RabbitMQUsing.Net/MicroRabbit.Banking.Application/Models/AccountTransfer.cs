﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroRabbit.Banking.Application.Models
{
    public class AccountTransfer
    {
        public int ToAccount { get; set; }
        public int FromAccount { get; set; }
        public decimal TransferAmount { get; set;}

    }
}

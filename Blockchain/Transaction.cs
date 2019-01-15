using System;
using System.Collections.Generic;
using System.Text;

namespace Blockchain
{
    public class Transaction
    {
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public decimal Amount { get; set; }

        public Transaction(string fromAddress, string toAddress, decimal amount)
        {
            FromAddress = fromAddress;
            ToAddress = toAddress;
            Amount = amount;
        }
    }
}

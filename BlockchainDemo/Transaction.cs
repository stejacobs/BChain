using System;
using System.Collections.Generic;
using System.Text;

namespace BCTestDemo
{

    public class PendingTransaction
    {
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public string OwnerName { get; set; }
        public int Amount { get; set; }
        public string transid { get; set; }
        public object transactions { get; set; }
    }

    public class Chain
    {
        public int Index { get; set; }
        public DateTime TimeStamp { get; set; }
        public string PreviousHash { get; set; }
        public string Hash { get; set; }
        public List<Transaction> Transactions { get; set; }
        public int Nonce { get; set; }
    }

    public class RootObject
    {
        public List<Transaction> PendingTransactions { get; set; }
        public int CurrentPropTaxOwed { get; set; }
        public List<Chain> Chain { get; set; }
        public int Difficulty { get; set; }
    }

    public class Transaction
    {
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public string OwnerName { get; set; }
        public int Amount { get; set; }
        public string transid { get; set; } = Guid.NewGuid().ToString("N").ToUpper();


        public Transaction(string fromAddress, string toAddress, int amount, string owerName)
        {
            FromAddress = fromAddress;
            ToAddress = toAddress;
            Amount = amount;
            OwnerName = owerName;
             //Unique Identifier for transaction
        }
    }
}

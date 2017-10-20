using System.Diagnostics;

namespace CCC
{
    [DebuggerDisplay("{TransactionSubmitTime} {Amount} $ : [{AccountFrom}] -> [{AccountTo}]")]
    public class Transaction
    {
        public Account AccountFrom { get; set;  }
        public Account AccountTo { get; set; }
        public int Amount { get; set; }
        public long TransactionSubmitTime { get; set; }

        public Transaction(Account accountFrom, Account accountTo, int amount, long transactionSubmitTime)
        {
            AccountFrom = accountFrom;
            AccountTo = accountTo;
            Amount = amount;
            TransactionSubmitTime = transactionSubmitTime;
        }

        public bool IsValid()
        {
            if (!AccountFrom.Validate()) return false;
            if (!AccountTo.Validate()) return false;
            if (!AccountFrom.ValidateWithdraw(Amount)) return false;

            return true;
        }

        public void Execute()
        {
            AccountFrom.Balance -= Amount;
            AccountTo.Balance += Amount;   
        }
    }
}
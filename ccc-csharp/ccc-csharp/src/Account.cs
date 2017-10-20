using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace CCC
{
    [DebuggerDisplay("{PersonName} {AccountNumber} {Balance} {OverdraftLimit}")]
    public class Account
    {
        public string PersonName { get; set; }
        public string AccountNumber { get; set; }
        public int Balance { get; set; }
        public int OverdraftLimit { get; set; }

        public Account(string personPersonName, string accountNumber, int actualAccountBalance, int overdraftLimit)
        {
            PersonName = personPersonName;
            AccountNumber = accountNumber;
            Balance = actualAccountBalance;
            OverdraftLimit = overdraftLimit;
        }

        public bool ValidateWithdraw(int amount)
        {
            return Balance + OverdraftLimit - amount >= 0;
        }

        public bool Validate()
        {
            return ValidateAccountNumber(AccountNumber);
        }
        
        public static bool ValidateAccountNumber(string accountNumber)
        {
            if (!Regex.IsMatch(accountNumber, @"CAT\d{2}[a-zA-Z]{10}")) return false;

            int checksum = int.Parse(accountNumber.Substring(3, 2));
            string id = accountNumber.Substring(5, 10);

            // constraint 1
            foreach (char ch in id)
            {
                int down = id.Count(f => f == char.ToLower(ch));
                int up = id.Count(f => f == char.ToUpper(ch));
                if (down != up)
                {
                    Console.WriteLine($"Invalid account number: {accountNumber}");
                    return false;
                }
            }

            // constraint 2
            int sum = 0;
            foreach (char ch in id + "CAT00")
            {
                sum += ch;
            }
            int remainder = sum % 97;
            int expected = 98 - remainder;
            if (checksum != expected)
            {
                Console.WriteLine($"Invalid checksum {expected} for account number: {accountNumber}");
                return false;
            }

            return true;
        }
    }
}
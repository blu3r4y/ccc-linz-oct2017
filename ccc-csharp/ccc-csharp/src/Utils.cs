using System;
using System.Collections.Generic;
using System.IO;

namespace CCC
{
    public class Utils
    {
        public static Input Singleton;

        public class Input
        {
            public List<Account> Accounts;
            public List<Transaction> Transactions;

            public Input(List<Account> accounts, List<Transaction> transactions)
            {
                Accounts = accounts;
                Transactions = transactions;
            }
        }

        public static Input Read(string path)
        {
            int i = 0;
            string[] totalLines = File.ReadAllLines(path);

            int numAccounts = int.Parse(totalLines[i]);
            i++;

            // read accounts
            var accounts = new List<Account>();
            Console.WriteLine($"Reading {numAccounts} transactions ...");
            for (int j = i; j < i + numAccounts; j++)
            {
                string line = totalLines[j];
                string[] splitted = line.Split(' ');

                string personName = splitted[0];
                string accountNumber = splitted[1];
                int actualAccountBalance = int.Parse(splitted[2]);
                int overdraftLimit = int.Parse(splitted[3]);

                accounts.Add(new Account(personName, accountNumber, actualAccountBalance, overdraftLimit));
            }

            i += numAccounts;

            int numTransactions = int.Parse(totalLines[i]);
            i++;


            // read transactions
            var transactions = new List<Transaction>();
            Console.WriteLine($"Reading {numTransactions} transactions ...");
            for (int j = i; j < i + numTransactions; j++)
            {
                string line = totalLines[j];
                string[] splitted = line.Split(' ');

                string accountNumberFrom = splitted[0];
                string accountNumberTo = splitted[1];
                int amount = int.Parse(splitted[2]);
                long transactionSubmitTime = long.Parse(splitted[3]);

                Account personNameFromAcc = accounts.Find(l => l.AccountNumber == accountNumberFrom);
                Account personNameToAcc = accounts.Find(l => l.AccountNumber == accountNumberTo);

                transactions.Add(new Transaction(personNameFromAcc, personNameToAcc,
                    amount, transactionSubmitTime));
            }

            Singleton = new Input(accounts, transactions);
            return Singleton;
        }
    }
}
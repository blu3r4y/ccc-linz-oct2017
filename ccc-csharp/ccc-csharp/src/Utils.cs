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
            public List<Transaction> Transactions;

            public Input(List<Transaction> transactions)
            {
                Transactions = transactions;
            }
        }

        public static Input Read(string path)
        {
            int i = 0;
            string[] totalLines = File.ReadAllLines(path);

            int numTransactions = int.Parse(totalLines[i]);
            i++;

            // read transactions
            var transactions = new List<Transaction>();
            Console.WriteLine($"Reading {numTransactions} transactions ...");
            for (int j = i; j < i + numTransactions; j++)
            {
                var inputElements = new List<InputElement>();
                var outputElements = new List<OutputElement>();
                    
                string line = totalLines[j];
                string[] splitted = line.Split(' ');

                string transactionId = splitted[0];
                int numInputs = int.Parse(splitted[1]);
                int ii = 2;

                if (transactionId == "0x00000095")
                {
                    int x = 6;
                }


                for (int jj = ii; jj < ii + numInputs * 3; jj = jj + 3)
                {
                    string inputId = splitted[jj];
                    string inputOwner = splitted[jj + 1];
                    int inputAmount = int.Parse(splitted[jj + 2]);
                    
                    inputElements.Add(new InputElement(inputId, inputOwner, inputAmount));
                }
                ii += numInputs * 3;
                
                int numOutputs = int.Parse(splitted[ii]);
                ii++;
                
                for (int jj = ii; jj < ii + numOutputs * 2; jj = jj + 2)
                {
                    string outputOwner = splitted[jj];
                    int outputAmount = int.Parse(splitted[jj + 1]);
                    outputElements.Add(new OutputElement(outputOwner, outputAmount));
                }
                ii += numOutputs * 2;
                
                long transactionSubmitTime = long.Parse(splitted[ii]);

                transactions.Add(new Transaction(transactionId, transactionSubmitTime, inputElements, outputElements, line));
            }

            Blockchain.AllTransactions = transactions;
            Singleton = new Input(transactions);
            return Singleton;
        }
    }
}
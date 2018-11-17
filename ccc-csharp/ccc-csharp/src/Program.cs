using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MathNet.Numerics.Interpolation;
using MoreLinq;

namespace CCC
{
    internal static class Program
    {
        public static string DataPath = @"C:\data\Dropbox\Projekte\Code\CCC_Linz17_Fall\data\";
        public static string LevelPath = Path.Combine(DataPath, @"level4\");

        public static void Main(string[] args)
        {
            Console.WriteLine(doLevel1("level4-eg.txt"));

            for (int i = 1; i <= 4; i++)
            {
                // Console.WriteLine(doLevel1($"level4-{i}.txt"));
            }

            Console.Read();
        }

        static int doLevel1(string fileName)
        {
            Blockchain.Valid.Clear();
            Blockchain.AllTransactions.Clear();
            
            Utils.Input data = Utils.Read(Path.Combine(LevelPath, fileName));

            var transactions = data.Transactions;
            var requests = data.Requests;
            
            // order by submission time
            transactions = transactions.OrderBy(t => t.Timestamp).ToList();
            requests = requests.OrderBy(r => r.Timestamp).ToList();

            // initial clean up
            checkChain(transactions);
            
            foreach (var request in requests)
            {
                Transaction newTransaction = null;
                foreach (Transaction transaction in Blockchain.Valid)
                {
                    if (transaction.IsValid())
                    {
                        // transaction.Execute();
                        // search if owner has enough funds
                        OutputElement element = transaction.Outputs.Find(o => o.Owner == request.FromOwner);
                        if (element != null)
                        {
                            if (element.Amount >= request.Amount)
                            {
                                newTransaction = new Transaction(request.TransactionId, request.Timestamp,
                                    new List<InputElement>
                                    {
                                        new InputElement(transaction.Id, request.FromOwner, element.Amount)
                                    },
                                    new List<OutputElement>
                                    {
                                        new OutputElement(request.ToOwner, request.Amount), // amount
                                        new OutputElement(request.FromOwner,
                                            element.Amount - request.Amount) // remainder
                                    });
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("transaction should be valid.");
                    }
                }
                
                if (newTransaction != null && newTransaction.IsValid())
                {
                    Console.WriteLine("executing new transaction");
                    newTransaction.Execute();
                    break;
                }
                else
                {
                    throw new Exception("dsaasd");
                }
            }

            checkChain(transactions, true, fileName);
            return 0;
        }

        static void checkChain(List<Transaction> chain, bool print = false, string fileName = null)
        {
            var output = new StringBuilder();
            int valid = 0;
            
            foreach (Transaction transaction in chain)
            {
                if (transaction.IsValid())
                {
                    transaction.Execute();
                    output.AppendLine(transaction.RawString);
                    valid++;
                }
            }
            
            if (print)
            {
                // prepend number of valid transactions
                output.Insert(0, valid.ToString() + "\n");
                
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine(output);
                File.WriteAllText(Path.Combine(LevelPath, fileName + ".out.txt"), output.ToString());
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MoreLinq;

namespace CCC
{
    internal static class Program
    {
        public static string DataPath = @"C:\data\Dropbox\Projekte\Code\CCC_Linz17_Fall\data\";
        public static string LevelPath = Path.Combine(DataPath, @"level3\");

        public static void Main(string[] args)
        {
            Console.WriteLine(doLevel1("level3-eg.txt"));

            for (int i = 1; i <= 4; i++)
            {
                Console.WriteLine(doLevel1($"level3-{i}.txt"));
            }

            Console.Read();
        }

        static int doLevel1(string fileName)
        {
            Blockchain.Valid.Clear();
            Blockchain.AllTransactions.Clear();
            
            var output = new StringBuilder();
            Utils.Input data = Utils.Read(Path.Combine(LevelPath, fileName));

            var transactions = data.Transactions;

            // order by submission time
            transactions = transactions.OrderBy(t => t.Timestamp).ToList();

            int valid = 0;
            foreach (Transaction transaction in transactions)
            {
                if (transaction.IsValid())
                {
                    transaction.Execute();
                    output.AppendLine(transaction.RawString);
                    valid++;
                }
            }
            
            // prepend number of valid transactions
            output.Insert(0, valid.ToString() + "\n");

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(output);
            File.WriteAllText(Path.Combine(LevelPath, fileName + ".out.txt"), output.ToString());
            return 0;
        }
    }
}
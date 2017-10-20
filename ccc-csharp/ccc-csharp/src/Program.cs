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
        public static string LevelPath = Path.Combine(DataPath, @"level1\");

        public static void Main(string[] args)
        {
            Console.WriteLine(doLevel1("level1-eg.txt"));

            for (int i = 1; i <= 4; i++)
            {
                Console.WriteLine(doLevel1($"level1-{i}.txt"));
            }

            Console.Read();
        }

        static int doLevel1(string fileName)
        {
            var output = new StringBuilder();
            Utils.Input data = Utils.Read(Path.Combine(LevelPath, fileName));

            var accounts = data.Accounts;
            var transactions = data.Transactions;

            // order by submission time
            transactions = transactions.OrderBy(t => t.TransactionSubmitTime).ToList();

            foreach (Transaction transaction in transactions)
            {
                transaction.Execute();
            }

            output.AppendLine(accounts.Count.ToString());
            foreach (Account account in accounts)
            {
                output.AppendLine(string.Format("{0} {1}", account.Name, account.Balance));
            }

            File.WriteAllText(Path.Combine(LevelPath, fileName + ".out.txt"), output.ToString());
            return 0;
        }
    }
}
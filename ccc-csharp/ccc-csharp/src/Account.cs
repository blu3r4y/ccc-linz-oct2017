using System.Diagnostics;
using System.Security.Cryptography;

namespace CCC
{
    [DebuggerDisplay("{Name} {Balance}")]
    public class Account
    {
        public string Name { get; set; }
        public int Balance { get; set; }

        public Account(string name, int balance)
        {
            Name = name;
            Balance = balance;
        }
    }
}
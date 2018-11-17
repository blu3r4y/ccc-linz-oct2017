using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CCC
{
    [DebuggerDisplay("#{Id} {Timestamp} : {Inputs} -> {Outputs}")]
    public class Transaction
    {
        public string Id { get; set; }
        public long Timestamp { get; set; }
        public List<InputElement> Inputs { get; set; }
        public List<OutputElement> Outputs { get; set; }

        public string RawString { get; set; }

        public Transaction(string id, long timestamp, List<InputElement> inputs, List<OutputElement> outputs,
            string rawString = null)
        {
            Id = id;
            Timestamp = timestamp;
            Inputs = inputs;
            Outputs = outputs;
            RawString = rawString;
        }

        public bool IsValid()
        {
            Console.WriteLine($"Checking #{this.Id} ...");

            // sum of input amounts == sum of output amounts
            int inputSum = Inputs.Sum(i => i.Amount);
            int outputSum = Outputs.Sum(i => i.Amount);
            if (inputSum != outputSum)
            {
                Console.WriteLine($"sum error {inputSum} != {outputSum}");
                return false;
            }

            // input must be output of prev trans
            foreach (InputElement input in Inputs)
            {
                // TODO: mentioned in file
                if (!Blockchain.Valid.Select(e => e.Id).Contains(input.Id)
                    && input.Owner != "origin")
                {
                    Console.WriteLine($"#{input.Id} not in blockchain and {input.Owner} != origin");
                    return false;
                }
            }

            // owner listed once
            if (Outputs.GroupBy(n => n.Owner).Any(c => c.Count() > 1))
            {
                Console.WriteLine("duplicate owner found");
                return false;
            }

            // spend completely -> input elements must be output elements of previous transactions with the exact same amount
            foreach (InputElement input in Inputs)
            {
                if (input.Owner != "origin")
                {
                    var prevTrans = Blockchain.Valid.Find(t => t.Id == input.Id);
                    var prevOutput = prevTrans.Outputs.Find(o => o.Owner == input.Owner);

                    if (prevOutput.Amount != input.Amount)
                    {
                        Console.WriteLine(
                            $"prev transaction #{prevTrans.Id} with output {prevOutput.Amount} $ from {prevOutput.Owner} != {input.Amount} of #{this.Id}");
                        return false;
                    }
                }
            }

            // amount > 0
            foreach (InputElement input in Inputs)
            {
                if (input.Amount <= 0)
                {
                    Console.WriteLine($"input element #{input.Id} amount {input.Amount} <= 0");
                    return false;
                }
            }
            foreach (OutputElement output in Outputs)
            {
                if (output.Amount <= 0)
                {
                    Console.WriteLine($"output element from {output.Owner} amount {output.Amount} <= 0");
                    return false;
                }
            }
            
            // distinct input ids
            if (Inputs.GroupBy(n => n.Id).Any(c => c.Count() > 1))
            {
                // TODO: never the case
                Console.WriteLine("duplicate input ids found");
                return false;
            }


            // input must not have been consumed yet
            foreach (InputElement input in Inputs)
            {
                if (Blockchain.Valid.SelectMany(t => t.Inputs.Select(i => i.Id)).Contains(input.Id))
                {
                    Console.WriteLine($"input #{input.Id} already consumed");
                    //return false;
                }
            }
            
            return true;
        }

        public void Execute()
        {
            Blockchain.Valid.Add(this);
        }
    }

    [DebuggerDisplay("{Owner} {Amount}")]
    public class OutputElement
    {
        public string Owner { get; set; }
        public int Amount { get; set; }

        public OutputElement(string owner, int amount)
        {
            Owner = owner;
            Amount = amount;
        }
    }

    [DebuggerDisplay("#{Id} {Owner} {Amount}")]
    public class InputElement
    {
        public string Id { get; set; }
        public string Owner { get; set; }
        public int Amount { get; set; }

        public InputElement(string id, string owner, int amount)
        {
            Id = id;
            Owner = owner;
            Amount = amount;
        }
    }
}
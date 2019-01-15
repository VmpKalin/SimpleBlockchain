using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Blockchain
{
    public sealed class Block
    {
        public DateTime CreatingTime { get; set; }
        public object Data { get; set; }
        public string Hash { get; set; }
        public string PreviousHash { get; set; }
        public int Nonce { get; set; }
        public List<Transaction> Transactions { get; set; }

        public Block(DateTime creatingTime, object data, List<Transaction> transactions, string previousHash = "")
        {
            CreatingTime = creatingTime;
            Data = data;
            Transactions = transactions;
            Hash = CalculateHash();
            PreviousHash = previousHash;
        }

        public void MineBlock(int difficulty)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Start mining new block");
            var strToCmp = new string('0', difficulty);
            while (!Hash.StartsWith(strToCmp))
            {
                Nonce++;
                Hash = CalculateHash();
            }

            Console.WriteLine("Block has been mined");
            Console.ResetColor();
        }

        public string CalculateHash() => HashUtil.Sha256(CreatingTime.ToString(CultureInfo.InvariantCulture),
            JsonConvert.SerializeObject(Data), PreviousHash, Nonce.ToString());
    }
}

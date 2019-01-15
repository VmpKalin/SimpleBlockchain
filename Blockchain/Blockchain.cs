using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Newtonsoft.Json;

namespace Blockchain
{
    public sealed class Blockchain
    {
        private readonly List<Block> _chain;
        private const int Difficulty = 3;
        private const decimal Reward = 10;
        private List<Transaction> _pendingTransactions;

        public Blockchain()
        {
            _pendingTransactions = new List<Transaction>();
            _chain = new List<Block> {CreateGenesisBlock()};
        }

        public Block CreateGenesisBlock()
        {
            return new Block(DateTime.UtcNow, new
            {
                Value = 10
            }, _pendingTransactions);
        }

        public Block GetLatestBlock() => _chain[_chain.Count - 1];

        public void MinePendingTransactions(string mineRewardAddress)
        {
            var block = new Block(DateTime.UtcNow, new { data = "My wallet" }, _pendingTransactions, GetLatestBlock().Hash);
            block.MineBlock(Difficulty);
            AddBlock(block);    
            _pendingTransactions = new List<Transaction>
            {
                (new Transaction(null, mineRewardAddress, Reward))
            };
        }

        public decimal GetBalanceOfAddress(string address)
        {
            decimal balance = 0;
            foreach (var block in _chain)
            {
                foreach (var trans in block.Transactions)
                {
                    if (trans.FromAddress == address)
                        balance -= trans.Amount;
                    if (trans.ToAddress == address)
                        balance += trans.Amount;
                }
            }
            return balance;
        }

        public void CreateTransaction(Transaction transaction)
        {
            _pendingTransactions.Add(transaction);
        }

        public void AddBlock(Block block)
        {
            if (!IsChainValid())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Chain is not valid. Break operation!");
                Console.ResetColor();
                return;
            }

            block.Transactions = _pendingTransactions;
            block.PreviousHash = GetLatestBlock().Hash;
            block.MineBlock(Difficulty);
            _chain.Add(block);
            Console.WriteLine("Added new block to chain");
        }

        public bool IsChainValid()
        {
            for (var i = 1; i < _chain.Count; i++)
            {
                var currentBlock = _chain[i];
                var previousBlock = _chain[i-1];

                if (!currentBlock.PreviousHash.Equals(previousBlock.Hash))
                    return false;

                if (!currentBlock.PreviousHash.Equals(previousBlock.CalculateHash()))
                    return false;
            }
            return true;
        }

        public void OutputChain()
        {
            Console.WriteLine("\nChain data:");
            Console.ForegroundColor = ConsoleColor.Yellow;
            foreach (var block in _chain)
            {
                Console.WriteLine($"Creation time: {block.CreatingTime}");
                Console.WriteLine($"Creation hash: {block.Hash}");
                Console.WriteLine($"Creation previous hash: {block.PreviousHash}");
                Console.WriteLine($"Creation data: {JsonConvert.SerializeObject(block.Data)}");
                Console.WriteLine("\n");
            }
            Console.ResetColor();
        } 

        public void ChainTesting()
        {
            OutputChain();
            Console.WriteLine("Add new blocks");
            //AddBlock(new Block(DateTime.UtcNow, new { Value = 299 }, _pendingTransactions));
            //AddBlock(new Block(DateTime.UtcNow, new { Value = 20 }, _pendingTransactions));
            Console.WriteLine($"Is chain valid? {IsChainValid()}");
            OutputChain();
            CreateTransaction(new Transaction("Bitcoin", "VmpAddress", 100));
            MinePendingTransactions("VmpAddress");
            Console.WriteLine($"Balance VmpAddress: {GetBalanceOfAddress("VmpAddress")}");
            Console.WriteLine($"Balance Alina: {GetBalanceOfAddress("Alina")}"); CreateTransaction(new Transaction("VmpAddress", "Alina", 5));
            MinePendingTransactions("Alina");
            Console.WriteLine($"Balance VmpAddress: {GetBalanceOfAddress("VmpAddress")}");
            Console.WriteLine($"Balance Alina: {GetBalanceOfAddress("Alina")}");
            Console.WriteLine("Change data illegally");
            //_chain[1].Data = new { Volume = 10 };
            Console.WriteLine($"Is chain valid? {IsChainValid()}");
            OutputChain();
        }
    }
}

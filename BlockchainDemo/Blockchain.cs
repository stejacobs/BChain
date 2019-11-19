using System;
using System.Collections.Generic;
using System.Text;

namespace BCTestDemo
{
    public class Blockchain
    {
        public IList<Transaction> PendingTransactions = new List<Transaction>();
        public IList<Block> Chain { set;  get; }
        public int Difficulty { set; get; } = 2;
        public int CurrentPropTaxOwed = 0; //not currently used 

        public Blockchain()
        {
         //   
        }


        public void InitializeChain()
        {
            Chain = new List<Block>();
            AddGenesisBlock();
        }

        public Block CreateGenesisBlock()
        {
            Block block = new Block(DateTime.Now, null, PendingTransactions);
            block.Mine(Difficulty);
            PendingTransactions = new List<Transaction>();
            return block;
        }

        public void AddGenesisBlock()
        {
            Chain.Add(CreateGenesisBlock());
        }
        
        public Block GetLatestBlock()
        {
            return Chain[Chain.Count - 1];
        }

        public void CreateTransaction(Transaction transaction)
        {
            PendingTransactions.Add(transaction);
        }
        public void ProcessPendingTransactions(string senderAddress, string minerAddress, string ownerName)
        {
            Block block = new Block(DateTime.Now, GetLatestBlock().Hash, PendingTransactions);
            AddBlock(block);

            PendingTransactions = new List<Transaction>();
            CreateTransaction(new Transaction(senderAddress, minerAddress, CurrentPropTaxOwed, ownerName));



        }

        public void AddBlock(Block block)
        {
            Block latestBlock = GetLatestBlock();
            block.Index = latestBlock.Index + 1;
            block.PreviousHash = latestBlock.Hash;
            //block.Hash = block.CalculateHash(); <-- Not being calculated here but could be
            block.Mine(this.Difficulty);
            Chain.Add(block);
            
        }

        public bool IsValid()
        {
            for (int i = 1; i < Chain.Count; i++)
            {
                Block currentBlock = Chain[i];
                Block previousBlock = Chain[i - 1];

                if (currentBlock.Hash != currentBlock.CalculateHash())
                {
                    return false;
                }

                if (currentBlock.PreviousHash != previousBlock.Hash)
                {
                    return false;
                }
            }
            return true;
        }

        public int GetBalance(string address)
        {
            int balance = 0;
                for (int i = 0; i < Chain.Count; i++)
                {
                    for (int j = 0; j < Chain[i].Transactions.Count; j++)
                    {
                       
                        var transaction = Chain[i].Transactions[j];
                    

                        if (transaction.FromAddress == address && transaction.ToAddress == address)
                        {
                            balance += transaction.Amount;
                        }
                        else
                        {

                            if (transaction.FromAddress == address)
                            {
                                balance -= transaction.Amount;
                            }

                            if (transaction.ToAddress == address)
                            {
                                balance += transaction.Amount;
                            }
                        }

                    }
            }

            return balance;
        }
    }
}

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Transactions;

namespace BCTestDemo
{
    class Program
    {
        public static int Port = 0;
        public static P2PServer Server = null;
        public static P2PClient Client = new P2PClient();
        public static Blockchain BCDemo = new Blockchain();
        public static string name = "unknown";

        static void Main(string[] args)
        {
            BCDemo.InitializeChain();

            if (args.Length >= 1)
                Port = int.Parse(args[0]);
            if (args.Length >= 2)
                name = args[1];

            if (Port > 0)
            {
                Server = new P2PServer();
                Server.Start();
            }

            if (name != "unkown")
            {
                Console.WriteLine($"Current user is {name}");
            }

           
            int selection = 0;
            while (selection != 4)
            {
                switch (selection)
                {
                    case 1:
                        Console.WriteLine("Please enter the server URL (enter 0 to cancel the operation)");
                        string serverURL = Console.ReadLine();
                        if (serverURL == "0")
                            break;
                        Client.Connect($"{serverURL}/bcdemo");
                        break;
                    case 2:
                        Console.WriteLine("Please enter the office name (enter 0 to cancel the operation)");
                        string receiverName = Console.ReadLine();
                        if (receiverName == "0")
                            break;

                        Console.WriteLine("Please enter the property owner name (enter 0 to cancel the operation)");
                        string ownerName = Console.ReadLine();
                        if (ownerName == "0")
                            break;
                        
                        Console.WriteLine("Please enter the property tax amount (enter 0 to cancel the operation)");
                        string amount = Console.ReadLine();
                        if (amount == "0")
                            break;

                        BCDemo.CreateTransaction(new Transaction(name, receiverName, int.Parse(amount), ownerName));
                        BCDemo.ProcessPendingTransactions(name, receiverName, ownerName);

                        int prime = BCDemo.GetBalance(name);
                        int recv = BCDemo.GetBalance(receiverName);

                        if (prime < 0)
                        {
                            Console.WriteLine(name + " negative balance:  " + prime);
                        }
                        else
                        {
                            Console.WriteLine(name + " balance:  " + prime);
                           
                        }

                        Console.WriteLine(receiverName + " balance:  " + recv);

                        Client.Broadcast(JsonConvert.SerializeObject(BCDemo));
                        break;
                    case 3:
                        Console.WriteLine("Blockchain");
                        var obj = JsonConvert.SerializeObject(BCDemo, Formatting.Indented);
                        Console.WriteLine(obj);
                        string filename = @"c:\test\" + name + "-" + DateTime.Now.ToString("mmddyyyyhhmmss") + ".json";
                        File.WriteAllText(filename, JsonConvert.SerializeObject(BCDemo, Formatting.Indented));
                        
                        JournalOutput(obj);
                       
                        break;

                }

                Console.WriteLine(Environment.NewLine + "<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<");
                Console.WriteLine("1. Join the network");
                Console.WriteLine("2. Add a transaction");
                Console.WriteLine("3. Display Current Blockchain");
                Console.WriteLine("4. Exit");
                Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>" + Environment.NewLine);
                Console.WriteLine("Please select an option....");
                string action = Console.ReadLine();
                selection = int.Parse(action);
            }

            if (Client != null)
            {
                try
                {
                    Client.Close();
                }
                finally
                {
                    Client = null;
                }
            }
        }

        private static void JournalOutput(string obj)
        {
            Console.WriteLine(Environment.NewLine + "Journal Output");

            RootObject ro = JsonConvert.DeserializeObject<RootObject>(obj);
            foreach (var cn in ro.Chain)
            {
                foreach (var trn in cn.Transactions)
                {
                    //var result = trn.transid + "," + trn.FromAddress + "," + trn.ToAddress + "," + trn.OwnerName + "," + trn.Amount;
                    Console.WriteLine("\n{0}", trn.transid + "," + trn.FromAddress + "," +
                                               trn.ToAddress + "," + trn.OwnerName + "," + trn.Amount);
                }
            }
        }
    }
}

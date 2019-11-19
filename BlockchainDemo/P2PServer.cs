using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Text;
using WebSocketSharp;
using WebSocketSharp.Net;
using WebSocketSharp.Server;

namespace BCTestDemo
{
    public class P2PServer: WebSocketBehavior
    {
        bool chainSynched = false;
        WebSocketServer wss = null;

        public void Start()
        {
            wss = new WebSocketServer($"ws://127.0.0.1:{Program.Port}");
            //wss.SslConfiguration.EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12;
            wss.AddWebSocketService<P2PServer>("/bcdemo");
            wss.Start();
            Console.WriteLine($"Started server at ws://127.0.0.1:{Program.Port}");
            
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            
            if (e.Data.Contains("Hi Server"))
            {
                Console.WriteLine(e.Data);
                Send($"From {Program.Port}: Hi Client");
            }
            else
            {
                Blockchain newChain = JsonConvert.DeserializeObject<Blockchain>(e.Data);

                if (newChain.IsValid() && newChain.Chain.Count > Program.BCDemo.Chain.Count)
                {
                    Program.BCDemo.Chain = newChain.Chain;
                }

                if (!chainSynched)
                {
                    Send(JsonConvert.SerializeObject(Program.BCDemo));
                    chainSynched = true;
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using static KitchenPr.Server;

namespace KitchenPr
{
    public class Threads
    { 
        
        public Mutex mut = new Mutex();
        
        public void Extractor()
        {
            //_queue = server.Data;
            while (true)
            {
                string order = string.Empty;
                mut.WaitOne(); 
                if (Que.Queue.Count > 0)
                {
                    order = Que.Queue.Dequeue();
                    //Console.WriteLine(order);
                }
                mut.ReleaseMutex();
                if(order == String.Empty) continue;
                using var client = new HttpClient();
                var data = new StringContent(order, Encoding.UTF8, "application/json");
                client.PostAsync("http://localhost:8100/kitchen", data);
                Thread.Sleep(1000);

            }
        }
        public List<Thread> ExtractThreads()
        {
            int thread_extractors = 4;
            List<Thread> list = new List<Thread>();
            for (int i = 0; i < thread_extractors; i++)
            {
                Thread thread = new Thread(Extractor);
                list.Add(thread);
            }

            return list;
        }
    }
}
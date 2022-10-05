using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using static KitchenPr.Server;

namespace KitchenPr
{
    public class Threads
    {
        private Queue<string> Order = JsonObjects;
        public Mutex mut = new Mutex();
        public void Extractor()
        {
            
            string order = string.Empty;
            //Thread.Sleep(1000);
            while (true)
            {
               
                if (Order.Count > 0)
                {
                    mut.WaitOne();
                    order = Order.Dequeue();
                    mut.ReleaseMutex();
                    
                }

                using var client = new HttpClient();
                var data = new StringContent(order, Encoding.UTF8, "application/json");
                client.PostAsync("http://localhost:8080/", data);
                Thread.Sleep(1000);
                
            }
        }
        
        public List<Thread> ExtractThreads()
        {
            int thread_extractors = 2;
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
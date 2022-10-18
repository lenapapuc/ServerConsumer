using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static KitchenPr.Client;

namespace KitchenPr
{
    public class Server
    {
       // public static Queue<string> Data;
        public int Port = 8090;

        private HttpListener _listener;
       // private Queue<string> Queue = new Queue<string>();
        public void Start()
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add("http://localhost:" + Port.ToString() + "/");
            _listener.Start();
            Receive();
        }

        public void Stop()
        {
            _listener.Stop();
        }

        private void Receive()
        {
            _listener.BeginGetContext(new AsyncCallback(ListenerCallback), _listener);
        }

        private void ListenerCallback(IAsyncResult result)
        {
            if (_listener.IsListening)
            {
                var context = _listener.EndGetContext(result);
                var request = context.Request;

                // do something with the request
                Console.WriteLine($"{request.HttpMethod} {request.Url}");
                

                if (request.HasEntityBody)
                {
                    var body = request.InputStream;
                    var encoding = request.ContentEncoding;
                    var reader = new StreamReader(body, encoding);
                    if (request.ContentType != null)
                    {
                        Console.WriteLine("Client data content type {0}", request.ContentType);
                    }
                    Console.WriteLine("Client data content length {0}", request.ContentLength64);

                    Console.WriteLine("Start of data:");
                    string st = reader.ReadToEnd();
                    string s = st;
                    Console.WriteLine(st);
                    if (st.Length != null || st.Length != 0)
                    Queue.Enqueue(st);
                    //Console.WriteLine(st);
                    Console.WriteLine("End of data:");
                    reader.Close();
                    body.Close();
                }

                Receive();
            }
        }
    
    }
}
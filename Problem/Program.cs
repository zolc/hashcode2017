using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Problem
{
    class Program
    {
        //private static int numOfvideos;
        //private static int numOfEndpoints;
        public int maxSize;
        public List<Cache> allCaches=new List<Cache>();
        public List<Endpoint> allEndpoints=new List<Endpoint>();
        public List<Video> allVideos= new List<Video>();
        //public List<Request> allRequests;
        static void Main(string[] args)
        {
            Program p = new Program();
            p.Parse();
           
        }

        void Parse()
        {
            int i = 0;
            string filename = Console.ReadLine();
            string input = File.ReadAllText(filename);
            //pierwsza linijka odjęta od input
            string line = input.Substring(0,input.IndexOf('\n'));
            input = input.Remove(0,line.Length + 1);
            string[] vals = line.Split(' ');
            int numOfEndpoints = int.Parse(vals[1]);
            int numOfRequests = int.Parse(vals[2]);
            int numOfCaches = int.Parse(vals[3]);
            int maxSize = int.Parse(vals[vals.Count() - 1]);
            //dodajemy wszystkie cache
            for (; i < numOfCaches; i++)
            {
                allCaches.Add(new Cache(maxSize, i));
            }
            //dodajemy wszystkie video
            line = input.Substring(0,input.IndexOf('\n'));
            input = input.Remove(0,line.Length + 1);
            vals = line.Split(' ');
            i = 0;
            foreach(var x in vals)
            {
                allVideos.Add(new Video(i++,int.Parse(x)));
            }

            //dodajemy wszystkie endpointy
            int endpts = 0;
            foreach (var row in input.Split('\n'))
            {
                while(endpts < numOfEndpoints)
                {
                    line = input.Substring(0,input.IndexOf('\n'));
                    input = input.Remove(0,line.Length + 1);
                    vals = line.Split(' ');
                    int dLatency = int.Parse(vals[0]);
                    int cachesNum= int.Parse(vals[1]);
                    Endpoint e = new Endpoint(dLatency);
                    for(int q=0; q< cachesNum; q++)
                    {
                        line = input.Substring(0,input.IndexOf('\n'));
                        input = input.Remove(0,line.Length + 1);
                        vals = line.Split(' ');
                        //dodanie cache'a do endpointa
                        e.latencies.Add(allCaches[int.Parse(vals[0])],int.Parse(vals[1]));
                        //dodanie endpointa do cache'a
                        allCaches[int.Parse(vals[0])].endpoints.Add(e);
                       
                    }
                    allEndpoints.Add(e);
                    endpts++;
                }
                //requesty
            }
        }

        void FilterRequests()
        {
            // 1. Usuwamy requesty z endpointów bez cache
            foreach (Endpoint endpoint in allEndpoints)
            {
                if (endpoint.latencies.Count == 0)
                {
                    endpoint.requests.Clear();
                    allEndpoints.Remove(endpoint);
                }
            }

            // 2. Usuwamy requesty na filmy, które przekraczają maxSize
            foreach (Endpoint endpoint in allEndpoints)
            {
                for (int i = 0; i < endpoint.requests.Count; i++)
                {
                    Request req = endpoint.requests[i];
                    if (req.video.size > maxSize)
                        endpoint.requests.Remove(req);
                }
            }
        }
        

    }
}

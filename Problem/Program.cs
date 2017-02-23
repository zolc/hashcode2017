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
        public List<Cache> allCaches;
        public List<Endpoint> allEndpoints;
        public List<Video> allVideos;
        //public List<Request> allRequests;
        static void Main(string[] args)
        {

        }

        void Parse()
        {
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
            //druga linijka odjęta od input
            line = input.Substring(0,input.IndexOf('\n'));
            input = input.Remove(0,line.Length + 1);
            vals = line.Split(' ');
            int i = 0;
            foreach(var x in vals)
            {
                allVideos.Add(new Video(i++,int.Parse(x)));
            }
            int endpts = 0;
            foreach (var row in input.Split('\n'))
            {
                while(endpts < numOfEndpoints)
                {
                    line = input.Substring(0,input.IndexOf('\n'));
                    input = input.Remove(0,line.Length + 1);
                    vals = line.Split(' ');
                    int dLatency = int.Parse(vals[0]);
                    int numOfCaches = int.Parse(vals[1]);

                    for(int q=0; q<numOfCaches; q++)
                    {

                    }


                    allEndpoints.Add(new Endpoint(, ))


                    endpts++;
                }
                
                i++;
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

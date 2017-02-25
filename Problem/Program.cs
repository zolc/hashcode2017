using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

enum ParseMethod
{
    ParseFromFile,
    ParseFromConsole
}

namespace Problem
{
    public class Program
    {
        // If the input is redirected, then read straight from the console
        // Otherwise ask for the filename
        ParseMethod parseMethod = Console.IsInputRedirected ? ParseMethod.ParseFromConsole : ParseMethod.ParseFromFile;

        public int maxSize;
        public List<Cache> allCaches = new List<Cache>();
        public List<Endpoint> allEndpoints = new List<Endpoint>();
        public List<Video> allVideos = new List<Video>();

        static void Main(string[] args)
        {
            Program p = new Program();
            p.ParseData();
            p.FilterRequests();
            foreach (var cache in p.SortCaches())
                cache.EvaluateAndFillWithVideos();
            p.OutputResults();
        }

        void ParseData()
        {
            string[] inputLines = null;
            if (parseMethod == ParseMethod.ParseFromFile)
            {
                string filename = Console.ReadLine();
                inputLines = this.ReadDataFromFile(filename);
            }
            else if (parseMethod == ParseMethod.ParseFromConsole)
                inputLines = this.ReadDataFromConsole();

            this.ParseDataFromLines(inputLines);
        }

        void FilterRequests()
        {
            // 1. Usuwamy requesty z endpointów bez cache
            List<Endpoint> endpointsToRemove = new List<Endpoint>();
            foreach (Endpoint endpoint in allEndpoints)
            {
                if (endpoint.latencies.Count == 0)
                    endpointsToRemove.Add(endpoint);
            }
            for (int i = 0; i < endpointsToRemove.Count; i++)
            {
                allEndpoints.Remove(endpointsToRemove[i]);
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

        IOrderedEnumerable<Cache> SortCaches()
        {
            return allCaches.OrderByDescending(item => item.endpointsConnected.Count);//.OrderBy(item => item.Key.showups)
        }

        void OutputResults()
        {
            List<Cache> usedCaches = new List<Cache>();
            foreach (var cache in allCaches)
            {
                if (cache.IsUsed())
                    usedCaches.Add(cache);
            }

            Console.WriteLine(usedCaches.Count);

            foreach (var cache in usedCaches)
                Console.WriteLine(cache);
        }

    }
}

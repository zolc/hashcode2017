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

        bool parallelCacheEvaluation = false;

        public int maxSize;
        public List<Cache> allCaches = new List<Cache>();
        public List<Endpoint> allEndpoints = new List<Endpoint>();
        public List<Video> allVideos = new List<Video>();

        static void Main(string[] args)
        {
            Program p = new Program();

            p.ParseData();
            p.FilterRequests();
            p.EvaluateAndFillCaches();
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

            var watch = System.Diagnostics.Stopwatch.StartNew();
            this.ParseDataFromLines(inputLines);
            watch.Stop();
            ProgramExtender.LogPerformanceMetric("parsing input data", watch);
        }

        void FilterRequests()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            // 1. Usuwamy requesty z endpointów bez cache
            List<Endpoint> endpointsToRemove = new List<Endpoint>();
            foreach (Endpoint endpoint in allEndpoints)
            {
                if (endpoint.latenciesToCaches.Count == 0)
                    endpointsToRemove.Add(endpoint);
            }
            for (int i = 0; i < endpointsToRemove.Count; i++)
                allEndpoints.Remove(endpointsToRemove[i]);

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

            watch.Stop();
            ProgramExtender.LogPerformanceMetric("filtering requests", watch);
        }

        void EvaluateAndFillCaches()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            if (parallelCacheEvaluation)
                Parallel.ForEach(SortCaches(), cache => cache.EvaluateAndFillWithVideos());
            else
            {
                foreach (var cache in SortCaches())
                    cache.EvaluateAndFillWithVideos();
            }

            watch.Stop();
            ProgramExtender.LogPerformanceMetric("evaluating and filling caches", watch);
        }

        IOrderedEnumerable<Cache> SortCaches()
        {
            return allCaches.OrderByDescending(item => item.endpointsConnected.Count);//.OrderBy(item => item.Key.showups)
        }

        void OutputResults()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            List<Cache> usedCaches = new List<Cache>();
            foreach (var cache in allCaches)
            {
                if (cache.IsUsed())
                    usedCaches.Add(cache);
            }

            Console.WriteLine(usedCaches.Count);

            foreach (var cache in usedCaches)
                Console.WriteLine(cache);

            watch.Stop();
            ProgramExtender.LogPerformanceMetric("printing results", watch);
        }

    }
}

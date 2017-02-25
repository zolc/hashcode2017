﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Problem
{
    class Program
    {
        public int maxSize;
        public List<Cache> allCaches = new List<Cache>();
        public List<Endpoint> allEndpoints = new List<Endpoint>();
        public List<Video> allVideos = new List<Video>();

        static void Main(string[] args)
        {
            Program p = new Program();
            try
            {
                p.Parse();
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("File not found");
                return;
            }
            
            p.FilterRequests();
            foreach (var cache in p.SortCaches())
                cache.EvaluateAndFillWithVideos();
            p.OutputResults();
        }

        void Parse()
        {
            string filename = Console.ReadLine();
            string[] fileLines = File.ReadAllLines(filename);

            // Początkowe wartości
            string[] initialNumbers = fileLines[0].Split(' ');
            int numOfVideos = int.Parse(initialNumbers[0]);
            int numOfEndpoints = int.Parse(initialNumbers[1]);
            int numOfRequests = int.Parse(initialNumbers[2]);
            int numOfCaches = int.Parse(initialNumbers[3]);
            maxSize = int.Parse(initialNumbers[4]);

            // dodajemy wszystkie cache
            for (int i = 0; i < numOfCaches; i++)
            {
                allCaches.Add(new Cache(maxSize, i));
            }
            //dodajemy wszystkie video
            string[] videosSizes = fileLines[1].Split(' ');
            for (int i = 0; i < numOfVideos; i++)
                allVideos.Add(new Video(i, int.Parse(videosSizes[i])));

            //dodajemy wszystkie endpointy i łączymy je z cache
            int currentLine = 2;
            for (int i = 0; i < numOfEndpoints; i++)
            {
                string[] endpointInfo = fileLines[currentLine].Split(' ');
                int dataCenterLatency = int.Parse(endpointInfo[0]);
                int endpointCachesAmount = int.Parse(endpointInfo[1]);

                Endpoint endpoint = new Endpoint(dataCenterLatency);
                currentLine++;
                for (int j = 0; j < endpointCachesAmount; j++)
                {
                    string[] cacheServerInfo = fileLines[currentLine++].Split(' ');
                    int cacheID = int.Parse(cacheServerInfo[0]);
                    int cacheLatency = int.Parse(cacheServerInfo[1]);

                    // dodanie Cache do Endpointa
                    endpoint.latencies.Add(allCaches[cacheID], cacheLatency);

                    // dodanie Endpointa do Cachea
                    allCaches[cacheID].endpointsConnected.Add(endpoint);
                }
                allEndpoints.Add(endpoint);
            }

            // dodajemy requesty
            for (int i = 0; i < numOfRequests; i++)
            {
                string[] requestInfo = fileLines[currentLine++].Split(' ');
                int videoID = int.Parse(requestInfo[0]);
                int endpointID = int.Parse(requestInfo[1]);
                int requestDemand = int.Parse(requestInfo[2]); // liczba requestów

                Video video = allVideos[videoID];
                Endpoint endpoint = allEndpoints[endpointID];

                endpoint.requests.Add(new Request(video, requestDemand));
            }
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

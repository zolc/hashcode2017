﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Problem
{
    public static class ProgramExtender
    {
        public static string[] ReadDataFromFile(this Program p, string filename)
        {
            return File.ReadAllLines(filename);
        }

        public static string[] ReadDataFromConsole(this Program p)
        {
            List<string> inputLines = new List<string>();
            bool continueReading = true;

            while (continueReading)
            {
                string inputLine = Console.ReadLine();
                if (inputLine == null)
                    continueReading = false;
                else
                    inputLines.Add(inputLine);
            }
            return inputLines.ToArray();
        }

        public static void ParseDataFromLines(this Program p, string[] inputLines)
        {
            // Initial numbers
            string[] initialNumbers = inputLines[0].Split(' ');
            int numOfVideos = int.Parse(initialNumbers[0]);
            int numOfEndpoints = int.Parse(initialNumbers[1]);
            int numOfRequests = int.Parse(initialNumbers[2]);
            int numOfCaches = int.Parse(initialNumbers[3]);
            p.maxSize = int.Parse(initialNumbers[4]);
            var watch = new System.Diagnostics.Stopwatch();

            // Add all caches
            watch.Start();
            for (int i = 0; i < numOfCaches; i++)
                p.allCaches.Add(new Cache(p.maxSize, i));
            watch.Stop();
            LogPerformanceMetric("creating caches", watch);

            // Add all videos
            watch.Restart();
            string[] videosSizes = inputLines[1].Split(' ');
            for (int i = 0; i < numOfVideos; i++)
                p.allVideos.Add(new Video(i, int.Parse(videosSizes[i])));
            watch.Stop();
            LogPerformanceMetric("creating videos", watch);

            // Add all endpoints and connect them with caches
            int currentLine = 2;
            watch.Restart();
            for (int i = 0; i < numOfEndpoints; i++)
            {
                string[] endpointInfo = inputLines[currentLine].Split(' ');
                int dataCenterLatency = int.Parse(endpointInfo[0]);
                int endpointCachesAmount = int.Parse(endpointInfo[1]);

                Endpoint endpoint = new Endpoint(dataCenterLatency);
                currentLine++;
                for (int j = 0; j < endpointCachesAmount; j++)
                {
                    string[] cacheServerInfo = inputLines[currentLine++].Split(' ');
                    int cacheID = int.Parse(cacheServerInfo[0]);
                    int cacheLatency = int.Parse(cacheServerInfo[1]);

                    // Add Cache to the Endpoint
                    endpoint.latenciesToCaches.Add(p.allCaches[cacheID], cacheLatency);

                    // Add Endpoint to the Cache
                    p.allCaches[cacheID].endpointsConnected.Add(endpoint);
                }
                p.allEndpoints.Add(endpoint);
            }
            watch.Stop();
            LogPerformanceMetric("creating endpoints", watch);

            // Add requests
            watch.Restart();
            for (int i = 0; i < numOfRequests; i++)
            {
                string[] requestInfo = inputLines[currentLine++].Split(' ');
                int videoID = int.Parse(requestInfo[0]);
                int endpointID = int.Parse(requestInfo[1]);
                int watchCount = int.Parse(requestInfo[2]);

                Video video = p.allVideos[videoID];
                Endpoint endpoint = p.allEndpoints[endpointID];

                endpoint.requests.Add(new Request(video, watchCount));
            }
            watch.Stop();
            LogPerformanceMetric("adding requests", watch);
        }

        public static void LogPerformanceMetric(string action, System.Diagnostics.Stopwatch watch)
        {
            if (!Console.IsErrorRedirected)
                return;

            Console.Error.WriteLine("{0,-30} {1,10} ms", action, watch.ElapsedMilliseconds);
        }
    }
}

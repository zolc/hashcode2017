using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Problem
{
    class Cache
    {
        public List<Endpoint> endpoints = new List<Endpoint>();
        public int cacheId;
        public int capacityLeft;

        public Dictionary<Video, double> metrics = new Dictionary<Video, double>();
        public List<Video> addedVideos = new List<Video>();
        public List<KeyValuePair<Video, double>> metricsList = new List<KeyValuePair<Video, double>>();
        public Cache(List<Endpoint> e, int maxCapacity)
        {
            endpoints = e;
            capacityLeft = maxCapacity;
        }
        public Cache(int maxCapacity, int i)
        {
            capacityLeft = maxCapacity;
            cacheId = i;
        }

        public void SortMetrics()
        {
            foreach (var m in metrics.OrderByDescending(item => item.Value))//.OrderBy(item => item.Key.showups)
            {
                metricsList.Add(m);
            }
        }
        public void EvaluateMetrics()
        {
            foreach (var endpoint in endpoints)
            {
                foreach (var req in endpoint.requests)
                {
                    try
                    {
                        metrics.Add(req.video, 0);
                    }
                    catch { }
                    metrics[req.video] += req.videoCount * (endpoint.dataCenterLatency - endpoint.latencies[this]);
                }
            }
            KeyValuePair<Video, double>[] array = metrics.ToArray();
            for (int i = 0; i < metrics.Count(); i++)
            {
                metrics[array[i].Key] /= array[i].Key.size;
            }
        }

        public void addVideos()
        {
            foreach (var x in metricsList)
            {
                if (capacityLeft >= x.Key.size)
                {
                    Video added = x.Key;
                    clearEndpoints(added);
                    capacityLeft -= x.Key.size;
                    addedVideos.Add(x.Key);
                    x.Key.showups++;
                }
            }
        }
        public void clearEndpoints(Video vid)
        {
            foreach (var end in endpoints)
            {
                end.DeleteVideo(vid);
            }
        }
        public void finish()
        {
            EvaluateMetrics();
            SortMetrics();
            addVideos();
        }
        public bool IsUsed()
        {
            return (addedVideos.Count() != 0);
        }
    }
}
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

        public Dictionary<Video,double> metrics = new Dictionary<Video,double>();
        public List<Video> addedVideos = new List<Video>();
        public Cache(List<Endpoint> e,int maxCapacity)
        {
            endpoints = e;
            capacityLeft = maxCapacity;
        }
        public Cache(int maxCapacity,int i)
        {
            capacityLeft = maxCapacity;
            cacheId = i;
        }

        public void SortMetrics()
        {
            metrics.OrderBy(item => item.Value);

        }
        public void EvaluateMetrics()
        {
            foreach (var endpoint in endpoints)
            {
                foreach (var req in endpoint.requests)
                {
                    try
                    {
                        metrics.Add(req.video,0);
                    }
                    catch{ }
                    metrics[req.video] += req.videoCount * (endpoint.dataCenterLatency - endpoint.latencies[this]);
                    metrics[req.video] /= (1 + req.video.showups);
                }
            }
            KeyValuePair<Video,double>[] array = metrics.ToArray();
            for(int i =0; i<metrics.Count(); i++)
            {
                metrics[array[i].Key] /= array[i].Key.size;
            }
        }

        public void addVideos()
        {
            foreach (var x in metrics)
            {
                if (capacityLeft >= x.Key.size)
                {
                    capacityLeft -= x.Key.size;
                    addedVideos.Add(x.Key);
                    x.Key.showups++;
                }
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

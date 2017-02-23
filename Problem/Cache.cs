using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Problem
{
    class Cache
    {
        public List<Endpoint> endpoints=new List<Endpoint>();
        public int cacheId;
        public int capacityLeft;

        public Dictionary<Video, double> metrics=new Dictionary<Video,double>();
        public List<Video> addedVideos = new List<Video>();
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
            metrics.OrderBy(item => item.Value);

        }
        public void EvaluateMetrics()
        {
            foreach(var endpoint in endpoints)
            {
                foreach(var req in endpoint.requests)
                {
                    metrics.Add(req.video,0);
                    metrics[req.video] += req.videoCount * (endpoint.dataCenterLatency - endpoint.latencies[this]);
                }
            }
            foreach(var x in metrics)
            {
                metrics[x.Key] /= x.Key.size;
            }
        }

        public void addVideo()
        {
            foreach(var x in metrics)
            {
                if(capacityLeft >= x.Key.size)
                {
                    capacityLeft -= x.Key.size;
                    addedVideos.Add(x.Key);
                }
            }
        }
    }
}

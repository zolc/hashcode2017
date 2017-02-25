using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Problem
{
    class Endpoint
    {
        public int dataCenterLatency;
        public Dictionary<Cache, int> latencies = new Dictionary<Cache, int>();
        public List<Request> requests = new List<Request>();


        public Endpoint(int d)
        {
            dataCenterLatency = d;
        }

        public void DeleteVideo(Video vid)
        {
            List<Request> videosToRemove = new List<Request>();
            foreach (var req in requests)
            {
                if (req.video == vid)
                    videosToRemove.Add(req);
            }
            foreach (var v in videosToRemove)
                requests.Remove(v);
        }
    }
}

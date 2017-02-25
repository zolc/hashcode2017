using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Problem
{
    public class Endpoint
    {
        public int dataCenterLatency;
        public Dictionary<Cache, int> latenciesToCaches = new Dictionary<Cache, int>();
        public List<Request> requests = new List<Request>();
        public object requestsLock = new object();

        public Endpoint(int _dataCenterLatency)
        {
            dataCenterLatency = _dataCenterLatency;
        }

        public void DeleteVideo(Video vid)
        {
            List<Request> requestsToRemove = new List<Request>();
            lock (requestsLock)
            {
                foreach (var request in requests)
                {
                    if (request.video == vid)
                        requestsToRemove.Add(request);
                }
                foreach (var request in requestsToRemove)
                    requests.Remove(request);
            }
        }
    }
}

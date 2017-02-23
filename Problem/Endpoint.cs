using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Problem
{
    class Endpoint
    {
        int dataCenterLatency;
        Dictionary<int,int> latencies;
        List<Cache> caches;
        List<Request> requests;

        public Endpoint(int d,Dictionary<int,int> l, List<Cache> ch, List<Request> req)
        {
            dataCenterLatency = d;
            latencies = l;

            caches = ch;
            requests = req;
        }
    }
}

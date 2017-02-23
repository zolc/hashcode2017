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
        public Dictionary<Cache,int> latencies;
        public List<Request> requests=new List<Request>();


        public Endpoint(int d,Dictionary<Cache,int> l)
        {
            dataCenterLatency = d;
            latencies = l;
        }
    }
}

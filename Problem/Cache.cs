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
    
    }
}

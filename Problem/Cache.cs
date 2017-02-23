using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Problem
{
    class Cache
    {
        List<Endpoint> endpoints;

        int capacityLeft;
        public Cache(List<Endpoint> e, int maxCapacity)
        {
            endpoints = e;
            capacityLeft = maxCapacity;
        }
    }
}

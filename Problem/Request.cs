using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Problem
{
    class Request
    {
        public Video video;
        public int videoCount;
        //public double Metric { get; set; }
        public Request(Video v, int vc)
        {
            video = v;
            videoCount = vc;
           // Metric = vc / v.size;
        }
    }
}

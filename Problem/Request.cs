using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Problem
{
    public class Request
    {
        public Video video;
        public int watchCount;

        public Request(Video _video, int _watchCount)
        {
            video = _video;
            this.watchCount = _watchCount;
        }

    }
}

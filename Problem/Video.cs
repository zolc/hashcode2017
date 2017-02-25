using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Problem
{
    class Video
    {
        public int id;
        public int size;
        public int alreadyUsedAmount = 0;

        public Video(int _id, int _size)
        {
            id = _id;
            size = _size;
        }
    }
}

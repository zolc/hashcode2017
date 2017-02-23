using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Problem
{
    class Program
    {
        //private static int numOfvideos;
        //private static int numOfEndpoints;
        public int maxSize;
        public List<Cache> allCaches;
        public List<Endpoint> allEndpoints;
        public List<Video> allVideos;
        public List<Request> allRequests;
        static void Main(string[] args)
        {

        }

        static void Parse()
        {
            string filename = Console.ReadLine();

            string input = File.ReadAllText(filename);
            string firstline = input.Substring(0,input.IndexOf('\n'));
            input = input.Remove(0,firstline.Length + 1);
            string[] vals = firstline.Split(' ');
           

        }

    }
}

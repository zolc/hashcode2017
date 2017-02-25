using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Problem
{
    public class Cache
    {
        public List<Endpoint> endpointsConnected = new List<Endpoint>();
        public int cacheID;
        public int capacityLeft;

        public Dictionary<Video, double> videoMetrics = new Dictionary<Video, double>();
        public List<Video> videosHeld = new List<Video>();

        public Cache(int capacity, int _cacheID)
        {
            capacityLeft = capacity;
            cacheID = _cacheID;
        }

        public IOrderedEnumerable<KeyValuePair<Video, double>> SortMetrics()
        {
            return videoMetrics.OrderByDescending(item => item.Value);
        }

        public void EvaluateMetrics()
        {
            foreach (var endpoint in endpointsConnected)
            {
                lock (endpoint.requestsLock)
                {
                    foreach (var request in endpoint.requests)
                    {
                        if (!videoMetrics.ContainsKey(request.video))
                            videoMetrics.Add(request.video, 0);

                        videoMetrics[request.video] += request.watchCount * (endpoint.dataCenterLatency - endpoint.latenciesToCaches[this])/request.video.size;
                    }
                }
            }
        }

        public void FillWithVideos(IEnumerable<Video> videos)
        {
            foreach (var video in videos)
            {
                if (capacityLeft >= video.size)
                {
                    RemoveVideoFromEndpoints(video);
                    capacityLeft -= video.size;
                    videosHeld.Add(video);
                    video.alreadyUsedAmount++;
                }
            }
        }
        public void RemoveVideoFromEndpoints(Video video)
        {
            foreach (var endpoint in endpointsConnected)
            {
                endpoint.DeleteVideo(video);
            }
        }

        public void EvaluateAndFillWithVideos()
        {
            EvaluateMetrics();
            IEnumerable<Video> sortedVideos = SortMetrics().Select(videoWithMetric => videoWithMetric.Key);
            FillWithVideos(sortedVideos);
        }

        public bool IsUsed()
        {
            return (videosHeld.Count() != 0);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(cacheID);

            foreach (var video in videosHeld)
                sb.AppendFormat(" {0}", video.id);

            return sb.ToString();
        }
    }
}
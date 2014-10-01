using System;
using ImhoNet.Domain.Model;

namespace ImhoNet.Domain.Helper
{
    internal class PotentialVideo
    {
        public PotentialVideo(Video video, double potentialRate)
        {
            Video = video;
            PotentialRate = potentialRate;
        }

        public Video Video { get; private set; }

        public double PotentialRate { get; private set; }
        
        public int CompareTo(PotentialVideo other)
        {
            return PotentialRate.CompareTo(other.PotentialRate);
        }
    }
}
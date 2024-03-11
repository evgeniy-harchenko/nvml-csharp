namespace Nvidia.Nvml
{
    public struct NvmlFBCStats
    {
        public uint AverageFPS { get; } //	Moving average new frames captured per second.
        public uint AverageLatency { get; } // Moving average new frame capture latency in microseconds.
        public uint SessionsCount { get; } // Total no of sessions.
    }
}
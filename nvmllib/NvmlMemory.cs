namespace Nvidia.Nvml
{
    public struct NvmlMemory
    {
        public ulong Total { get; }
        public ulong Free { get; }
        public ulong Used { get; }
    }
}
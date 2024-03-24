using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using NvmlDeviceArchitecture = System.UInt32;
using NvmlFanControlPolicy = System.UInt32;

namespace Nvidia.Nvml
{
    internal static class Api
    {
        private static class Constants
        {
            public const string PlaceHolderLibraryName = "PlaceHolderLibrary";
            public const string WindowsAssemblyName = "nvml.dll";
            public const string LinuxAssemblyName = "libnvidia-ml.so";
        }

        static Api()
            => NativeLibrary.SetDllImportResolver(Assembly.GetExecutingAssembly(), DllImportResolver);

        private static string GetLibraryName()
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Unix:
                    return Constants.LinuxAssemblyName;
                case PlatformID.Win32NT:
                    return Constants.WindowsAssemblyName;
                default:
                    return Constants.LinuxAssemblyName;
            }
        }

        private static IntPtr DllImportResolver(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
        {
            var platformDependentName = GetLibraryName();

            NativeLibrary.TryLoad(platformDependentName, assembly, searchPath, out IntPtr handle);
            return handle;
        }

        #region Initialization And Cleanup

        [DllImport(Constants.PlaceHolderLibraryName, EntryPoint = "nvmlInit")]
        internal static extern NvmlReturn NvmlInit();

        [DllImport(Constants.PlaceHolderLibraryName, EntryPoint = "nvmlInitWithFlags")]
        internal static extern NvmlReturn NvmlInitWithFlags(uint flags);

        [DllImport(Constants.PlaceHolderLibraryName, EntryPoint = "nvmlInit_v2")]
        internal static extern NvmlReturn NvmlInitV2();

        [DllImport(Constants.PlaceHolderLibraryName, EntryPoint = "nvmlShutdown")]
        internal static extern NvmlReturn NvmlShutdown();

        #endregion

        #region System Queries

        [DllImport(Constants.PlaceHolderLibraryName, EntryPoint = "nvmlSystemGetCudaDriverVersion")]
        internal static extern NvmlReturn NvmlSystemGetCudaDriverVersion(out int cudaDriverVersion);

        [DllImport(Constants.PlaceHolderLibraryName, EntryPoint = "nvmlSystemGetCudaDriverVersion_v2")]
        internal static extern NvmlReturn NvmlSystemGetCudaDriverVersionV2(out int cudaDriverVersion);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi, EntryPoint = "nvmlSystemGetDriverVersion")]
        internal static extern NvmlReturn NvmlSystemGetDriverVersion(
            [Out, MarshalAs(UnmanagedType.LPArray)]
            byte[] version, uint length);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi, EntryPoint = "nvmlSystemGetNVMLVersion")]
        internal static extern NvmlReturn NvmlSystemGetNVMLVersion(
            [Out, MarshalAs(UnmanagedType.LPArray)]
            byte[] version, uint length);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi, EntryPoint = "nvmlSystemGetProcessName")]
        internal static extern NvmlReturn NvmlSystemGetProcessName(uint pid,
            [Out, MarshalAs(UnmanagedType.LPArray)]
            byte[] name, uint length);

        #endregion

        #region Device Queries

        [DllImport(Constants.PlaceHolderLibraryName, EntryPoint = "nvmlDeviceGetHandleByIndex")]
        internal static extern NvmlReturn NvmlDeviceGetHandleByIndex(uint index, out IntPtr device);

        [DllImport(Constants.PlaceHolderLibraryName, EntryPoint = "nvmlDeviceGetTemperature")]
        internal static extern NvmlReturn NvmlDeviceGetTemperature(IntPtr device, NvmlTemperatureSensor sensorType,
            out uint temperature);

        [DllImport(Constants.PlaceHolderLibraryName, EntryPoint = "nvmlDeviceGetTemperatureThreshold")]
        internal static extern NvmlReturn NvmlDeviceGetTemperatureThreshold(IntPtr device,
            NvmlTemperatureThresholds thresholdType,
            out uint temperature);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi,
            EntryPoint = "nvmlDeviceGetComputeRunningProcesses")]
        internal static extern NvmlReturn NvmlDeviceGetComputeRunningProcesses(IntPtr device, out uint infoCount,
            [Out, MarshalAs(UnmanagedType.LPArray)]
            NvmlProcessInfo[] infos);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi,
            EntryPoint = "nvmlDeviceGetAPIRestriction")]
        internal static extern NvmlReturn NvmlDeviceGetAPIRestriction(IntPtr device, NvmlRestrictedAPI apiType,
            out NvmlEnableState isRestricted);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi,
            EntryPoint = "nvmlDeviceGetApplicationsClock")]
        internal static extern NvmlReturn NvmlDeviceGetApplicationsClock(IntPtr device, NvmlClockType clockType,
            out uint clockMhz);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi, EntryPoint = "nvmlDeviceGetArchitecture")]
        internal static extern NvmlReturn NvmlDeviceGetArchitecture(IntPtr device, out NvmlDeviceArchitecture arch);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi, EntryPoint = "nvmlDeviceAttributes")]
        internal static extern NvmlReturn NvmlDeviceGetAttributes(IntPtr device, out NvmlDeviceAttributes attributes);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi,
            EntryPoint = "nvmlDeviceGetAutoBoostedClocksEnabled")]
        internal static extern NvmlReturn NvmlDeviceGetAutoBoostedClocksEnabled(IntPtr device,
            out NvmlEnableState isEnabled, out NvmlEnableState defaultIsEnabled);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi,
            EntryPoint = "nvmlDeviceGetBAR1MemoryInfo")]
        internal static extern NvmlReturn NvmlDeviceGetBAR1MemoryInfo(IntPtr device, out NvmlBAR1Memory bar1Memory);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi, EntryPoint = "nvmlDeviceGetBoardId")]
        internal static extern NvmlReturn NvmlDeviceGetBoardId(IntPtr device, out uint boardId);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi,
            EntryPoint = "nvmlDeviceGetBoardPartNumber")]
        internal static extern NvmlReturn NvmlDeviceGetBoardPartNumber(IntPtr device,
            [Out, MarshalAs(UnmanagedType.LPArray)]
            byte[] partNumber, uint length);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi, EntryPoint = "nvmlDeviceGetBrand")]
        internal static extern NvmlReturn NvmlDeviceGetBrand(IntPtr device, out NvmlBrandType type);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi,
            EntryPoint = "nvmlDeviceGetBridgeChipInfo")]
        internal static extern NvmlReturn NvmlDeviceGetBridgeChipInfo(IntPtr device,
            NvmlBridgeChipHierarchy bridgeHierarchy);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi, EntryPoint = "nvmlDeviceGetClock")]
        internal static extern NvmlReturn NvmlDeviceGetClock(IntPtr device, NvmlClockType clockType,
            NvmlClockId clockId, out uint clockMHz);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi, EntryPoint = "nvmlDeviceGetClockInfo")]
        internal static extern NvmlReturn NvmlDeviceGetClockInfo(IntPtr device, NvmlClockType type, out uint clock);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi, EntryPoint = "nvmlDeviceGetMaxClockInfo")]
        internal static extern NvmlReturn NvmlDeviceGetMaxClockInfo(IntPtr device, NvmlClockType type, out uint clock);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi, EntryPoint = "nvmlDeviceGetComputeMode")]
        internal static extern NvmlReturn NvmlDeviceGetComputeMode(IntPtr device, out NvmlComputeMode mode);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi,
            EntryPoint = "nvmlDeviceGetComputeRunningProcesses")]
        internal static extern NvmlReturn NvmlDeviceGetComputeRunningProcesses(IntPtr device, out uint infoCount,
            out NvmlProcessInfo infos);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi, EntryPoint = "nvmlDeviceGetCount_v2")]
        internal static extern NvmlReturn NvmlDeviceGetCount_v2(out uint deviceCount);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi,
            EntryPoint = "nvmlDeviceGetCudaComputeCapability")]
        internal static extern NvmlReturn NvmlDeviceGetCudaComputeCapability(IntPtr device, out int major,
            out int minor);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi,
            EntryPoint = "nvmlDeviceGetCurrPcieLinkGeneration")]
        internal static extern NvmlReturn NvmlDeviceGetCurrPcieLinkGeneration(IntPtr device, out uint currLinkGen);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi,
            EntryPoint = "nvmlDeviceGetCurrPcieLinkWidth")]
        internal static extern NvmlReturn NvmlDeviceGetCurrPcieLinkWidth(IntPtr device, out uint currLinkWidth);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi,
            EntryPoint = "nvmlDeviceGetCurrentClocksThrottleReasons")]
        internal static extern NvmlReturn NvmlDeviceGetCurrentClocksThrottleReasons(IntPtr device,
            out ulong clocksThrottleReasons);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi,
            EntryPoint = "nvmlDeviceGetDecoderUtilization")]
        internal static extern NvmlReturn NvmlDeviceGetDecoderUtilization(IntPtr device, out uint utilization,
            out uint samplingPeriodUs);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi,
            EntryPoint = "nvmlDeviceGetDefaultApplicationsClock")]
        internal static extern NvmlReturn NvmlDeviceGetDefaultApplicationsClock(IntPtr device, NvmlClockType clockType,
            out uint clockMHz);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi,
            EntryPoint = "nvmlDeviceGetDetailedEccErrors")]
        internal static extern NvmlReturn NvmlDeviceGetDetailedEccErrors(IntPtr device, NvmlMemoryErrorType errorType,
            NvmlEccCounterType counterType, out NvmlEccErrorCounts eccCounts);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi, EntryPoint = "nvmlDeviceGetDisplayActive")]
        internal static extern NvmlReturn NvmlDeviceGetDisplayActive(IntPtr device, out NvmlEnableState isActive);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi, EntryPoint = "nvmlDeviceGetDisplayMode")]
        internal static extern NvmlReturn NvmlDeviceGetDisplayMode(IntPtr device, out NvmlEnableState display);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi, EntryPoint = "nvmlDeviceGetDriverModel")]
        internal static extern NvmlReturn NvmlDeviceGetDriverModel(IntPtr device, out NvmlDriverModel current,
            out NvmlDriverModel pending);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi, EntryPoint = "nvmlDeviceGetEccMode")]
        internal static extern NvmlReturn NvmlDeviceGetEccMode(IntPtr device, out NvmlEnableState current,
            out NvmlEnableState pending);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi,
            EntryPoint = "nvmlDeviceGetEncoderCapacity")]
        internal static extern NvmlReturn NvmlDeviceGetEncoderCapacity(IntPtr device, NvmlEncoderType encoderQueryType,
            out uint encoderCapacity);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi,
            EntryPoint = "nvmlDeviceGetEncoderSessions")]
        internal static extern NvmlReturn NvmlDeviceGetEncoderSessions(IntPtr device, out uint sessionCount,
            out NvmlEncoderSessionInfo sessionInfos);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi, EntryPoint = "nvmlDeviceGetEncoderStats")]
        internal static extern NvmlReturn NvmlDeviceGetEncoderStats(IntPtr device, out uint sessionCount,
            out uint averageFps, out uint averageLatency);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi,
            EntryPoint = "nvmlDeviceGetEncoderUtilization")]
        internal static extern NvmlReturn NvmlDeviceGetEncoderUtilization(IntPtr device, out uint utilization,
            out uint samplingPeriodUs);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi,
            EntryPoint = "nvmlDeviceGetEnforcedPowerLimit")]
        internal static extern NvmlReturn NvmlDeviceGetEnforcedPowerLimit(IntPtr device, out uint limit);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi, EntryPoint = "nvmlDeviceGetFBCSessions")]
        internal static extern NvmlReturn NvmlDeviceGetFBCSessions(IntPtr device, out uint sessionCount,
            out NvmlFBCSessionInfo sessionInfo);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi, EntryPoint = "nvmlDeviceGetFBCStats")]
        internal static extern NvmlReturn NvmlDeviceGetFBCStats(IntPtr device, out NvmlFBCStats fbcStats);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi,
            EntryPoint = "nvmlDeviceGetFanControlPolicy_v2")]
        internal static extern NvmlReturn NvmlDeviceGetFanControlPolicy_v2(IntPtr device, uint fan,
            out NvmlFanControlPolicy policy);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi, EntryPoint = "nvmlDeviceGetFanSpeed")]
        internal static extern NvmlReturn NvmlDeviceGetFanSpeed(IntPtr device, out uint speed);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi, EntryPoint = "nvmlDeviceGetFanSpeed_v2")]
        internal static extern NvmlReturn NvmlDeviceGetFanSpeed_v2(IntPtr device, uint fan, out uint speed);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi,
            EntryPoint = "nvmlDeviceGetGpcClkMinMaxVfOffset")]
        internal static extern NvmlReturn NvmlDeviceGetGpcClkMinMaxVfOffset(IntPtr device, out int minOffset,
            out int maxOffset);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi,
            EntryPoint = "nvmlDeviceGetGpcClkVfOffset")]
        internal static extern NvmlReturn NvmlDeviceGetGpcClkVfOffset(IntPtr device, out int offset);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi,
            EntryPoint = "nvmlDeviceGetGpuMaxPcieLinkGeneration")]
        internal static extern NvmlReturn NvmlDeviceGetGpuMaxPcieLinkGeneration(IntPtr device,
            out uint maxLinkGenDevice);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi,
            EntryPoint = "nvmlDeviceGetGpuOperationMode")]
        internal static extern NvmlReturn NvmlDeviceGetGpuOperationMode(IntPtr device, out NvmlGpuOperationMode current,
            out NvmlGpuOperationMode pending);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi,
            EntryPoint = "nvmlDeviceGetGraphicsRunningProcesses")]
        internal static extern NvmlReturn NvmlDeviceGetGraphicsRunningProcesses(IntPtr device, out uint infoCount,
            [Out, MarshalAs(UnmanagedType.LPArray)]
            NvmlProcessInfo[] infos);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi,
            EntryPoint = "nvmlDeviceGetHandleByIndex_v2")]
        internal static extern NvmlReturn NvmlDeviceGetHandleByIndex_v2(uint index, out IntPtr device);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi,
            EntryPoint = "nvmlDeviceGetHandleByPciBusId_v2")]
        internal static extern NvmlReturn NvmlDeviceGetHandleByPciBusId_v2(string pciBusId, out IntPtr device);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi,
            EntryPoint = "nvmlDeviceGetHandleBySerial")]
        internal static extern NvmlReturn NvmlDeviceGetHandleBySerial(string serial, IntPtr device);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi, EntryPoint = "nvmlDeviceGetHandleByUUID")]
        internal static extern NvmlReturn NvmlDeviceGetHandleByUUID(string uuid, out IntPtr device);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi, EntryPoint = "nvmlDeviceGetIndex")]
        internal static extern NvmlReturn NvmlDeviceGetIndex(IntPtr device, out uint index);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi,
            EntryPoint = "nvmlDeviceGetInforomConfigurationChecksum")]
        internal static extern NvmlReturn NvmlDeviceGetInforomConfigurationChecksum(IntPtr device, out uint checksum);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi,
            EntryPoint = "nvmlDeviceGetInforomImageVersion")]
        internal static extern NvmlReturn NvmlDeviceGetInforomImageVersion(IntPtr device, out string version,
            uint length);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi,
            EntryPoint = "nvmlDeviceGetCurrPcieLinkGeneration")]
        internal static extern NvmlReturn NvmlDeviceGetMaxPcieLinkGeneration(IntPtr device, out uint maxLinkGen);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi,
            EntryPoint = "nvmlDeviceGetMaxPcieLinkWidth")]
        internal static extern NvmlReturn NvmlDeviceGetMaxPcieLinkWidth(IntPtr device, out uint maxLinkWidth);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi,
            EntryPoint = "nvmlDeviceGetMemClkVfOffset")]
        internal static extern NvmlReturn NvmlDeviceGetMemClkVfOffset(IntPtr device, out int offset);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi,
            EntryPoint = "nvmlDeviceGetMemoryBusWidth")]
        internal static extern NvmlReturn NvmlDeviceGetMemoryBusWidth(IntPtr device, out uint busWidth);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi,
            EntryPoint = "nvmlDeviceGetMemoryErrorCounter")]
        internal static extern NvmlReturn NvmlDeviceGetMemoryErrorCounter(IntPtr device, NvmlMemoryErrorType errorType,
            NvmlEccCounterType counterType, NvmlMemoryLocation locationType, out ulong count);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi,
            EntryPoint = "nvmlDeviceGetMinMaxFanSpeed")]
        internal static extern NvmlReturn NvmlDeviceGetMinMaxFanSpeed(IntPtr device, out uint minSpeed,
            out uint maxSpeed);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi,
            EntryPoint = "nvmlDeviceGetMemoryInfo")]
        internal static extern NvmlReturn NvmlDeviceGetMemoryInfo(IntPtr device, out NvmlMemory memory);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi, EntryPoint = "nvmlDeviceGetName")]
        internal static extern NvmlReturn NvmlDeviceGetName(IntPtr device,
            [Out, MarshalAs(UnmanagedType.LPArray)]
            byte[] name, uint length);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi, EntryPoint = "nvmlDeviceGetNumFans")]
        internal static extern NvmlReturn NvmlDeviceGetNumFans(IntPtr device, out uint numFans);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi, EntryPoint = "nvmlDeviceGetNumGpuCores")]
        internal static extern NvmlReturn NvmlDeviceGetNumGpuCores(IntPtr device, out uint numCores);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi, EntryPoint = "nvmlDeviceGetPciInfo_v3")]
        internal static extern NvmlReturn NvmlDeviceGetPciInfo_v3(IntPtr device, out NvmlPciInfo pci);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi,
            EntryPoint = "nvmlDeviceGetPersistenceMode")]
        internal static extern NvmlReturn NvmlDeviceGetPersistenceMode(IntPtr device, out NvmlEnableState mode);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi,
            EntryPoint = "nvmlDeviceGetPowerManagementDefaultLimit")]
        internal static extern NvmlReturn
            NvmlDeviceGetPowerManagementDefaultLimit(IntPtr device, out uint defaultLimit);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi,
            EntryPoint = "nvmlDeviceGetPowerManagementLimit")]
        internal static extern NvmlReturn NvmlDeviceGetPowerManagementLimit(IntPtr device, out uint limit);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi,
            EntryPoint = "nvmlDeviceGetPowerManagementLimitConstraints")]
        internal static extern NvmlReturn NvmlDeviceGetPowerManagementLimitConstraints(IntPtr device, out uint minLimit,
            out uint maxLimit);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi, EntryPoint = "nvmlDeviceGetUUID")]
        internal static extern NvmlReturn NvmlDeviceGetUUID(IntPtr device,
            [Out, MarshalAs(UnmanagedType.LPArray)]
            byte[] uuid, uint length);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi,
            EntryPoint = "nvmlDeviceGetUtilizationRates")]
        internal static extern NvmlReturn NvmlDeviceGetUtilizationRates(IntPtr device, out NvmlUtilization utilization);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi, EntryPoint = "nvmlDeviceGetVbiosVersion")]
        internal static extern NvmlReturn NvmlDeviceGetVbiosVersion(IntPtr device,
            [Out, MarshalAs(UnmanagedType.LPArray)]
            byte[] version, uint length);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi,
            EntryPoint = "nvmlDeviceSetDefaultFanSpeed_v2")]
        internal static extern NvmlReturn NvmlDeviceSetDefaultFanSpeed_v2(IntPtr device, uint fan);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi,
            EntryPoint = "nvmlDeviceSetFanControlPolicy")]
        internal static extern NvmlReturn NvmlDeviceSetFanControlPolicy(IntPtr device, uint fan,
            out NvmlFanControlPolicy policy);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi,
            EntryPoint = "nvmlSystemGetConfComputeGpusReadyState")]
        internal static extern NvmlReturn NvmlSystemGetConfComputeGpusReadyState(out uint isAcceptingWork);

        #endregion

        #region Device Commands

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi,
            EntryPoint = "nvmlDeviceClearEccErrorCounts")]
        internal static extern NvmlReturn NvmlDeviceClearEccErrorCounts(IntPtr device, NvmlEccCounterType counterType);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi, EntryPoint = "nvmlDeviceGetClkMonStatus")]
        internal static extern NvmlReturn NvmlDeviceGetClkMonStatus(IntPtr device, out IntPtr status);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi,
            EntryPoint = "nvmlDeviceResetGpuLockedClocks")]
        internal static extern NvmlReturn NvmlDeviceResetGpuLockedClocks(IntPtr device);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi,
            EntryPoint = "nvmlDeviceResetMemoryLockedClocks")]
        internal static extern NvmlReturn NvmlDeviceResetMemoryLockedClocks(IntPtr device);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi,
            EntryPoint = "nvmlDeviceSetAPIRestriction")]
        internal static extern NvmlReturn NvmlDeviceSetAPIRestriction(IntPtr device, NvmlRestrictedAPI apiType,
            NvmlEnableState isRestricted);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi,
            EntryPoint = "nvmlDeviceSetApplicationsClocks")]
        internal static extern NvmlReturn NvmlDeviceSetApplicationsClocks(IntPtr device, uint memClockMHz,
            uint graphicsClockMHz);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi, EntryPoint = "nvmlDeviceSetComputeMode")]
        internal static extern NvmlReturn NvmlDeviceSetComputeMode(IntPtr device, NvmlComputeMode mode);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi,
            EntryPoint = "nvmlDeviceSetConfComputeUnprotectedMemSize")]
        internal static extern NvmlReturn NvmlDeviceSetConfComputeUnprotectedMemSize(IntPtr device, ulong sizeKiB);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi, EntryPoint = "nvmlDeviceSetDriverModel")]
        internal static extern NvmlReturn NvmlDeviceSetDriverModel(IntPtr device, NvmlDriverModel driverModel,
            uint flags);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi, EntryPoint = "nvmlDeviceSetEccMode")]
        internal static extern NvmlReturn NvmlDeviceSetEccMode(IntPtr device, NvmlEnableState ecc);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi, EntryPoint = "nvmlDeviceSetFanSpeed_v2")]
        internal static extern NvmlReturn NvmlDeviceSetFanSpeed_v2(IntPtr device, uint fan, uint speed);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi,
            EntryPoint = "nvmlDeviceSetGpcClkVfOffset")]
        internal static extern NvmlReturn NvmlDeviceSetGpcClkVfOffset(IntPtr device, int offset);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi,
            EntryPoint = "nvmlDeviceSetGpuLockedClocks")]
        internal static extern NvmlReturn NvmlDeviceSetGpuLockedClocks(IntPtr device, uint minGpuClockMHz,
            uint maxGpuClockMHz);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi,
            EntryPoint = "nvmlDeviceSetGpuOperationMode")]
        internal static extern NvmlReturn NvmlDeviceSetGpuOperationMode(IntPtr device, NvmlGpuOperationMode mode);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi,
            EntryPoint = "nvmlDeviceSetMemClkVfOffset")]
        internal static extern NvmlReturn NvmlDeviceSetMemClkVfOffset(IntPtr device, int offset);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi,
            EntryPoint = "nvmlDeviceSetMemoryLockedClocks")]
        internal static extern NvmlReturn NvmlDeviceSetMemoryLockedClocks(IntPtr device, uint minMemClockMHz,
            uint maxMemClockMHz);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi,
            EntryPoint = "nvmlDeviceSetPersistenceMode")]
        internal static extern NvmlReturn NvmlDeviceSetPersistenceMode(IntPtr device, NvmlEnableState mode);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi,
            EntryPoint = "nvmlDeviceSetPowerManagementLimit")]
        internal static extern NvmlReturn NvmlDeviceSetPowerManagementLimit(IntPtr device, uint limit);

        [DllImport(Constants.PlaceHolderLibraryName, CharSet = CharSet.Ansi,
            EntryPoint = "nvmlSystemSetConfComputeGpusReadyState")]
        internal static extern NvmlReturn NvmlSystemSetConfComputeGpusReadyState(uint isAcceptingWork);

        #endregion
    }

    public static class NvGpu
    {
        public static int CudaDriverVersionMajor(int version)
        {
            return version / 1000;
        }

        #region Initialization And Cleanup

        public static void NvmlInit()
        {
            NvmlReturn res;
            res = Api.NvmlInit();
            if (NvmlReturn.NVML_SUCCESS != res)
            {
                throw new SystemException(res.ToString());
            }
        }

        public static void NvmlInitV2()
        {
            NvmlReturn res;
            res = Api.NvmlInitV2();
            if (NvmlReturn.NVML_SUCCESS != res)
            {
                throw new SystemException(res.ToString());
            }
        }

        public static void NvmlInitWithFlags(uint flags)
        {
            NvmlReturn res;
            res = Api.NvmlInitWithFlags(flags);
            if (NvmlReturn.NVML_SUCCESS != res)
            {
                throw new SystemException(res.ToString());
            }
        }

        public static void NvmlShutdown()
        {
            NvmlReturn res;
            res = Api.NvmlShutdown();
            if (NvmlReturn.NVML_SUCCESS != res)
            {
                throw new SystemException(res.ToString());
            }
        }

        #endregion

        #region System Queries

        public static string NvmlSystemGetProcessName(uint pid, uint length)
        {
            NvmlReturn res;
            byte[] name = new byte[length];
            res = Api.NvmlSystemGetProcessName(pid, name, length);
            if (NvmlReturn.NVML_SUCCESS != res)
            {
                throw new SystemException(res.ToString());
            }

            return Encoding.Default.GetString(name).Replace("\0", "");
        }

        public static string nvmlSystemGetNVMLVersion()
        {
            NvmlReturn res;
            byte[] version = new byte[NvmlConstants.NVML_SYSTEM_NVML_VERSION_BUFFER_SIZE];
            res = Api.NvmlSystemGetNVMLVersion(version, NvmlConstants.NVML_SYSTEM_NVML_VERSION_BUFFER_SIZE);
            if (NvmlReturn.NVML_SUCCESS != res)
            {
                throw new SystemException(res.ToString());
            }

            return Encoding.Default.GetString(version).Replace("\0", "");
        }

        public static string NvmlSystemGetDriverVersion()
        {
            NvmlReturn res;
            byte[] version = new byte[NvmlConstants.NVML_SYSTEM_DRIVER_VERSION_BUFFER_SIZE];
            res = Api.NvmlSystemGetDriverVersion(version, NvmlConstants.NVML_SYSTEM_DRIVER_VERSION_BUFFER_SIZE);
            if (NvmlReturn.NVML_SUCCESS != res)
            {
                throw new SystemException(res.ToString());
            }

            return Encoding.Default.GetString(version).Replace("\0", "");
        }

        public static int NvmlSystemGetCudaDriverVersion()
        {
            int driverVersion;
            NvmlReturn res;
            res = Api.NvmlSystemGetCudaDriverVersion(out driverVersion);
            if (NvmlReturn.NVML_SUCCESS != res)
            {
                throw new SystemException(res.ToString());
            }

            return driverVersion;
        }

        public static int NvmlSystemGetCudaDriverVersionV2()
        {
            int driverVersion;
            NvmlReturn res;
            res = Api.NvmlSystemGetCudaDriverVersionV2(out driverVersion);
            if (NvmlReturn.NVML_SUCCESS != res)
            {
                throw new SystemException(res.ToString());
            }

            return driverVersion;
        }

        #endregion

        #region Device Queries

        public static NvmlComputeMode NvmlDeviceGetComputeMode(IntPtr device)
        {
            NvmlComputeMode mode;
            var res = Api.NvmlDeviceGetComputeMode(device, out mode);
            if (NvmlReturn.NVML_SUCCESS != res)
            {
                throw new SystemException(res.ToString());
            }

            return mode;
        }

        public static uint NvmlDeviceGetCountV2()
        {
            uint count = 0;
            NvmlReturn res = Api.NvmlDeviceGetCount_v2(out count);
            if (NvmlReturn.NVML_SUCCESS != res)
            {
                throw new SystemException(res.ToString());
            }

            return count;
        }

        public static uint NvmlDeviceGetMemoryBusWidth(IntPtr device)
        {
            uint busWidth = 0;
            NvmlReturn res = Api.NvmlDeviceGetMemoryBusWidth(device, out busWidth);
            if (NvmlReturn.NVML_SUCCESS != res)
            {
                throw new SystemException(res.ToString());
            }

            return busWidth;
        }

        public static NvmlMemory NvmlDeviceGetMemoryInfo(IntPtr device)
        {
            NvmlReturn res;
            NvmlMemory memory;
            res = Api.NvmlDeviceGetMemoryInfo(device, out memory);
            if (NvmlReturn.NVML_SUCCESS != res)
            {
                throw new SystemException(res.ToString());
            }

            return memory;
        }

        public static NvmlPciInfo NvmlDeviceGetPciInfoV3(IntPtr device)
        {
            NvmlPciInfo data = new NvmlPciInfo();
            NvmlReturn res = Api.NvmlDeviceGetPciInfo_v3(device, out data);
            if (NvmlReturn.NVML_SUCCESS != res)
            {
                throw new SystemException(res.ToString());
            }

            return data;
        }

        public static string NvmlDeviceGetName(IntPtr device)
        {
            byte[] buffer = new byte[NvmlConstants.NVML_DEVICE_NAME_BUFFER_SIZE];
            var res = Api.NvmlDeviceGetName(device, buffer, NvmlConstants.NVML_DEVICE_NAME_BUFFER_SIZE);
            if (NvmlReturn.NVML_SUCCESS != res)
            {
                throw new SystemException(res.ToString());
            }

            return Encoding.Default.GetString(buffer).Replace("\0", "");
        }

        public static uint NvmlDeviceGetNumFans(IntPtr device)
        {
            NvmlReturn res;
            uint numFans;
            res = Api.NvmlDeviceGetNumFans(device, out numFans);
            if (NvmlReturn.NVML_SUCCESS != res)
            {
                throw new SystemException(res.ToString());
            }

            return (uint)numFans;
        }

        public static uint NvmlDeviceGetNumGpuCores(IntPtr device)
        {
            NvmlReturn res;
            uint numCores;
            res = Api.NvmlDeviceGetNumGpuCores(device, out numCores);
            if (NvmlReturn.NVML_SUCCESS != res)
            {
                throw new SystemException(res.ToString());
            }

            return (uint)numCores;
        }

        public static string NvmlDeviceGetBoardPartNumber(IntPtr device)
        {
            byte[] partNumber = new byte[NvmlConstants.NVML_DEVICE_PART_NUMBER_BUFFER_SIZE];
            NvmlReturn res = Api.NvmlDeviceGetBoardPartNumber(device, partNumber,
                NvmlConstants.NVML_DEVICE_PART_NUMBER_BUFFER_SIZE);
            if (NvmlReturn.NVML_SUCCESS != res)
            {
                throw new SystemException(res.ToString());
            }

            return Encoding.Default.GetString(partNumber).Replace("\0", "");
        }

        public static NvmlEnableState NvmlDeviceGetAPIRestriction(IntPtr device, NvmlRestrictedAPI apiType)
        {
            NvmlEnableState state;
            NvmlReturn res;
            res = Api.NvmlDeviceGetAPIRestriction(device, apiType, out state);
            if (NvmlReturn.NVML_SUCCESS != res)
            {
                throw new SystemException(res.ToString());
            }

            return state;
        }

        public static (List<NvmlProcessInfo>, uint) NvmlDeviceGetComputeRunningProcesses(IntPtr device)
        {
            NvmlReturn res;
            int size = Marshal.SizeOf<NvmlProcessInfo>();
            // IntPtr buffer = Marshal.AllocHGlobal(size * 5);
            uint count = 0;

            res = Api.NvmlDeviceGetComputeRunningProcesses(device, out count, null);
            if (count <= 0)
            {
                return (new List<NvmlProcessInfo>(), count);
            }

            NvmlProcessInfo[] buffer = new NvmlProcessInfo[count];
            res = Api.NvmlDeviceGetComputeRunningProcesses(device, out count, buffer);
            if (NvmlReturn.NVML_SUCCESS != res)
            {
                throw new SystemException(res.ToString());
            }

            return (new List<NvmlProcessInfo>(buffer), count);
        }

        public static IntPtr NvmlDeviceGetHandleByIndex(uint index)
        {
            var device = IntPtr.Zero;
            NvmlReturn res;
            res = Api.NvmlDeviceGetHandleByIndex(index, out device);
            if (NvmlReturn.NVML_SUCCESS != res)
            {
                throw new SystemException(res.ToString());
            }

            return device;
        }

        public static uint NvmlDeviceGetTemperature(IntPtr device)
        {
            NvmlReturn res;
            uint temperature;
            res = Api.NvmlDeviceGetTemperature(device, NvmlTemperatureSensor.NVML_TEMPERATURE_GPU, out temperature);
            if (NvmlReturn.NVML_SUCCESS != res)
            {
                throw new SystemException(res.ToString());
            }

            return (uint)temperature;
        }

        public static uint NvmlDeviceGetTemperature(IntPtr device,
            NvmlTemperatureSensor sensorType)
        {
            NvmlReturn res;
            uint temperature;
            res = Api.NvmlDeviceGetTemperature(device, sensorType, out temperature);
            if (NvmlReturn.NVML_SUCCESS != res)
            {
                throw new SystemException(res.ToString());
            }

            return (uint)temperature;
        }

        public static uint NvmlDeviceGetTemperatureThreshold(IntPtr device,
            NvmlTemperatureThresholds thresholdType)
        {
            NvmlReturn res;
            uint temperature;
            res = Api.NvmlDeviceGetTemperatureThreshold(device, thresholdType, out temperature);
            if (NvmlReturn.NVML_SUCCESS != res)
            {
                throw new SystemException(res.ToString());
            }

            return (uint)temperature;
        }

        public static uint NvmlDeviceGetClockInfo(IntPtr device, NvmlClockType type)
        {
            NvmlReturn res;
            uint clock;
            res = Api.NvmlDeviceGetClockInfo(device, type, out clock);
            if (NvmlReturn.NVML_SUCCESS != res)
            {
                throw new SystemException(res.ToString());
            }

            return (uint)clock;
        }

        public static uint NvmlDeviceGetMaxClockInfo(IntPtr device, NvmlClockType type)
        {
            NvmlReturn res;
            uint clock;
            res = Api.NvmlDeviceGetMaxClockInfo(device, type, out clock);
            if (NvmlReturn.NVML_SUCCESS != res)
            {
                throw new SystemException(res.ToString());
            }

            return (uint)clock;
        }

        public static uint NvmlDeviceGetFanSpeed(IntPtr device)
        {
            NvmlReturn res;
            uint speed;
            res = Api.NvmlDeviceGetFanSpeed(device, out speed);
            if (NvmlReturn.NVML_SUCCESS != res)
            {
                throw new SystemException(res.ToString());
            }

            return (uint)speed;
        }

        public static uint NvmlDeviceGetFanSpeed_v2(IntPtr device, uint fan)
        {
            NvmlReturn res;
            uint speed;
            res = Api.NvmlDeviceGetFanSpeed_v2(device, fan, out speed);
            if (NvmlReturn.NVML_SUCCESS != res)
            {
                throw new SystemException(res.ToString());
            }

            return (uint)speed;
        }

        public static void NvmlDeviceSetDefaultFanSpeed_v2(IntPtr device, uint fan)
        {
            NvmlReturn res;
            res = Api.NvmlDeviceSetDefaultFanSpeed_v2(device, fan);
            if (NvmlReturn.NVML_SUCCESS != res)
            {
                throw new SystemException(res.ToString());
            }
        }

        public static NvmlFBCStats NvmlDeviceGetFBCStats(IntPtr device)
        {
            NvmlReturn res;
            NvmlFBCStats stats;
            res = Api.NvmlDeviceGetFBCStats(device, out stats);
            if (NvmlReturn.NVML_SUCCESS != res)
            {
                throw new SystemException(res.ToString());
            }

            return stats;
        }

        public static NvmlBAR1Memory NvmlDeviceGetBAR1MemoryInfo(IntPtr device)
        {
            NvmlReturn res;
            NvmlBAR1Memory memory;
            res = Api.NvmlDeviceGetBAR1MemoryInfo(device, out memory);
            if (NvmlReturn.NVML_SUCCESS != res)
            {
                throw new SystemException(res.ToString());
            }

            return memory;
        }

        public static string NvmlDeviceGetUUID(IntPtr device)
        {
            byte[] buffer = new byte[NvmlConstants.NVML_DEVICE_UUID_V2_BUFFER_SIZE];
            var res = Api.NvmlDeviceGetUUID(device, buffer, NvmlConstants.NVML_DEVICE_UUID_V2_BUFFER_SIZE);
            if (NvmlReturn.NVML_SUCCESS != res)
            {
                throw new SystemException(res.ToString());
            }

            return Encoding.Default.GetString(buffer).Replace("\0", "");
        }

        public static NvmlUtilization NvmlDeviceGetUtilizationRates(IntPtr device)
        {
            NvmlReturn res;
            NvmlUtilization utilization;
            res = Api.NvmlDeviceGetUtilizationRates(device, out utilization);
            if (NvmlReturn.NVML_SUCCESS != res)
            {
                throw new SystemException(res.ToString());
            }

            return utilization;
        }

        public static string NvmlDeviceGetVbiosVersion(IntPtr device)
        {
            byte[] buffer = new byte[NvmlConstants.NVML_DEVICE_VBIOS_VERSION_BUFFER_SIZE];
            var res = Api.NvmlDeviceGetVbiosVersion(device, buffer,
                NvmlConstants.NVML_DEVICE_VBIOS_VERSION_BUFFER_SIZE);
            if (NvmlReturn.NVML_SUCCESS != res)
            {
                throw new SystemException(res.ToString());
            }

            return Encoding.Default.GetString(buffer).Replace("\0", "");
        }

        public static (uint util, uint samplingPeriodUs) NvmlDeviceGetEncoderUtilization(IntPtr device)
        {
            NvmlReturn res;
            uint util;
            uint samplingPeriodUs;
            res = Api.NvmlDeviceGetEncoderUtilization(device, out util, out samplingPeriodUs);
            if (NvmlReturn.NVML_SUCCESS != res)
            {
                throw new SystemException(res.ToString());
            }

            return (util, samplingPeriodUs);
        }

        public static (uint util, uint samplingPeriodUs) NvmlDeviceGetDecoderUtilization(IntPtr device)
        {
            NvmlReturn res;
            uint util;
            uint samplingPeriodUs;
            res = Api.NvmlDeviceGetDecoderUtilization(device, out util, out samplingPeriodUs);
            if (NvmlReturn.NVML_SUCCESS != res)
            {
                throw new SystemException(res.ToString());
            }

            return (util, samplingPeriodUs);
        }

        public static (uint minSpeed, uint maxSpeed) NvmlDeviceGetMinMaxFanSpeed(IntPtr device)
        {
            NvmlReturn res;
            uint minSpeed;
            uint maxSpeed;
            res = Api.NvmlDeviceGetMinMaxFanSpeed(device, out minSpeed, out maxSpeed);
            if (NvmlReturn.NVML_SUCCESS != res)
            {
                throw new SystemException(res.ToString());
            }

            return (minSpeed, maxSpeed);
        }

        public static uint NvmlDeviceGetCurrPcieLinkGeneration(IntPtr device)
        {
            NvmlReturn res;
            uint currLinkGen;
            res = Api.NvmlDeviceGetCurrPcieLinkGeneration(device, out currLinkGen);
            if (NvmlReturn.NVML_SUCCESS != res)
            {
                throw new SystemException(res.ToString());
            }

            return (uint)currLinkGen;
        }

        public static uint NvmlDeviceGetCurrPcieLinkWidth(IntPtr device)
        {
            NvmlReturn res;
            uint currLinkWidth;
            res = Api.NvmlDeviceGetCurrPcieLinkWidth(device, out currLinkWidth);
            if (NvmlReturn.NVML_SUCCESS != res)
            {
                throw new SystemException(res.ToString());
            }

            return (uint)currLinkWidth;
        }

        public static uint NvmlDeviceGetMaxPcieLinkGeneration(IntPtr device)
        {
            NvmlReturn res;
            uint maxLinkGen;
            res = Api.NvmlDeviceGetMaxPcieLinkGeneration(device, out maxLinkGen);
            if (NvmlReturn.NVML_SUCCESS != res)
            {
                throw new SystemException(res.ToString());
            }

            return (uint)maxLinkGen;
        }

        public static uint NvmlDeviceGetMaxPcieLinkWidth(IntPtr device)
        {
            NvmlReturn res;
            uint maxLinkWidth;
            res = Api.NvmlDeviceGetMaxPcieLinkWidth(device, out maxLinkWidth);
            if (NvmlReturn.NVML_SUCCESS != res)
            {
                throw new SystemException(res.ToString());
            }

            return (uint)maxLinkWidth;
        }

        #endregion

        #region Device Commands

        public static void NvmlDeviceClearEccErrorCounts(IntPtr device, NvmlEccCounterType counterType)
        {
            NvmlReturn res;
            res = Api.NvmlDeviceClearEccErrorCounts(device, counterType);
            if (NvmlReturn.NVML_SUCCESS != res)
            {
                throw new SystemException(res.ToString());
            }
        }

        public static IntPtr NvmlDeviceGetClkMonStatus(IntPtr device)
        {
            IntPtr status = IntPtr.Zero;
            NvmlReturn res;
            res = Api.NvmlDeviceGetClkMonStatus(device, out status);
            if (NvmlReturn.NVML_SUCCESS != res)
            {
                throw new SystemException(res.ToString());
            }

            return status;
        }

        public static void NvmlDeviceResetGpuLockedClocks(IntPtr device)
        {
            NvmlReturn res;
            res = Api.NvmlDeviceResetGpuLockedClocks(device);
            if (NvmlReturn.NVML_SUCCESS != res)
            {
                throw new SystemException(res.ToString());
            }
        }

        public static void NvmlDeviceResetMemoryLockedClocks(IntPtr device)
        {
            NvmlReturn res;
            res = Api.NvmlDeviceResetMemoryLockedClocks(device);
            if (NvmlReturn.NVML_SUCCESS != res)
            {
                throw new SystemException(res.ToString());
            }
        }

        public static void NvmlDeviceSetAPIRestriction(IntPtr device, NvmlRestrictedAPI apiType,
            NvmlEnableState isRestricted)
        {
            NvmlReturn res;
            res = Api.NvmlDeviceSetAPIRestriction(device, apiType, isRestricted);
            if (NvmlReturn.NVML_SUCCESS != res)
            {
                throw new SystemException(res.ToString());
            }
        }

        public static void NvmlDeviceSetApplicationsClocks(IntPtr device, uint memClockMHz, uint graphicsClockMHz)
        {
            NvmlReturn res;
            res = Api.NvmlDeviceSetApplicationsClocks(device, memClockMHz, graphicsClockMHz);
            if (NvmlReturn.NVML_SUCCESS != res)
            {
                throw new SystemException(res.ToString());
            }
        }

        public static void NvmlDeviceSetComputeMode(IntPtr device, NvmlComputeMode mode)
        {
            var res = Api.NvmlDeviceSetComputeMode(device, mode);
            if (NvmlReturn.NVML_SUCCESS != res)
            {
                throw new SystemException(res.ToString());
            }
        }

        public static void NvmlDeviceSetConfComputeUnprotectedMemSize(IntPtr device, ulong sizeKiB)
        {
            NvmlReturn res;
            res = Api.NvmlDeviceSetConfComputeUnprotectedMemSize(device, sizeKiB);
            if (NvmlReturn.NVML_SUCCESS != res)
            {
                throw new SystemException(res.ToString());
            }
        }

        public static void NvmlDeviceSetDriverModel(IntPtr device, NvmlDriverModel driverModel, uint flags)
        {
            NvmlReturn res;
            res = Api.NvmlDeviceSetDriverModel(device, driverModel, flags);
            if (NvmlReturn.NVML_SUCCESS != res)
            {
                throw new SystemException(res.ToString());
            }
        }

        public static void NvmlDeviceSetEccMode(IntPtr device, NvmlEnableState ecc)
        {
            NvmlReturn res;
            res = Api.NvmlDeviceSetEccMode(device, ecc);
            if (NvmlReturn.NVML_SUCCESS != res)
            {
                throw new SystemException(res.ToString());
            }
        }

        public static void NvmlDeviceSetFanSpeed_v2(IntPtr device, uint fan, uint speed)
        {
            NvmlReturn res;
            res = Api.NvmlDeviceSetFanSpeed_v2(device, fan, speed);
            if (NvmlReturn.NVML_SUCCESS != res)
            {
                throw new SystemException(res.ToString());
            }
        }

        public static void NvmlDeviceSetGpcClkVfOffset(IntPtr device, int offset)
        {
            NvmlReturn res;
            res = Api.NvmlDeviceSetGpcClkVfOffset(device, offset);
            if (NvmlReturn.NVML_SUCCESS != res)
            {
                throw new SystemException(res.ToString());
            }
        }

        public static void NvmlDeviceSetGpuLockedClocks(IntPtr device, uint minGpuClockMHz, uint maxGpuClockMHz)
        {
            NvmlReturn res;
            res = Api.NvmlDeviceSetGpuLockedClocks(device, minGpuClockMHz, maxGpuClockMHz);
            if (NvmlReturn.NVML_SUCCESS != res)
            {
                throw new SystemException(res.ToString());
            }
        }

        public static void NvmlDeviceSetGpuOperationMode(IntPtr device, NvmlGpuOperationMode mode)
        {
            NvmlReturn res;
            res = Api.NvmlDeviceSetGpuOperationMode(device, mode);
            if (NvmlReturn.NVML_SUCCESS != res)
            {
                throw new SystemException(res.ToString());
            }
        }

        public static void NvmlDeviceSetMemClkVfOffset(IntPtr device, int offset)
        {
            NvmlReturn res;
            res = Api.NvmlDeviceSetMemClkVfOffset(device, offset);
            if (NvmlReturn.NVML_SUCCESS != res)
            {
                throw new SystemException(res.ToString());
            }
        }

        public static void NvmlDeviceSetMemoryLockedClocks(IntPtr device, uint minMemClockMHz, uint maxMemClockMHz)
        {
            NvmlReturn res;
            res = Api.NvmlDeviceSetMemoryLockedClocks(device, minMemClockMHz, maxMemClockMHz);
            if (NvmlReturn.NVML_SUCCESS != res)
            {
                throw new SystemException(res.ToString());
            }
        }

        public static void NvmlDeviceSetPersistenceMode(IntPtr device, NvmlEnableState mode)
        {
            NvmlReturn res;
            res = Api.NvmlDeviceSetPersistenceMode(device, mode);
            if (NvmlReturn.NVML_SUCCESS != res)
            {
                throw new SystemException(res.ToString());
            }
        }

        public static void NvmlDeviceSetPowerManagementLimit(IntPtr device, uint limit)
        {
            NvmlReturn res;
            res = Api.NvmlDeviceSetPowerManagementLimit(device, limit);
            if (NvmlReturn.NVML_SUCCESS != res)
            {
                throw new SystemException(res.ToString());
            }
        }

        public static void NvmlSystemSetConfComputeGpusReadyState(uint isAcceptingWork)
        {
            NvmlReturn res;
            res = Api.NvmlSystemSetConfComputeGpusReadyState(isAcceptingWork);
            if (NvmlReturn.NVML_SUCCESS != res)
            {
                throw new SystemException(res.ToString());
            }
        }

        #endregion
    }
}
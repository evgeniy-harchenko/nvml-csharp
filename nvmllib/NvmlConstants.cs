namespace Nvidia.Nvml
{
    public class NvmlConstants
    {
        /**
      * Buffer size guaranteed to be large enough for \ref nvmlDeviceGetInforomVersion and \ref nvmlDeviceGetInforomImageVersion
      */
        public const uint NVML_DEVICE_INFOROM_VERSION_BUFFER_SIZE = 16;

        /**
         * Buffer size guaranteed to be large enough for storing GPU identifiers.
         */
        public const uint NVML_DEVICE_UUID_BUFFER_SIZE = 80;

        /**
         * Buffer size guaranteed to be large enough for \ref nvmlDeviceGetUUID
         */
        public const uint NVML_DEVICE_UUID_V2_BUFFER_SIZE = 96;

        /**
         * Buffer size guaranteed to be large enough for \ref nvmlDeviceGetBoardPartNumber
         */
        public const uint NVML_DEVICE_PART_NUMBER_BUFFER_SIZE = 80;

        /**
         * Buffer size guaranteed to be large enough for \ref nvmlSystemGetDriverVersion
         */
        public const uint NVML_SYSTEM_DRIVER_VERSION_BUFFER_SIZE = 80;

        /**
         * Buffer size guaranteed to be large enough for \ref nvmlSystemGetNVMLVersion
         */
        public const uint NVML_SYSTEM_NVML_VERSION_BUFFER_SIZE = 80;

        /**
         * Buffer size guaranteed to be large enough for storing GPU device names.
         */
        public const uint NVML_DEVICE_NAME_BUFFER_SIZE = 64;

        /**
         * Buffer size guaranteed to be large enough for \ref nvmlDeviceGetName
         */
        public const uint NVML_DEVICE_NAME_V2_BUFFER_SIZE = 96;

        /**
         * Buffer size guaranteed to be large enough for \ref nvmlDeviceGetSerial
         */
        public const uint NVML_DEVICE_SERIAL_BUFFER_SIZE = 30;

        /**
         * Buffer size guaranteed to be large enough for \ref nvmlDeviceGetVbiosVersion
         */
        public const uint NVML_DEVICE_VBIOS_VERSION_BUFFER_SIZE = 32;

        public const uint NVML_INIT_FLAG_NO_GPUS = 1; //!< Don't fail nvmlInit() when no GPUs are found
        
        public const uint NVML_INIT_FLAG_NO_ATTACH = 2; //!< Don't attach GPUs
    }
}
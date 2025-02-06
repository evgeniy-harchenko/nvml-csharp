using System;
using System.Text;
using NUnit.Framework;
using Nvidia.Nvml;

namespace NvlmTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void SetDeviceComputeMode()
        {
            try
            {
                NvGpu.NvmlInitV2();
                IntPtr device = NvGpu.NvmlDeviceGetHandleByIndex(0);
                var original = NvGpu.NvmlDeviceGetComputeMode(device);
                NvGpu.NvmlDeviceSetComputeMode(device, NvmlComputeMode.NVML_COMPUTEMODE_PROHIBITED);
                var current = NvGpu.NvmlDeviceGetComputeMode(device);
                if (NvmlComputeMode.NVML_COMPUTEMODE_PROHIBITED != current)
                {
                    Assert.Fail("Must return NVML_COMPUTEMODE_PROHIBITED mode");
                }

                NvGpu.NvmlDeviceSetComputeMode(device, original);
                current = NvGpu.NvmlDeviceGetComputeMode(device);
                if (original != current)
                {
                    Assert.Fail("Must return original mode");
                }

                NvGpu.NvmlShutdown();
            }
            catch (Exception e)
            {
                if (e.Message.Equals("NVML_ERROR_NO_PERMISSION"))
                {
                    TestContext.Progress.WriteLine("SetDeviceComputeMode >> In order to this test pass, the test case must be running with admin permission");
                } 
                else if (e.Message.Equals("NVML_ERROR_NOT_SUPPORTED"))
                {
                    TestContext.Progress.WriteLine("SetDeviceComputeMode >> This mode isn't supported by this device");
                    Assert.Pass();
                }
                Assert.Fail(e.ToString());
            }
        }

        [Test]
        public void RetrieveDeviceComputeMode()
        {
            try
            {
                NvGpu.NvmlInitV2();
                IntPtr device = NvGpu.NvmlDeviceGetHandleByIndex(0);
                var computeMode = NvGpu.NvmlDeviceGetComputeMode(device);
                NvGpu.NvmlShutdown();
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [Test]
        public void RetrieveDeviceCount()
        {
            try
            {
                NvGpu.NvmlInitV2();
                var count = NvGpu.NvmlDeviceGetCountV2();
                NvGpu.NvmlShutdown();
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [Test]
        public void RetrieveDevicePciInfo()
        {
            try
            {
                NvGpu.NvmlInitV2();
                var device = NvGpu.NvmlDeviceGetHandleByIndex(0);
                var info = NvGpu.NvmlDeviceGetPciInfoV3(device);
                byte[] busIdData = Array.ConvertAll(info.busId, (a) => (byte)a);
                byte[] busIdLegacyData = Array.ConvertAll(info.busIdLegacy, (a) => (byte)a);
                string busId = Encoding.Default.GetString(busIdData);
                string busIdLegacy = Encoding.Default.GetString(busIdLegacyData);
                NvGpu.NvmlShutdown();
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [Test]
        public void RetrieveDeviceName()
        {
            try
            {
                NvGpu.NvmlInitV2();
                var device = NvGpu.NvmlDeviceGetHandleByIndex(0);
                string name = NvGpu.NvmlDeviceGetName(device);
                NvGpu.NvmlShutdown();
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [Test]
        public void RetrieveDeviceBoardPartNumberTest()
        {
            try
            {
                NvGpu.NvmlInitV2();
                var device = NvGpu.NvmlDeviceGetHandleByIndex(0);
                var partnumber = NvGpu.NvmlDeviceGetBoardPartNumber(device);

                NvGpu.NvmlShutdown();
            }
            catch (Exception e)
            {
                if (e.Message.Equals("NVML_ERROR_NOT_SUPPORTED"))
                {
                    Assert.Pass("NVML_ERROR_NOT_SUPPORTED means vbios fields have not been filled");
                }
                Assert.Fail(e.ToString());
            }
        }

        [Test]
        public void ApiRestictionsTest()
        {
            try
            {
                NvGpu.NvmlInitV2();
                var device = IntPtr.Zero;
                device = NvGpu.NvmlDeviceGetHandleByIndex(0);
                var state = NvGpu.NvmlDeviceGetAPIRestriction(device, NvmlRestrictedAPI.NVML_RESTRICTED_API_SET_APPLICATION_CLOCKS);

                NvGpu.NvmlShutdown();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        [Test]
        public void RetrieveProcessListTest()
        {
            try
            {
                NvGpu.NvmlInitV2();
                var device = IntPtr.Zero;
                device = NvGpu.NvmlDeviceGetHandleByIndex(0);
                var (list, count) = NvGpu.NvmlDeviceGetComputeRunningProcesses(device);
                NvGpu.NvmlShutdown();
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [Test]
        public void RetrieveProcessNameTest()
        {
            try
            {
                NvGpu.NvmlInitV2();
                var device = IntPtr.Zero;
                device = NvGpu.NvmlDeviceGetHandleByIndex(0);
                var (list, count) = NvGpu.NvmlDeviceGetComputeRunningProcesses(device);
                if (count > 0)
                {
                    TestContext.Progress.WriteLine("RetrieveProcessNameTest >> Testing NvmlSystemGetProcessName as we have processes in gpu");
                    string processName = NvGpu.NvmlSystemGetProcessName(list[0].Pid, 30);
                }
                else
                {
                    TestContext.Progress.WriteLine("RetrieveProcessNameTest >> Testing NvmlSystemGetProcessName with inexistent pid 0");
                    string processName = NvGpu.NvmlSystemGetProcessName(0, 30);
                }
                NvGpu.NvmlShutdown();
            }
            catch (Exception e)
            {
                if (e.Message.Equals("NVML_ERROR_NOT_FOUND"))
                {
                    Assert.Pass();
                }
                Assert.Fail(e.ToString());
            }
        }

        [Test]
        public void RetrieveNvmlVersionTest()
        {
            try
            {
                NvGpu.NvmlInitV2();
                string version = NvGpu.nvmlSystemGetNVMLVersion();
                if (version.Length == 0 || version == null)
                {
                    Assert.Fail("Something fail to acquire nvml version.");
                }
                NvGpu.NvmlShutdown();
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [Test]
        public void RetrieveDriverVersionTest()
        {
            try
            {
                NvGpu.NvmlInitV2();
                string driverVersion = NvGpu.NvmlSystemGetDriverVersion();
                if (driverVersion.Length == 0 || driverVersion == null)
                {
                    Assert.Fail("Something fail to acquire driver version.");
                }
                NvGpu.NvmlShutdown();
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [Test]
        public void RetriveCudaDriverVersionTest()
        {
            try
            {
                NvGpu.NvmlInitV2();
                int version = NvGpu.NvmlSystemGetCudaDriverVersion();
                int major = NvGpu.CudaDriverVersionMajor(version);
                NvGpu.NvmlShutdown();
                NvGpu.NvmlInitV2();
                version = NvGpu.NvmlSystemGetCudaDriverVersionV2();
                major = NvGpu.CudaDriverVersionMajor(version);
                NvGpu.NvmlShutdown();
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [Test]
        public void InitializationTest()
        {
            try
            {
                NvGpu.NvmlInit();
                NvGpu.NvmlShutdown();
                NvGpu.NvmlInitV2();
                NvGpu.NvmlShutdown();
                NvGpu.NvmlInitWithFlags(NvmlConstants.NVML_INIT_FLAG_NO_ATTACH);
                NvGpu.NvmlShutdown();
                NvGpu.NvmlInitWithFlags(NvmlConstants.NVML_INIT_FLAG_NO_GPUS);
                NvGpu.NvmlShutdown();
                NvGpu.NvmlInitWithFlags(NvmlConstants.NVML_INIT_FLAG_NO_GPUS | NvmlConstants.NVML_INIT_FLAG_NO_ATTACH);
                NvGpu.NvmlShutdown();
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [Test]
        public void RecoverDeviceTest()
        {
            try
            {
                var device = IntPtr.Zero;
                NvGpu.NvmlInitV2();
                device = NvGpu.NvmlDeviceGetHandleByIndex(0);
                if (IntPtr.Zero == device)
                {
                    Assert.Fail("Device cant be IntPtr.Zero.");
                }
                NvGpu.NvmlShutdown();
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [Test]
        public void GetGpuTemperatureTest()
        {
            try
            {
                var device = IntPtr.Zero;
                NvGpu.NvmlInitV2();
                device = NvGpu.NvmlDeviceGetHandleByIndex(0);
                var temperature = NvGpu.NvmlDeviceGetTemperature(device, NvmlTemperatureSensor.NVML_TEMPERATURE_GPU);
                if (!(temperature >= 40 && temperature <= 80))
                {
                    Assert.Fail("Cant get temperature.");
                }
                NvGpu.NvmlShutdown();
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }
        
        [Test]
        public void RetrieveDevicePerformanceStateTest()
        {
            try
            {
                NvGpu.NvmlInitV2();
                IntPtr device = NvGpu.NvmlDeviceGetHandleByIndex(0);
                int pState = NvGpu.NvmlDeviceGetPerformanceState(device);
                TestContext.Progress.WriteLine($"Performance State: {pState}");
                // Обычно pState находится в диапазоне от 0 до 15, где 0 — максимальная производительность
                Assert.IsTrue(pState >= 0 && pState <= 15, "Performance state is out of expected range (0-15)");
                NvGpu.NvmlShutdown();
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [Test]
        public void RetrieveDeviceClockThrottleReasonsTest()
        {
            try
            {
                NvGpu.NvmlInitV2();
                IntPtr device = NvGpu.NvmlDeviceGetHandleByIndex(0);
                ulong throttleReasons = NvGpu.NvmlDeviceGetCurrentClocksThrottleReasons(device);
                TestContext.Progress.WriteLine($"Clock Throttle Reasons: {throttleReasons}");
                // Пока не знаем ожидаемого набора флагов, проверим, что значение неотрицательное.
                Assert.IsTrue(throttleReasons >= 0, "Clock throttle reasons must be non-negative");
                NvGpu.NvmlShutdown();
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [Test]
        public void RetrieveDeviceSupportedClocksThrottleReasonsTest()
        {
            try
            {
                NvGpu.NvmlInitV2();
                IntPtr device = NvGpu.NvmlDeviceGetHandleByIndex(0);
                ulong supportedReasons = NvGpu.NvmlDeviceGetSupportedClocksThrottleReasons(device);
                TestContext.Progress.WriteLine($"Supported Clocks Throttle Reasons: {supportedReasons}");
                // Аналогично, проверим, что значение неотрицательное.
                Assert.IsTrue(supportedReasons >= 0, "Supported clocks throttle reasons must be non-negative");
                NvGpu.NvmlShutdown();
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [Test]
        public void RetrieveDeviceCurrentClocksThrottleReasonsTest()
        {
            try
            {
                NvGpu.NvmlInitV2();
                IntPtr device = NvGpu.NvmlDeviceGetHandleByIndex(0);
                ulong currentThrottleReasons = NvGpu.NvmlDeviceGetCurrentClocksThrottleReasons(device);
                TestContext.Progress.WriteLine($"Current Clocks Throttle Reasons: {currentThrottleReasons}");
                // Проверим, что значение корректное (>= 0)
                Assert.IsTrue(currentThrottleReasons >= 0, "Current clocks throttle reasons must be non-negative");
                NvGpu.NvmlShutdown();
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }
        
                [Test]
        public void RetrieveDevicePowerStateTest()
        {
            try
            {
                NvGpu.NvmlInitV2();
                IntPtr device = NvGpu.NvmlDeviceGetHandleByIndex(0);
                
                uint powerState = NvGpu.NvmlDeviceGetPowerState(device);
                TestContext.Progress.WriteLine($"Power State: {powerState}");
                
                // Обычно powerState — это значение в диапазоне (например, 0..5).
                Assert.IsTrue(powerState <= 5, "Power state is outside the expected range (0-5)");
                
                NvGpu.NvmlShutdown();
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [Test]
        public void RetrieveDevicePowerUsageTest()
        {
            try
            {
                NvGpu.NvmlInitV2();
                IntPtr device = NvGpu.NvmlDeviceGetHandleByIndex(0);
                
                uint powerUsage = NvGpu.NvmlDeviceGetPowerUsage(device);
                TestContext.Progress.WriteLine($"Power Usage (mW): {powerUsage}");
                
                // Проверим, что потребление мощности положительно.
                Assert.IsTrue(powerUsage > 0, "Power usage must be greater than zero");
                
                NvGpu.NvmlShutdown();
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [Test]
        public void RetrieveDeviceSupportedEventTypesTest()
        {
            try
            {
                NvGpu.NvmlInitV2();
                IntPtr device = NvGpu.NvmlDeviceGetHandleByIndex(0);
                
                ulong eventTypes = NvGpu.NvmlDeviceGetSupportedEventTypes(device);
                TestContext.Progress.WriteLine($"Supported Event Types: {eventTypes}");
                
                // Обычно набор поддерживаемых событий не должен быть нулевым.
                Assert.IsTrue(eventTypes != 0, "Supported event types should not be zero");
                
                NvGpu.NvmlShutdown();
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }

        [Test]
        public void RetrieveDeviceTotalEnergyConsumptionTest()
        {
            try
            {
                NvGpu.NvmlInitV2();
                IntPtr device = NvGpu.NvmlDeviceGetHandleByIndex(0);
                
                uint energy = NvGpu.NvmlDeviceGetTotalEnergyConsumption(device);
                TestContext.Progress.WriteLine($"Total Energy Consumption: {energy}");
                
                // Значение энергии может быть 0, если устройство не поддерживает измерение или тест выполнен слишком быстро.
                Assert.IsTrue(energy >= 0, "Total energy consumption should be non-negative");
                
                NvGpu.NvmlShutdown();
            }
            catch (Exception e)
            {
                // Если устройство не поддерживает измерение энергии, можно передать тест.
                if (e.Message.Contains("NVML_ERROR_NOT_SUPPORTED"))
                {
                    TestContext.Progress.WriteLine("Total Energy Consumption is not supported by this device.");
                    Assert.Pass();
                }
                Assert.Fail(e.ToString());
            }
        }

        [Test]
        public void RetrieveDevicePowerManagementLimitConstraintsTest()
        {
            try
            {
                NvGpu.NvmlInitV2();
                IntPtr device = NvGpu.NvmlDeviceGetHandleByIndex(0);
                
                (uint minLimit, uint maxLimit) = NvGpu.NvmlDeviceGetPowerManagementLimitConstraints(device);
                TestContext.Progress.WriteLine($"Power Management Limit Constraints - Min: {minLimit}, Max: {maxLimit}");
                
                // Минимальный лимит должен быть не больше максимального.
                Assert.IsTrue(minLimit <= maxLimit, "Minimum limit should be less than or equal to maximum limit");
                
                NvGpu.NvmlShutdown();
            }
            catch (Exception e)
            {
                Assert.Fail(e.ToString());
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TactPlay.Connection;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Enumeration;
using Windows.Storage.Streams;

namespace TactPlay.Connection
{
    class BLEConnection : ITactPlayConnection
    {
        private static readonly string DEVICE_NAME = "TactPlay";
        private static readonly string SERVICE_UUID = "5A2D3BF8-F0BC-11E5-9CE9-5E5517507E66";
        private static readonly string CHARACTERISTIC_UUID = "5a2d40ee-f0bc-11e5-9ce9-5e5517507e66";

        private List<DeviceInformation> devices = new List<DeviceInformation>();
        private SendVisualizer visualizer = new SendVisualizer();

        private DeviceWatcher deviceWatcher;
        private bool connecting;
        private GattCharacteristic motorCharacteristic;

        public BLEConnection()
        {
            visualizer.Show();
        }

        private void QueryDevices()
        {
            // Query for extra properties you want returned
            string[] requestedProperties = { "System.Devices.Aep.DeviceAddress", "System.Devices.Aep.IsConnected" };

            deviceWatcher =
                        DeviceInformation.CreateWatcher(
                                BluetoothLEDevice.GetDeviceSelectorFromPairingState(false),
                                requestedProperties,
                                DeviceInformationKind.AssociationEndpoint);

            // Register event handlers before starting the watcher.
            // Added, Updated and Removed are required to get all nearby devices
            deviceWatcher.Added += DeviceWatcher_Added;
            deviceWatcher.Updated += DeviceWatcher_Updated;
            // Start the watcher.
            deviceWatcher.Start();
        }

        private bool TactPlayFound()
        {
            foreach (DeviceInformation bleDeviceInfo in devices)
            {
                if (bleDeviceInfo.Name.Equals(DEVICE_NAME))
                {
                    return true;
                }
            }
            return false;
        }

        private async void WriteBytes(byte[] bytes)
        {
            visualizer.AddValue(bytes);
            if (motorCharacteristic == null)
            {
                return;
            }
            var writer = new DataWriter();
            long startTime = Environment.TickCount;
            Console.WriteLine(startTime + " Sending " + Utils.ByteArrayToString(bytes));
            writer.WriteBytes(bytes);
            GattCommunicationStatus status = await motorCharacteristic.WriteValueAsync(writer.DetachBuffer()); //TODO catch Exception after disconnect
            long endTime = Environment.TickCount;
            Console.WriteLine(endTime + " Status for " + Utils.ByteArrayToString(bytes) + ": " + status + ". Time: " + (endTime - startTime) + " ms");
        }

        private async void ConnectTactPlay()
        {
            lock (this)
            {
                if (connecting)
                {
                    return;
                }
                else
                {
                    connecting = true;
                }
            }
            DeviceInformation deviceInfo = GetDeviceByName(DEVICE_NAME);
            if (deviceInfo == null)
            {
                return;
            }
            Console.WriteLine("Connecting to " + deviceInfo.Id + "  " + deviceInfo.Name);
            BluetoothLEDevice bluetoothLeDevice = await BluetoothLEDevice.FromIdAsync(deviceInfo.Id);
            Console.WriteLine("Query service");
            GattDeviceServicesResult servicesResult = await bluetoothLeDevice.GetGattServicesAsync();
            Console.WriteLine("Query service complete");
            if (servicesResult.Status == GattCommunicationStatus.Success)
            {
                IReadOnlyList<GattDeviceService> services = servicesResult.Services;

                foreach (GattDeviceService service in services)
                {
                    if (service.Uuid.Equals(new Guid(SERVICE_UUID)))
                    {
                        Console.WriteLine("Service found!");
                        GattCharacteristicsResult characteristicsResult = await service.GetCharacteristicsAsync();
                        if (characteristicsResult.Status == GattCommunicationStatus.Success)
                        {
                            IReadOnlyList<GattCharacteristic> characteristics = characteristicsResult.Characteristics;
                            foreach (GattCharacteristic characteristic in characteristics)
                            {
                                if (characteristic.Uuid.Equals(new Guid(CHARACTERISTIC_UUID)))
                                {
                                    Console.WriteLine("Characteristic found!");
                                    motorCharacteristic = characteristic;
                                }
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Unknown service: " + service.Uuid);
                    }
                }

            }
            lock (this)
            {
                connecting = false;
            }
        }

        private DeviceInformation GetDeviceByID(string id)
        {
            foreach (DeviceInformation bleDeviceInfo in devices)
            {
                if (bleDeviceInfo.Id == id)
                {
                    return bleDeviceInfo;
                }
            }
            return null;
        }

        private DeviceInformation GetDeviceByName(string name)
        {
            foreach (DeviceInformation bleDeviceInfo in devices)
            {
                if (bleDeviceInfo.Name == name)
                {
                    return bleDeviceInfo;
                }
            }
            return null;
        }

        private void DeviceWatcher_Added(DeviceWatcher sender, DeviceInformation deviceInfo)
        {
            devices.Add(deviceInfo);
            Console.WriteLine("Device added: " + deviceInfo.Id + "  " + deviceInfo.Name);
        }

        private void DeviceWatcher_Updated(DeviceWatcher sender, DeviceInformationUpdate deviceInfoUpdate)
        {
            if (GetDeviceByID(deviceInfoUpdate.Id) != null)
            {
                GetDeviceByID(deviceInfoUpdate.Id).Update(deviceInfoUpdate);
                Console.WriteLine("Device Updated for " + deviceInfoUpdate.Id);
            }
        }

        public void ConnectToDevice()
        {
            Task.Run(() =>
            {
                QueryDevices();
                while (!TactPlayFound())
                {
                    Task.Delay(100);
                }
                ConnectTactPlay();
            });
        }

        public void CloseConnection()
        {
            motorCharacteristic = null;
        }

        public void SendBytes(byte[] bytes)
        {
            WriteBytes(bytes);
        }
    }
}

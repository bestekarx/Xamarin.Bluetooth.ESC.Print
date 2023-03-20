using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Android.Bluetooth;
using Java.Util;
using XamarinESCUtils.Model;
using XamarinESCUtils.Platforms.Common;

namespace XamarinESCUtils.Platforms.Android
{
    public class BluetoothService : IBlueToothService
    {
        private static readonly UUID PrinterUuid = UUID.FromString("00001101-0000-1000-8000-00805F9B34FB");
        private static BluetoothSocket _bluetoothSocket;

        private static BluetoothAdapter GetBtAdapter()
        {
            return BluetoothAdapter.DefaultAdapter;
        }

        private static BluetoothDevice GetDevice(BluetoothAdapter bluetoothAdapter, string printerAddress)
        {
            var devices = bluetoothAdapter.BondedDevices;
            return devices?.FirstOrDefault(bluetoothDevice => bluetoothDevice.Address != null && bluetoothDevice.Address.Equals(printerAddress));
        }

        private static BluetoothSocket GetSocket(BluetoothDevice device)
        {
            var socket = device.CreateRfcommSocketToServiceRecord(PrinterUuid);
            if (socket == null) return null;
            socket.Connect();
            return socket;
        }

        public IList<BluetoothInfo> GetDeviceList()
        {
            using var bluetoothAdapter = BluetoothAdapter.DefaultAdapter;
            var list = new List<BluetoothInfo>();
            bluetoothAdapter?.Enable();
            if (bluetoothAdapter?.BondedDevices == null) return list;
            list.AddRange((bluetoothAdapter?.BondedDevices).Select(bluetooth => new BluetoothInfo { Name = bluetooth.Name, Adress = bluetooth.Address, }));
            return list;
        }

        bool IBlueToothService.ConnectBlueTooth(string printerAddress)
        {
            if (_bluetoothSocket != null) return true;

            if (GetBtAdapter() == null)
            {
                Console.WriteLine("Print error: Bluetooth device is unavailable");
                return false;
            }

            if (!GetBtAdapter().IsEnabled)
            {
                Console.WriteLine("Print error: Bluetooth device not detected. Please open Bluetooth!");
                return false;
            }

            BluetoothDevice device;
            if ((device = GetDevice(GetBtAdapter(), printerAddress)) == null)
            {
                Console.WriteLine("print error: Not Found InnerPrinter!");
                return false;
            }

            try
            {
                _bluetoothSocket = GetSocket(device);
            }
            catch (IOException e)
            {
                Console.WriteLine("print error: Bluetooth connect failed!");
                return false;
            }

            return true;
        }

        void IBlueToothService.DisconnectBlueTooth()
        {
            if (_bluetoothSocket == null) return;

            try
            {
                var output = _bluetoothSocket.OutputStream;
                output?.Close();
                _bluetoothSocket.Close();
                _bluetoothSocket = null;
            }
            catch (IOException e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        void IBlueToothService.SendData(byte[] bytes)
        {
            if (_bluetoothSocket != null)
            {
                try
                {
                    var output = _bluetoothSocket.OutputStream;
                    output?.Write(bytes, 0, bytes.Length);
                }
                catch (IOException e)
                {
                    Console.WriteLine(e.StackTrace);
                }
            }
            else
            {
                Console.WriteLine("BluetoothSocket null");
            }
        }
    }
}

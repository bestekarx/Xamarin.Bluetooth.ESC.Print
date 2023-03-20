using System.Collections.Generic;
using XamarinESCUtils.Model;

namespace XamarinESCUtils.Platforms.Common
{
    public interface IBlueToothService
    {
        IList<BluetoothInfo> GetDeviceList();
        bool ConnectBlueTooth(string printerAddress);
        void SendData(byte[] bytes);
        void DisconnectBlueTooth();
    }
}

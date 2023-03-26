# Xamarin.Forms Android ESC/POS Bluetooth Printer

## Features

- 58, 80mm bluetooth esc/pos printer support
- Bold font support
- Justify text right, left, center
- Print QR Code
- Print Barcode
- Turkish Character Supported

> The project is completely
>  open source and open to support.
>  Download the Sample Application
>  for details.


The application has been developed for the current Xamarin.Forms android platform.

## Android Manifest Permission
```sh
<uses-permission android:name="android.permission.BLUETOOTH" />
<uses-permission android:name="android.permission.BLUETOOTH_ADMIN" />
<uses-permission android:name="android.permission.BLUETOOTH_CONNECT" />
```
## Note:
> ** Make sure to allow all permissions from application settings before running the project.
## Android Mainactivity.cs
```sh
[assembly: Xamarin.Forms.Dependency(typeof(BluetoothService))]
[assembly: Xamarin.Forms.Dependency(typeof(EscUtil))]
namespace SampleApp.Droid
...
```
# Xamarin Forms
## Init Implementation
```sh
using SampleApp.ViewModels;
using Xamarin.Forms;
using XamarinESCUtils.Platforms.Common;
namespace SampleApp
{
    public partial class MainPage : ContentPage
    {
    	private IBlueToothService _blueToothService;
    	private IEscUtil _escUtil;

		public MainPage()
		{
			InitializeComponent();

			_blueToothService = DependencyService.Get<IBlueToothService>();
			_escUtil = DependencyService.Get<IEscUtil>();
		}
	}
}
```
## Get Bluetooth Device List
```sh
private List <BluetoothInfo> PrinterList = new List<BluetoothInfo>();
....
...
var list = _blueToothService.GetDeviceList();
foreach (var item in list)
{
	PrinterList.Add(item);
}
```
## Selected Bluetooth Printer
```sh
return new Command<object>(async (parameter) =>
{
    try
    {
        if (parameter is BluetoothInfo selectedItem)
        {
            _BluetoothInfo = selectedItem;
        }
    }
    catch (Exception e)
    {
        await App.Current.MainPage.DisplayToastAsync(e.Message);
        throw;
    }
});
```
## Send Printer Data
```sh
if (_BluetoothInfo != null)
{
    _blueToothService.ConnectBlueTooth(_BluetoothInfo.Adress);

    _blueToothService.SendData(_escUtil.SetCodeSystem(_escUtil.CodeParse(20))); // utf-8
    _blueToothService.SendData(_escUtil.UnderlineOff());
    _blueToothService.SendData(_escUtil.BoldOn());
    _blueToothService.SendData(_escUtil.AlignCenter());
    _blueToothService.SendData(_escUtil.GetTextBytes("Welcome to XamarinForms.ESCUtils"));
    _blueToothService.SendData(_escUtil.NextLine(1));
    _blueToothService.SendData(_escUtil.BoldOff());
    _blueToothService.SendData(_escUtil.AlignLeft());
    _blueToothService.SendData(_escUtil.GetTextBytes("This is a test output."));
    _blueToothService.SendData(_escUtil.NextLine(1));
    _blueToothService.SendData(_escUtil.AlignRight());
    _blueToothService.SendData(_escUtil.GetTextBytes("Have a nice day :)"));
    _blueToothService.SendData(_escUtil.NextLine(4));
    _blueToothService.SendData(_escUtil.AlignCenter());
    _blueToothService.SendData(_escUtil.GetPrintQrCode($"https://github.com/bestekarx", 3, 1));
    _blueToothService.SendData(_escUtil.NextLine(1));
}
```

<img src="https://user-images.githubusercontent.com/17545048/227748088-1db14ec9-65f3-40a5-afce-3bb246d0a3c4.jpg" width="400" height="400">
<img src="https://user-images.githubusercontent.com/17545048/227748089-b93d9d18-293d-4dc7-bdbe-1ccfddfe3b4b.jpg" width="400" height="800">


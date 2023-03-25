### Xamarin.Forms Android ESC/POS Bluetooth Printer

- Xamarin.Forms and Xamarin.Android supported.
- Completely open source, open to development.


###Android Manifest Permission

> 
	<uses-permission android:name="android.permission.BLUETOOTH" />
	<uses-permission android:name="android.permission.BLUETOOTH_ADMIN" />
	<uses-permission android:name="android.permission.BLUETOOTH_CONNECT" />

###Android Mainactivity.cs

> 
	[assembly: Xamarin.Forms.Dependency(typeof(BluetoothService))]
	[assembly: Xamarin.Forms.Dependency(typeof(EscUtil))]
	namespace SampleApp.Droid
	......
	
##Xamarin Forms
###Init Implementation
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

###Get Bluetooth Device List
	private List <BluetoothInfo> PrinterList = new List<BluetoothInfo>();
	....
	...
	 var list = _blueToothService.GetDeviceList();
	 foreach (var item in list)
	{
		PrinterList.Add(item);
	}
###Selected Bluetooth Printer
    if (parameter is BluetoothInfo selectedItem)
	{
		_BluetoothInfo = selectedItem;
	}
###Send Printer Data
	_blueToothService.ConnectBlueTooth(_BluetoothInfo.Adress);
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

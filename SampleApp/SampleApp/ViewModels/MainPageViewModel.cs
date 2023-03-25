using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using XamarinESCUtils.Model;
using XamarinESCUtils.Platforms.Common;


namespace SampleApp.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        #region PropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        #endregion

        #region Fields

        private XamarinESCUtils.Platforms.Common.IBlueToothService _blueToothService;
        private XamarinESCUtils.Platforms.Common.IEscUtil _escUtil;

        #endregion

        #region Properties

        private List <BluetoothInfo> _PrinterList = new List<BluetoothInfo>();
        public List<BluetoothInfo> PrinterList
        {
            get => _PrinterList;
            set
            {
                _PrinterList = value; 
                OnPropertyChanged();
            }
        }

        private string _PrinterListInfo = "Please select a printer";
        public string PrinterListInfo
        {
            get => _PrinterListInfo;
            set
            {
                _PrinterListInfo = value;
                OnPropertyChanged();
            }
        }

        private BluetoothInfo _BluetoothInfo;
        #endregion

        #region Ctor

        public MainPageViewModel()
        {
            Init();
        }

        #endregion

        #region Methods

        private async Task Init()
        {
            try
            {
                _blueToothService = DependencyService.Get<IBlueToothService>();
                _escUtil = DependencyService.Get<IEscUtil>();

                var list = _blueToothService.GetDeviceList();

                if (list.Count <= 0)
                {
                    await App.Current.MainPage.DisplayToastAsync("Bluetooth printer not found.");
                    PrinterListInfo = "Printer not found";
                    return;
                }

                foreach (var item in list)
                    PrinterList.Add(item);

                OnPropertyChanged(nameof(PrinterList));
            }
            catch (Exception e)
            {
                await App.Current.MainPage.DisplayToastAsync(e.Message);
                throw;
            }
        }

        #endregion

        #region Commands

        public ICommand PrinterSelected
        {
            get
            {
                return new Command<object>(async (parameter) =>
                {
                    try
                    {
                        if (parameter is BluetoothInfo selectedItem)
                        {
                            _BluetoothInfo = selectedItem;
                            PrinterListInfo = $"Selected printer: {_BluetoothInfo.Name}";
                            await App.Current.MainPage.DisplayToastAsync($"Selected printer: {_BluetoothInfo.Name}");
                        }
                    }
                    catch (Exception e)
                    {
                        await App.Current.MainPage.DisplayToastAsync(e.Message);
                        throw;
                    }
                });
            }
        }

        public ICommand OnPrintTest
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
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
                    }
                    catch (Exception e)
                    {
                        await App.Current.MainPage.DisplayToastAsync(e.Message);
                        throw;
                    }
                    finally
                    {
                        _blueToothService.DisconnectBlueTooth();
                        await App.Current.MainPage.DisplayToastAsync("Printer successfully !!!");
                    }
                });
            }
        }


        #endregion
    }
}

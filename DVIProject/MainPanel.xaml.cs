using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Toolkit.Mvvm;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;
using DVIProject.DVIService;
using Timer = System.Timers.Timer;

namespace DVI
{
    public partial class MainPanel : Window
    {
        private readonly monitorSoapClient _client = new();
        
        private readonly Timer _soapTimer = new System.Timers.Timer(50000); //Update Temp and Storage
        public readonly Timer ClockTimer = new System.Timers.Timer(100); //Update the clock

        private readonly Timer _clockSwitch = new System.Timers.Timer(10000); //When to switch timezone
        public int TimeZoneId = 0;
        public readonly string[] TimeZonesArr = { "Romance Standard Time", "GMT Standard Time", "Singapore Standard Time", "AUS Eastern Standard Time", "Pacific Standard Time", "Eastern Standard Time" };
        //public readonly string[] TimeZonesCity = { "Copenhagen", "London", "Singapore", "Melbourne", "US Pacific", "US Eastern" };

        private readonly Vm _vm = new(); //Viewmodel

        public MainPanel()
        {
            this.DataContext = _vm;
            InitializeComponent();
            TimersSetup();
            Clock();
            ClientUpdate();
            
        }

        public void TimersSetup()
        {
            ClockTimer.AutoReset = true;
            ClockTimer.Start();
            _clockSwitch.AutoReset = true;
            _clockSwitch.Start();
            _soapTimer.AutoReset = true;
            _soapTimer.Start();
        }

        public DateTime Clock()
        {
            var rnd = new Random();

            _clockSwitch.Elapsed += (s, e) => {
                TimeZoneId = rnd.Next(1, TimeZonesArr.Length);
            };


            var startTime = DateTime.UtcNow;
            var tst = TimeZoneInfo.FindSystemTimeZoneById(TimeZonesArr[TimeZoneId]);


            return TimeZoneInfo.ConvertTime(startTime, TimeZoneInfo.Utc, tst);
        }

        public void ClientUpdate()
        {
            //Clock
            ClockTimer.Elapsed += (s, e) => _vm.Time = Clock();


            // FIX !!!!!!!!!!!!!!!!!! - Time not matching timezone name
            _clockSwitch.Elapsed += (s, e) => _vm.CurrentTimeId = TimeZoneId;
            _clockSwitch.Elapsed += (s, e) => _vm.CurrentTimeZone = TimeZonesArr[TimeZoneId]; 


            _soapTimer.Elapsed += (s, e) =>
            {
                _vm.OutDoorH = _client.OutdoorHumidity();
                _vm.OutDoorT = _client.OutdoorTemp();
                _vm.StockH = _client.StockHumidity();
                _vm.StockT = _client.StockTemp();
                _vm.StockOverMax = new ObservableCollection<string>(_client.StockItemsOverMax());
                _vm.StockMostSold = new ObservableCollection<string>(_client.StockItemsMostSold());
            };
        }
    }

    public class Vm : ObservableObject
    {
        private string _currentTimeZone;

        public string CurrentTimeZone
        {
            get => _currentTimeZone;
            set => SetProperty(ref _currentTimeZone, value);
        }

        private int _currentTimeId;
        public int CurrentTimeId
        {
            get => _currentTimeId;
            set => SetProperty(ref _currentTimeId, value);
        }

        private DateTime _time;
        public DateTime Time
        {
            get => _time; 
            set => SetProperty(ref _time, value);
        }

        private double _outDoorH;
        public double OutDoorH
        {
            get => _outDoorH;
            set => SetProperty(ref _outDoorH, value);
        }

        private double _outDoorT;
        public double OutDoorT
        {
            get => _outDoorT;
            set => SetProperty(ref _outDoorT, value);
        }

        private double _stockH;
        public double StockH
        {
            get => _stockH;
            set => SetProperty(ref _stockH, value);
        }
        private double _stockT;
        public double StockT
        {
            get => _stockT;
            set => SetProperty(ref _stockT, value);
        }
        
        private ObservableCollection<string> _stockOverMax;
        public ObservableCollection<string> StockOverMax
        {
            get => _stockOverMax;
            set => SetProperty(ref _stockOverMax, value);
        }

        private ObservableCollection<string> _stockMostSold;
        public ObservableCollection<string> StockMostSold
        {
            get => _stockMostSold;
            set => SetProperty(ref _stockMostSold, value);
        }
        
    }
}
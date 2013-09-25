namespace UARTMon {
    using System;
    using System.ComponentModel;
    using System.IO.Ports;
    using System.Reflection;

    internal sealed class SerialMonitor {
        private SerialPort _port;

        public SerialMonitor() {
            UpdatePortList();
        }

        public SerialSettings Settings { get; private set; }
        public event EventHandler<SerialDataEventArgs> NewSerialDataRecieved;

        ~SerialMonitor() {
            Dispose(false);
        }

        public void Dispose(bool disposing = true) {
            if(disposing)
                _port.DataReceived -= SerialPortDataReceived;
            if(_port == null)
                return;
            if(_port.IsOpen)
                _port.Close();
            _port.Dispose();
        }

        private void UpdatePortList() {
            Settings = new SerialSettings {
                                          PortNameCollection = SerialPort.GetPortNames()
                                          };
            Settings.PropertyChanged += SettingsPropertyChanged;
            if(Settings.PortNameCollection.Length > 0)
                Settings.PortName = Settings.PortNameCollection[0];
        }

        private void SettingsPropertyChanged(object sender, PropertyChangedEventArgs e) {
            if(e.PropertyName.Equals("PortName"))
                UpdateBaudRateCollection();
        }

        public void StopListening() {
            if(_port != null && _port.IsOpen)
                _port.Close();
        }

        public void StartListening() {
            if(string.IsNullOrEmpty(Settings.PortName))
                return;
            if(_port != null && _port.IsOpen)
                _port.Close();
            _port = new SerialPort(Settings.PortName, Settings.BaudRate, Settings.Parity, Settings.DataBits, Settings.StopBits);
            _port.DataReceived += SerialPortDataReceived;
            _port.Open();
        }

        private void SerialPortDataReceived(object sender, SerialDataReceivedEventArgs e) {
            var dataLength = _port.BytesToRead;
            var data = new byte[dataLength];
            var nbrDataRead = _port.Read(data, 0, dataLength);
            if(nbrDataRead == 0)
                return;
            if(NewSerialDataRecieved != null)
                NewSerialDataRecieved(this, new SerialDataEventArgs(data));
        }

        private void UpdateBaudRateCollection() {
            _port = new SerialPort(Settings.PortName);
            _port.Open();
            var tmp = _port.BaseStream.GetType().GetField("commProp", BindingFlags.Instance | BindingFlags.NonPublic);
            if(tmp == null)
                return;
            var p = tmp.GetValue(_port.BaseStream);
            tmp = p.GetType().GetField("dwSettableBaud", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if(tmp == null)
                return;
            var dwSettableBaud = (Int32) tmp.GetValue(p);
            _port.Close();
            Settings.UpdateBaudRateCollection(dwSettableBaud);
        }
    }

    internal sealed class SerialSettings : INotifyPropertyChanged {
        private int _baud = 115200;
        private int _bits = 8;
        private Parity _parity = Parity.None;
        private string _port = "";
        private StopBits _stop = StopBits.One;

        #region Properties

        public string PortName {
            get { return _port; }
            set {
                if(_port.Equals(value))
                    return;
                _port = value;
                SendPropertyChangedEvent("PortName");
            }
        }

        public int BaudRate {
            get { return _baud; }
            set {
                if(_baud == value)
                    return;
                _baud = value;
                SendPropertyChangedEvent("BaudRate");
            }
        }

        public Parity Parity {
            get { return _parity; }
            set {
                if(_parity == value)
                    return;
                _parity = value;
                SendPropertyChangedEvent("Parity");
            }
        }

        public int DataBits {
            get { return _bits; }
            set {
                if(_bits == value)
                    return;
                _bits = value;
                SendPropertyChangedEvent("DataBits");
            }
        }

        public StopBits StopBits {
            get { return _stop; }
            set {
                if(_stop == value)
                    return;
                _stop = value;
                SendPropertyChangedEvent("StopBits");
            }
        }

        public string[] PortNameCollection { get; set; }

        private BindingList<int> BaudRateCollection { get; set; }

        private int[] DataBitsCollection { get; set; }

        #endregion Properties

        public SerialSettings() {
            DataBitsCollection = new[] {
                                       5, 6, 7, 8
                                       };
            BaudRateCollection = new BindingList<int>();
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        public void UpdateBaudRateCollection(int possibleBaudRates) {
            const int baud075 = 0x00000001;
            const int baud110 = 0x00000002;
            const int baud150 = 0x00000008;
            const int baud300 = 0x00000010;
            const int baud600 = 0x00000020;
            const int baud1200 = 0x00000040;
            const int baud1800 = 0x00000080;
            const int baud2400 = 0x00000100;
            const int baud4800 = 0x00000200;
            const int baud7200 = 0x00000400;
            const int baud9600 = 0x00000800;
            const int baud14400 = 0x00001000;
            const int baud19200 = 0x00002000;
            const int baud38400 = 0x00004000;
            const int baud56K = 0x00008000;
            const int baud57600 = 0x00040000;
            const int baud115200 = 0x00020000;
            const int baud128K = 0x00010000;

            BaudRateCollection.Clear();

            if((possibleBaudRates & baud075) > 0)
                BaudRateCollection.Add(75);
            if((possibleBaudRates & baud110) > 0)
                BaudRateCollection.Add(110);
            if((possibleBaudRates & baud150) > 0)
                BaudRateCollection.Add(150);
            if((possibleBaudRates & baud300) > 0)
                BaudRateCollection.Add(300);
            if((possibleBaudRates & baud600) > 0)
                BaudRateCollection.Add(600);
            if((possibleBaudRates & baud1200) > 0)
                BaudRateCollection.Add(1200);
            if((possibleBaudRates & baud1800) > 0)
                BaudRateCollection.Add(1800);
            if((possibleBaudRates & baud2400) > 0)
                BaudRateCollection.Add(2400);
            if((possibleBaudRates & baud4800) > 0)
                BaudRateCollection.Add(4800);
            if((possibleBaudRates & baud7200) > 0)
                BaudRateCollection.Add(7200);
            if((possibleBaudRates & baud9600) > 0)
                BaudRateCollection.Add(9600);
            if((possibleBaudRates & baud14400) > 0)
                BaudRateCollection.Add(14400);
            if((possibleBaudRates & baud19200) > 0)
                BaudRateCollection.Add(19200);
            if((possibleBaudRates & baud38400) > 0)
                BaudRateCollection.Add(38400);
            if((possibleBaudRates & baud56K) > 0)
                BaudRateCollection.Add(56000);
            if((possibleBaudRates & baud57600) > 0)
                BaudRateCollection.Add(57600);
            if((possibleBaudRates & baud115200) > 0)
                BaudRateCollection.Add(115200);
            if((possibleBaudRates & baud128K) > 0)
                BaudRateCollection.Add(128000);

            SendPropertyChangedEvent("BaudRateCollection");
        }

        private void SendPropertyChangedEvent(string propertyName) {
            var handler = PropertyChanged;
            if(handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    internal sealed class SerialDataEventArgs : EventArgs {
        private readonly byte[] _data;

        public SerialDataEventArgs(byte[] dataInByteArray) {
            _data = dataInByteArray;
        }

        public byte[] Data {
            get { return _data; }
        }
    }
}
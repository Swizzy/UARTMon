namespace UARTMon {
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Text;

    internal class Program {
        private static bool _dolog, _lowspeed;
        private static StreamWriter _logger;
        private static string _logname;

        private static void Main(string[] args) {
            if(args.Length < 1) {
                Console.WriteLine("You must specify COM port to use!");
                return;
            }
            var monitor = new SerialMonitor();
            var settings = monitor.Settings;
            monitor.NewSerialDataRecieved += MonitorOnNewSerialDataRecieved;
            foreach(var port in settings.PortNameCollection) {
                if(port != args[0].ToUpper())
                    continue;
                settings.PortName = port;
                break;
            }
            if(string.IsNullOrEmpty(settings.PortName)) {
                Console.WriteLine("Invalid port specified! Try again...");
                return;
            }
            if(args.Length >= 2) {
                foreach(var s in args) {
                    if(s.Equals("-lowspeed"))
                        _lowspeed = true;
                    else if(!_dolog) {
                        _logger = new StreamWriter(s) {
                                                      AutoFlush = true
                                                      };
                        _logname = s;
                        _dolog = true;
                    }
                }
            }
            Console.WriteLine("Starting to listen on port: {0}", args[0]);
            if(_dolog)
                Console.WriteLine("A Log will be saved to: {0}", _logname);
            monitor.SetSpeed(_lowspeed);
            monitor.StartListening();
            string line;
            do
                line = Console.ReadLine();
            while(line == null || !line.Equals("stop", StringComparison.CurrentCultureIgnoreCase) && !line.Equals("restart", StringComparison.CurrentCultureIgnoreCase));
            if(line.Equals("restart", StringComparison.CurrentCultureIgnoreCase))
                Process.Start(Assembly.GetExecutingAssembly().CodeBase, args.Length == 1 ? args[0] : string.Format("{0} \"{1}\"", args[0], args[1]));
        }

        private static void MonitorOnNewSerialDataRecieved(object sender, SerialDataEventArgs e) {
            var tmp = Encoding.UTF8.GetString(e.Data);
            Console.Write(tmp);
            if(_dolog && _logger != null)
                _logger.Write(tmp);
        }
    }
}
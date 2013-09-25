namespace UARTMon
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Text;

    class Program
    {
        static bool _dolog;
        static StreamWriter _logger;
        static void Main(string[] args) {

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
                _logger = new StreamWriter(args[1]) {
                                                    AutoFlush = true
                                                    };
                _dolog = true;
            }
            Console.WriteLine("Starting to listen on port: {0}", args[0]);
            if(_dolog)
                Console.WriteLine("A Log will be saved to: {0}", args[1]);
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
            if (_dolog && _logger != null)
                _logger.Write(tmp);
        }
    }
}

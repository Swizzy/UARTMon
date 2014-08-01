namespace UARTMon {
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Text;

    internal class Program {
        private static bool _doLog, _lowSpeed, _useDemon;
        private static StreamWriter _logger;
        private static string _logName;
        private static SerialMonitor _monitor;
        private static Demon _demon;

        private static Assembly CurrentDomainAssemblyResolve(object sender, ResolveEventArgs args) {
            if(string.IsNullOrEmpty(args.Name))
                throw new Exception("DLL Read Failure (Nothing to load!)");
            var name = string.Format("{0}.dll", args.Name.Split(',')[0]);
            using(var stream = Assembly.GetAssembly(typeof(Program)).GetManifestResourceStream(string.Format("{0}.{1}", typeof(Program).Namespace, name))) {
                if(stream == null)
                    throw new Exception(string.Format("Can't find external nor internal {0}!", name));
                var data = new byte[stream.Length];
                stream.Read(data, 0, data.Length);
                return Assembly.Load(data);
            }
        }

        private static void Main(string[] args) {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomainAssemblyResolve;
            if(args.Length < 1) {
                Console.WriteLine("You must specify COM port to use!");
                return;
            }

            if(args[0].Equals("demon", StringComparison.CurrentCultureIgnoreCase)) {
                if(!StartDemon()) {
                    Console.WriteLine("Failed to communicate with your Demon!");
                    return;
                }
            }
            else {
                _monitor = new SerialMonitor();
                _monitor.NewSerialDataRecieved += MonitorOnNewSerialDataRecieved;
                foreach(var port in _monitor.Settings.PortNameCollection) {
                    if(port != args[0].ToUpper())
                        continue;
                    _monitor.Settings.PortName = port;
                    break;
                }
                if(string.IsNullOrEmpty(_monitor.Settings.PortName)) {
                    Console.WriteLine("Invalid port specified! Try again...");
                    return;
                }
            }
            if(args.Length >= 2) {
                foreach(var s in args) {
                    if(s.Equals("-lowspeed"))
                        _lowSpeed = true;
                    else if(!_doLog) {
                        _logger = new StreamWriter(s) {
                                                          AutoFlush = true
                                                      };
                        _logName = s;
                        _doLog = true;
                    }
                }
            }
            Console.WriteLine("Starting to listen on port: {0}", args[0]);
            if(_doLog)
                Console.WriteLine("A Log will be saved to: {0}", _logName);
            if(!_useDemon) {
                _monitor.SetSpeed(_lowSpeed);
                _monitor.StartListening();
            }
            string line;
            do
                line = Console.ReadLine();
            while(line == null || !line.Equals("stop", StringComparison.CurrentCultureIgnoreCase) && !line.Equals("restart", StringComparison.CurrentCultureIgnoreCase));
            {
                if(line.Equals("stop", StringComparison.CurrentCultureIgnoreCase)) {
                    if(!_useDemon)
                        _monitor.StopListening();
                    else
                        _demon.Stop();
                }
                if(!line.Equals("restart", StringComparison.CurrentCultureIgnoreCase))
                    return;
                if(!_useDemon)
                    _monitor.StopListening();
                else
                    _demon.Stop();
                Process.Start(Assembly.GetExecutingAssembly().CodeBase, args.Length == 1 ? args[0] : string.Format("{0} \"{1}\"", args[0], args[1]));
            }
        }

        private static bool StartDemon() {
            _useDemon = true;
            _demon = new Demon();
            _demon.NewSerialDataRecieved += MonitorOnNewSerialDataRecieved;
            return _demon.Start();
        }

        private static void MonitorOnNewSerialDataRecieved(object sender, SerialDataEventArgs e) {
            var tmp = Encoding.UTF8.GetString(e.Data);
            Console.Write(tmp);
            if(_doLog && _logger != null)
                _logger.Write(tmp);
        }
    }
}